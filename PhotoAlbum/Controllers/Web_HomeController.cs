using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;
namespace CMSWebsite.Controllers
{
    public class Web_HomeController : Controller
    {
        PhotoAlbumEntities db = new PhotoAlbumEntities();
        public ActionResult Index()
        {
            Web_HomeIndexDataModel model = new Web_HomeIndexDataModel();

            model.LstPost = db.store_GetPostIndex().ToList();

            // Ban phim co
            model.LstPhimCo = db.store_GetProductByCateID("2").ToList();

            // Ban lot chuot
            model.LstChuot = db.store_GetProductByCateID("3").ToList();

            // Lot chuot khuyen mai
            model.SpecialChuot = model.LstChuot.Where(o => o.IsFeature).FirstOrDefault();

            model.LstSlider = db.store_GetProductSlider().ToList();

            model.LstFeaturer = db.store_GetProductFeatureHomePage().ToList();

            // Danh muc san pham
            model.LstCate = db.CMS_CateProduct.OrderBy(o=>o.CategoyID).ToList();

            return View(model);
        }
        
        public ActionResult Portfolio()
        {
            return View();
        }

        public ActionResult GetByCateID(int id)
        {
            Web_HomeIndexDataModel model = new Web_HomeIndexDataModel();

            //model.LstProduct = db.store_GetProductByCateID(id.ToString()).ToList();

            return View("_WrapProduct",model);
        }
    }
}