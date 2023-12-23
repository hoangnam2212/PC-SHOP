using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;

namespace CMSWebsite.Controllers
{
    public class Web_RouterController : CMS_BaseController
    {
        PhotoAlbumEntities db = new PhotoAlbumEntities();
        // GET: Web_Router
        public ActionResult Index(string seourl)
        {
            switch (seourl)
            {
                case "lien-he":
                    return PartialView("Contact");
                case "gioi-thieu":
                    return PartialView("AboutUs");
                case "dang-ky-thiet-ke":
                    return PartialView("RegistryInterior");
                case "dich-vu":
                    return PartialView("Services");
                default:
                    return PartialView("Page404");
            }
        }

        //public ActionResult Portfolio(int id, string seourl)
        //{
        //    DetailPortfolioModel model = new DetailPortfolioModel();
        //    model.Portfolio = db.Web_Portfolio.Where(o => o.PortfolioId == id && o.Active == true).FirstOrDefault();
        //    model.LstRelate = db.Web_Portfolio.Where(o => o.PortfolioId != id && o.Active == true).OrderByDescending(o => o.PortfolioId).Take(3).ToList();
        //    int cateID = db.Web_PortfolioCategory.Where(o => o.PortfolioID == id).Select(o => o.CateID).FirstOrDefault();
        //    model.Cate = db.Web_CategoryPortfolio.Where(o => o.CategoryId == cateID).FirstOrDefault();
        //    //model.Previous = db.Web_Portfolio.Where(o => o.Active == true && o.PortfolioId < id).OrderByDescending(o => o.PortfolioId).FirstOrDefault();
        //    //model.Next = db.Web_Portfolio.Where(o => o.Active == true && o.PortfolioId > id).FirstOrDefault();
        //    return View(model);
        //}

        public ActionResult Post(int id, string seourl)
        {
            DetailPostModel model = new DetailPostModel();
            model.Post = db.Web_Post.Where(o => o.PostId == id && o.Active == true).FirstOrDefault();

            var lstCateID = db.Web_PostCategory.Where(o => o.PostID == id).Select(o => o.CateID).ToList();
            int cateID = lstCateID.FirstOrDefault();
            model.LstRelate = db.store_GetPostByCateID(cateID).Take(3).Where(o => o.PostId != id).ToList();
            //model.LstCate = db.Web_Category.Where(o => lstCateID.Contains(o.CategoryId)).ToList();
            model.Author = db.CMS_Users.Where(o => o.UserID == model.Post.PostAuthor.Value).FirstOrDefault();
            model.LstCate = db.Web_Category.Where(o => o.Active == true).ToList();
            //model.Previous = db.Web_Post.Where(o => o.Active == true && o.PostId < id).OrderByDescending(o => o.PostId).FirstOrDefault();
            //model.Next = db.Web_Post.Where(o => o.Active == true && o.PostId > id).FirstOrDefault();
            //model.LstTags = db.store_GetTags().ToList();
            return View(model);
        }

        //public ActionResult Page(string seourl)
        //{
        //    DetailPostModel model = new DetailPostModel();
        //    string url = "trang/" + seourl;
        //    Web_Post post = new Web_Post();
        //    post = db.Web_Post.Where(o => o.PostType == "page" && o.URLFriendly == url && o.Active == true).FirstOrDefault();
        //    if (post == null) post = new Web_Post();
        //    model.Post = post;
        //    return View(model);
        //}

        public ActionResult ListPost(string seourl)
        {
            ListPostModel model = new ListPostModel();
            Web_Category cate = db.Web_Category.Where(o => o.CateURL == seourl && o.Active == true).FirstOrDefault();
            if (cate != null)
            {
                List<int> lstID = db.Web_PostCategory.Where(o => o.CateID == cate.CategoryId).Select(o => o.PostID).ToList();
                model.LstPost = db.Web_Post.Where(o => lstID.Contains(o.PostId)).OrderByDescending(o => o.PostId).ToList();
                model.Cate = cate;
            }
            else
            {
                model.LstPost = db.Web_Post.Where(o => o.Active == true && o.PostType == "post").OrderByDescending(o => o.PostId).Take(30).ToList();
                model.Cate = new Web_Category();
                model.Cate.CategoryName = "PC-SHOP";
            }
            //foreach (var item in model.LstPost)
            //{
            //    var temp = db.store_GetCateNameByPostID(item.PostId).FirstOrDefault();
            //    item.PostSeoKeyword = temp.CategoryName;
            //    item.PostSeoDesc = temp.CateURL;
            //}
            //model.LstCate = db.Web_Category.Where(o => o.ParentId == 0 && o.Active == true).ToList();
            return View(model);
        }

        //public ActionResult ListPortfolio(string seourl)
        //{
        //    ListPortfolioModel model = new ListPortfolioModel();
        //    //Web_CategoryPortfolio cate = db.Web_CategoryPortfolio.Where(o => o.CateURL == seourl && o.Active == true).FirstOrDefault();
        //    //if (cate != null)
        //    //{
        //    //    //List<int> lstID = db.Web_PortfolioCategory.Where(o => o.CateID == cate.CategoryId).Select(o => o.PortfolioID).ToList();
        //    //    model.LstPortfolio = db.store_GetPortfolioByCateID(cate.CategoryId).ToList();
        //    //    model.Cate = cate;
        //    //}
        //    //else
        //    //{
        //    //    model.LstPortfolio = db.store_GetPortfolioByCateID(1).ToList();
        //    //    model.LstPortfolio.AddRange(db.store_GetPortfolioByCateID(2).ToList());
        //    //    model.Cate = new Web_CategoryPortfolio();
        //    //}
        //    model.LstPortfolio = db.store_GetPortfolioByCateID(1).ToList();
        //    model.LstPortfolio.AddRange(db.store_GetPortfolioByCateID(2).ToList());
        //    model.LstPortfolio = model.LstPortfolio.OrderByDescending(o => o.PortfolioId).ToList();
        //    model.Cate = new Web_CategoryPortfolio();
        //    return View(model);
        //}

        public ActionResult RouterProduct(int id, string seourl)
        {
            DetailProductModel models = new DetailProductModel();
            CMS_Product pro = new CMS_Product();
            try
            {
                pro = db.CMS_Product.Where(o => o.ProductID == id && o.Active == true).FirstOrDefault();
                string cateRelate = pro.CateID.Split(',')[1];
                models.RelateProduct = db.store_GetProductByCateID(cateRelate).Take(5).ToList();
                models.Product = pro;
                
                //models.LstAttr = db.usp_GetAttrByProductID(pro.ProductID).ToList();
                //models.LstTopSale = db.store_GetProductTopSaleHomePage().ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return View(models);
        }

        public ActionResult RouterListProduct(string seourl)
        {
            ListProductsModel model = new ListProductsModel();
            int id = 0;
            model.Cate = db.CMS_CateProduct.Where(o => o.CateURL == seourl).FirstOrDefault();
            if (model.Cate == null)
            {
                model.LstProducts = db.store_GetProductByCateID("0").ToList();
                model.Cate = new CMS_CateProduct();
                model.Cate.Name = "Tất cả sản phẩm";
            }
            else
            {
                id = model.Cate.CategoyID;
                model.LstProducts = db.store_GetProductByCateID(id.ToString()).ToList();
            }


            return View(model);
        }

    }
}