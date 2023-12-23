using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;

namespace CMSWebsite.Controllers
{
    public class CMS_ProductController : CMS_BaseController
    {

        /*
            PositionInt  = 1 : Dọc , = 0 : Ngang 
             
        */

        PhotoAlbumEntities db = new PhotoAlbumEntities();

        #region [Sản phẩm]

        public ActionResult ProductManage()
        {
            InitView();
            ProductDataModel model = new ProductDataModel();
            model.LstCate = db.CMS_CateProduct.Where(o => o.Active == true).ToList();
          
            model.LstProView = GetProductView();
            return View(model);
        }

        public List<ProductViewModel> GetProductView()
        {
            List<ProductViewModel> result = new List<ProductViewModel>();
            ProductViewModel temp;
            var lstPro = db.CMS_Product.Where(o => o.Active == true).OrderByDescending(o => o.ProductID).ToList();
            var lstCate = db.CMS_CateProduct.Where(o => o.Active == true).ToList();
            foreach (var item in lstPro)
            {
                temp = new ProductViewModel();
                temp.PriceRetail = item.PriceRetail;
                temp.PriceRetailStr = ConvertDot(item.PriceRetail.ToString());
                temp.ProductCate = item.CateID;
                temp.ProductCateStr = lstCate.Where(o => item.CateID.Contains("," + o.CategoyID + ",")).Select(o => o.Name).FirstOrDefault();
                temp.ProductCode = item.ProductCode;
                temp.ProductID = item.ProductID;
                temp.ProductName = item.Name;
                temp.Status = item.IsOnSale == true ? "Đang kinh doanh" : "Ngừng kinh doanh";
                temp.ProductImg = item.Images;
                result.Add(temp);
            }
            return result;
        }


        public ActionResult SaveNewProducts(ProductEditData pro)
        {
            InitView();
            try
            {
                CMS_Product newProduct = new CMS_Product();
                newProduct.ProductCode = pro.ProductCode;
                newProduct.Active = true;
                newProduct.CateID = "," + pro.Category + ",";
                newProduct.CreateBy = this.UserID;
                newProduct.CreateDate = DateTime.Now;
                newProduct.Images = pro.HidImg == null ? string.Empty : pro.HidImg;
                newProduct.IsOnSale = true;
                newProduct.ImageHover = pro.HidImg2 == null ? string.Empty : pro.HidImg2;
                newProduct.Gallery = pro.Gallery == null ? string.Empty : pro.Gallery;
                newProduct.Note = "";
                newProduct.ProDescription = pro.ProductDescription == null ? string.Empty : pro.ProductDescription;
                newProduct.ProSumary = pro.ProductSummary == null ? string.Empty : pro.ProductSummary;
                newProduct.IsTarget = true;//Convert.ToBoolean(ProductModel.IsTarget);
                newProduct.PriceRetail = Convert.ToDouble(pro.PriceRetailEdit.ToString().Replace(".", ""));
                newProduct.PriceSource = Convert.ToInt32(pro.PriceOri.Replace(".", ""));
                newProduct.Name = pro.ProductName;
                newProduct.URLFriendly = "";
                newProduct.PriceRetailStr = pro.PriceRetailEdit;
                newProduct.CountSaled = 0;
                newProduct.IsSlider = pro.IsSlider == 1;
                newProduct.IsFeature = pro.IsFeature == 1;
                db.CMS_Product.Add(newProduct);
                db.SaveChanges();
                
                newProduct.URLFriendly = "/san-pham/" + ToFriendlyUrl(newProduct.Name) + "-" + newProduct.ProductID;
                db.SaveChanges();
                string detail = "Thêm mới sản phẩm: " + newProduct.Name;
                WriteLogToDB(this.UserID,this.Fullname,"Thêm mới sản phẩm", detail, DateTime.Now);
            }
            catch (Exception ex)
            {
                return Content("Nhập sai định dạng. Mời nhập lại !");
            }

            return PartialView("_ListProducts", GetProductView());
        }

        public ActionResult GetProductByID(int productID)
        {
            ProductDataModal model = new ProductDataModal();

            model.ProEdit = db.CMS_Product.Where(o => o.ProductID == productID).FirstOrDefault();
            model.LstCate = db.CMS_CateProduct.Where(o => o.Active == true).ToList();

            return View("_ModalEditProduct", model);
        }

        [HttpPost]
        public ActionResult SaveEditProducts(ProductEditData pro)
        {
            InitView();
            try
            {
                int id = pro.ProductID;
                CMS_Product newProduct = db.CMS_Product.Where(o => o.ProductID == id).FirstOrDefault();
                newProduct.ProductCode = pro.ProductCode;
                newProduct.CateID = ","+ pro.Category + ",";
                newProduct.UpdateBy = this.UserID;
                newProduct.UpdateDate = DateTime.Now;
                newProduct.Images = pro.HidImg == null ? string.Empty : pro.HidImg;
                newProduct.ImageHover = pro.HidImg2 == null ? string.Empty : pro.HidImg2;
                newProduct.Gallery = pro.Gallery == null ? string.Empty : pro.Gallery;
                newProduct.ProDescription = pro.ProductDescription == null ? string.Empty : pro.ProductDescription;
                newProduct.PriceRetail = Convert.ToDouble(pro.PriceRetailEdit.ToString().Replace(".", ""));
                newProduct.Name = pro.ProductName;
                newProduct.PriceRetailStr = pro.PriceRetailEdit;
                newProduct.PriceSource = Convert.ToInt32(pro.PriceOri.Replace(".", ""));
                newProduct.ProSumary = pro.ProductSummary == null ? string.Empty : pro.ProductSummary;
                newProduct.IsSlider = pro.IsSlider == 1;
                newProduct.IsFeature = pro.IsFeature == 1;
                db.SaveChanges();

                newProduct.URLFriendly = "/san-pham/" + ToFriendlyUrl(newProduct.Name) + "-" + newProduct.ProductID;
                db.SaveChanges();
               
                string detail = "Cập nhật sản phẩm: " + newProduct.Name;
                WriteLogToDB(this.UserID, this.Fullname, "Cập nhật sản phẩm", detail, DateTime.Now);
            }
            catch (Exception ex)
            {
                Content("Nhập sai định dạng. Mời nhập lại !");
            }

            return PartialView("_ListProducts", GetProductView());
        }

        // Xóa 
        public ActionResult DeleteProductByID(int productID)
        {
            CMS_Product stop = db.CMS_Product.Where(o => o.ProductID == productID).FirstOrDefault();
            if(stop != null)
            {
                stop.Active = false;
                db.SaveChanges();
            }

            return PartialView("_ListProducts", GetProductView());
        }

        // Modal
        public ActionResult _ModalNewProduct()
        {
            ProductDataModal model = new ProductDataModal();
            model.LstCate =  db.CMS_CateProduct.Where(o => o.Active == true).ToList();
            return View("_ModalNewProduct",model);
        }

        #endregion

        #region [Loai-san-pham]

        // GET: CMS_Product
        public ActionResult ProductCategory()
        {
            InitView();
            ProductCategoryModel model = new ProductCategoryModel();
            model.LstCate = db.CMS_CateProduct.Where(o => o.Active == true).ToList();
            return View(model);
        }

        public ActionResult AddCategory(string cateName, int parentId, string currentImg)
        {
            InitView();
            try
            {
                CMS_CateProduct addNew = new CMS_CateProduct();
                addNew.Name = cateName;
                addNew.ParentID = parentId;
                addNew.Active = true;
                addNew.ImgHomePage = currentImg;
                addNew.CateURL = ToFriendlyUrl(cateName);
                db.CMS_CateProduct.Add(addNew);
                db.SaveChanges();

                WriteLogToDB(this.UserID, this.Fullname, "Thêm danh mục sản phẩm", "Thêm mới danh mục : " + addNew.Name, DateTime.Now);

                return View("_TblProductsCate", db.CMS_CateProduct.Where(o => o.Active == true).ToList());
            }
            catch (Exception ex)
            {
                return Content("Error");
            }
        }
        
        public ActionResult GetParentCategory()
        {
            return View("_SelectParentCate", db.CMS_CateProduct.Where(o => o.Active == true && o.ParentID == 0).ToList());
        }


        public ActionResult EditCateById(int cId, string cateName, int pId, string currentImg)
        {
            try
            {
                CMS_CateProduct editCate = db.CMS_CateProduct.Where(o => o.CategoyID == cId).FirstOrDefault();
                editCate.Name = cateName;
                editCate.CateURL = ToFriendlyUrl(cateName);
                editCate.ImgHomePage = currentImg;
                editCate.ParentID = pId;
                db.SaveChanges();
                return View("_TblProductsCate", db.CMS_CateProduct.Where(o => o.Active == true).ToList());
            }
            catch (Exception ex)
            {
                return Content("Error");
            }
        }

        public ActionResult DeleteCategory(int cateId)
        {
            try
            {
                CMS_CateProduct model = db.CMS_CateProduct.Where(o => o.CategoyID == cateId).FirstOrDefault();
                var lstChild = db.CMS_CateProduct.Where(o => o.ParentID == cateId).ToList();
                foreach (var item in lstChild)
                {
                    item.ParentID = 0;
                }
                db.CMS_CateProduct.Remove(model);
                db.SaveChanges();
                return View("_TblProductsCate", db.CMS_CateProduct.Where(o => o.Active == true).ToList());
            }
            catch (Exception ex)
            {
                return Content("Error");
            }
        }
        #endregion 
        
    }
}