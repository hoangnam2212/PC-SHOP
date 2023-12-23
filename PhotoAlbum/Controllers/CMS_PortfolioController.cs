using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;

namespace CMSWebsite.Controllers
{
    public class CMS_PortfolioController : CMS_BaseController
    {
        PhotoAlbumEntities db = new PhotoAlbumEntities();

        #region [Tất cả dự án]

        public ActionResult AllPortfolio()
        {
            InitView();
            List<PortfolioView> model = new List<PortfolioView>();
            try
            {
                List<Web_Portfolio> lstPortfolio = new List<Web_Portfolio>();
                lstPortfolio = db.Web_Portfolio.Where(o => o.Active == true).OrderByDescending(o => o.CreateDate.Value).ToList();
                List<Web_PortfolioCategory> arr = new List<Web_PortfolioCategory>();
                PortfolioView temp;
                int authorID = 0;
                for (int i = 0; i < lstPortfolio.Count; i++)
                {
                    temp = new PortfolioView();
                    authorID = lstPortfolio[i].Author.Value;
                    temp.PortfolioAuthor = dbCMS.CMS_Users.Where(o => o.UserID == authorID).Select(o => o.FullName).FirstOrDefault();

                    temp.PortfolioDate = Convert.ToDateTime(lstPortfolio[i].CreateDate).ToString("dd/MM/yyyy lúc HH:mm");
                    
                    temp.ImgFace = lstPortfolio[i].ImageFace;
                    temp.UrlFriendly = lstPortfolio[i].URLFriendly;
                    temp.PortfolioId = lstPortfolio[i].PortfolioId;
                    temp.PortfolioTitle = lstPortfolio[i].Title;
                    temp.PortfolioCate = "";
                    arr = new List<Web_PortfolioCategory>();
                    authorID = lstPortfolio[i].PortfolioId;
                    arr = db.Web_PortfolioCategory.Where(o => o.PortfolioID == authorID).ToList();
                    foreach (var item in arr)
                    {
                        temp.PortfolioCate += db.Web_CategoryPortfolio.Where(o => o.CategoryId == item.CateID).Select(o => o.CategoryName).FirstOrDefault() + ", ";
                    }
                    temp.PortfolioCate = temp.PortfolioCate.Substring(0, temp.PortfolioCate.Length - 2);
                    model.Add(temp);
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        #endregion

        #region [Thêm mới dự án]

        public ActionResult AddPortfolio()
        {
            InitView();

            return View(GetCategoryPortfolios());
        }

        // Thêm mới dự án
        [HttpPost]
        public ActionResult AddNewPortfolio(PortfolioModel models)
        {
            InitView();
            try
            {
                Web_Portfolio newPortfolio = new Web_Portfolio();
                newPortfolio.Author = this.UserID;
                newPortfolio.Active = true;
                newPortfolio.Content = models.ContentPortfolio;
                newPortfolio.CreateDate = DateTime.Now;
                newPortfolio.Description = models.Description;
                newPortfolio.ImageFace = models.Image;
                newPortfolio.ImgGallery = models.ImageGallery;
                newPortfolio.SeoDesc = models.SeoDesc;
                newPortfolio.SeoKeyword = models.SeoKeyword;
                newPortfolio.Title = models.Title;
                newPortfolio.URLFriendly = "";
                db.Web_Portfolio.Add(newPortfolio);
                db.SaveChanges();
                newPortfolio.URLFriendly = "du-an/" + ToFriendlyUrl(models.Title) + "-" + newPortfolio.PortfolioId;
                db.SaveChanges();
                string[] cateSplit = models.CatePortfolio.Split(',');
                for (int i = 0; i < cateSplit.Length - 1; i++)
                {
                    Web_PortfolioCategory portfolioCate = new Web_PortfolioCategory();
                    portfolioCate.CateID = Convert.ToInt32(cateSplit[i]);
                    portfolioCate.PortfolioID = newPortfolio.PortfolioId;
                    dbCMS.Web_PortfolioCategory.Add(portfolioCate);

                }
                dbCMS.SaveChanges();
                WriteLogToDB(this.UserID, this.Fullname, "Thêm mới dự án", this.Fullname + " tạo dự án mới : " + newPortfolio.Title, DateTime.Now);
                return Content("OK");
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }

        #endregion

        #region [Danh mục dự án]

        public ActionResult CategoryPortfolio()
        {
            InitView();

            PortfolioCategoryModel model = new PortfolioCategoryModel();
            
            model.LstCate = GetCategoryPortfolios();

            return View(model);
        }

        // Thêm mới loại dự án
        public ActionResult AddCategory(string cateName, int parentId)
        {
            InitView();
            try
            {
                Web_CategoryPortfolio addNew = new Web_CategoryPortfolio();
                addNew.CategoryName = cateName;
                addNew.ParentId = parentId;
                addNew.Active = true;
                addNew.CateIcon = "";
                addNew.CateImg = "";
                addNew.CateURL = ToFriendlyUrl(cateName);
                db.Web_CategoryPortfolio.Add(addNew);
                db.SaveChanges();

                WriteLogToDB(this.UserID, this.Fullname, "Thêm mới loại dự án", "Thêm mới loại dự án : " + addNew.CategoryName, DateTime.Now);

                return View("_TblPostCate", GetCategoryPortfolios());
            }
            catch (Exception ex)
            {
                // WriteFileLog.WriteLog(string.Empty, ex);
                return Content("Error");
            }
        }

        public ActionResult GetParentCategory()
        {
            return View("_SelectParentCate", db.Web_CategoryPortfolio.Where(o => o.Active == true && o.ParentId == 0).ToList());
        }

        // Sửa loại dự án
        public ActionResult EditCateById(int cId, string cateName, int pId)
        {
            InitView();
            try
            {
                Web_CategoryPortfolio editCate = db.Web_CategoryPortfolio.Where(o => o.CategoryId == cId).FirstOrDefault();
                editCate.CategoryName = cateName;
                editCate.CateURL = ToFriendlyUrl(cateName);
                editCate.ParentId = pId;
                db.SaveChanges();

                WriteLogToDB(this.UserID, this.Fullname, "Cập nhật loại dự án", "Thay đổi loại dự án : " + editCate.CategoryName, DateTime.Now);
                return View("_TblPostCate", GetCategoryPortfolios());
            }
            catch (Exception ex)
            {
                return Content("Error");
            }
        }

        // Xóa danh mục dự án
        public ActionResult DeleteCategory(int cateId)
        {
            try
            {
                Web_CategoryPortfolio model = db.Web_CategoryPortfolio.Where(o => o.CategoryId == cateId).FirstOrDefault();
                var lstChild = db.Web_CategoryPortfolio.Where(o => o.ParentId == cateId).ToList();
                foreach (var item in lstChild)
                {
                    item.ParentId = 0;
                }
                model.Active = false;
                db.SaveChanges();

                WriteLogToDB(this.UserID, this.Fullname, "Xóa loại dự án", "Xóa loại dự án : " + model.CategoryName, DateTime.Now);

                return View("_TblPostCate", GetCategoryPortfolios());
            }
            catch (Exception ex)
            {
                return Content("Error");
            }
        }

        public List<Web_CategoryPortfolio> GetCategoryPortfolios()
        {
            List<Web_CategoryPortfolio> lstCate = new List<Web_CategoryPortfolio>();
            List<Web_CategoryPortfolio> lstCateOrder = new List<Web_CategoryPortfolio>();
            lstCate = db.Web_CategoryPortfolio.Where(o => o.Active == true && o.ParentId == 0).ToList();
            foreach (var item in lstCate)
            {
                lstCateOrder.Add(item);
                lstCateOrder.AddRange(db.Web_CategoryPortfolio.Where(o => o.Active == true && o.ParentId == item.CategoryId));
            }
            return lstCateOrder;
        }

        #endregion 
    }
}