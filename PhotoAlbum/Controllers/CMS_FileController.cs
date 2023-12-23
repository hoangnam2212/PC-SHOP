using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;
using System.IO;
using System.Configuration;

namespace CMSWebsite.Controllers
{
    public class CMS_FileController : Controller
    {
        public const string PathPhoto = "/Images/photo/Articleimages";
        //
        // GET: /File/
        PhotoAlbumEntities db = new PhotoAlbumEntities();

        [HttpPost]
        public JsonResult UploadImgPost(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                //string p = DateTime.Now.ToString("MM-yyyy");
                string directoryPath = Server.MapPath("~"+ PathPhoto);
                if (!System.IO.Directory.Exists(directoryPath))
                    System.IO.Directory.CreateDirectory(directoryPath);
                string[] fileName = file.FileName.Split('.');
                string f = fileName[0] + "-" + DateTime.Now.ToString("HH-mm-dd-MM") + "." + fileName[1];

                var path = Path.Combine(directoryPath, f);
                file.SaveAs(path);
                return Json(new
                {
                    statusCode = 200,
                    status = "OK",
                    nameFile = PathPhoto + "/"+ f,
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                statusCode = 400,
                status = "Bad Request! Upload Failed",
                nameFile = "",
            }, JsonRequestBehavior.AllowGet);
        }

        // Upload ảnh website
        [HttpPost]
        public JsonResult UploadImgWeb(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                //string p = DateTime.Now.ToString("MM-yyyy");
                string directoryPath = Server.MapPath("~"+ PathPhoto);
                if (!System.IO.Directory.Exists(directoryPath))
                    System.IO.Directory.CreateDirectory(directoryPath);
                string[] fileName = file.FileName.Split('.');
                string f = fileName[0] + "-" + DateTime.Now.ToString("HH-mm-dd-MM") + "." + fileName[1];

                var path = Path.Combine(directoryPath, f);
                file.SaveAs(path);
                return Json(new
                {
                    statusCode = 200,
                    status = "OK",
                    nameFile = PathPhoto + "/" + f,
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                statusCode = 400,
                status = "Bad Request! Upload Failed",
                nameFile = "",
            }, JsonRequestBehavior.AllowGet);
        }

        // Upload ảnh profile
        [HttpPost]
        public JsonResult UploadImgFace(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                //string p = DateTime.Now.ToString("MM-yyyy");
                string directoryPath = Server.MapPath("~"+ PathPhoto);
                if (!System.IO.Directory.Exists(directoryPath))
                    System.IO.Directory.CreateDirectory(directoryPath);
                string[] fileName = file.FileName.Split('.');
                string f = fileName[0] + "-" + DateTime.Now.ToString("HH-mm-dd-MM") + "." + fileName[1];
                var path = Path.Combine(directoryPath, f);
                file.SaveAs(path);

                int UserId = Convert.ToInt32(Session["CMS_UserID"]);
                CMS_Users user = db.CMS_Users.Where(o => o.UserID == UserId).FirstOrDefault();
                user.ImgFace = PathPhoto+"/" + f;
                db.SaveChanges();
                Session["CMS_ImgFace"] = PathPhoto+ "/" + f;
                HttpCookie cms = Request.Cookies.Get("CMS_Cookie");
                if (cms != null)
                {
                    cms["CMS_ImgFace"] = PathPhoto+ "/" + f;
                    Response.Cookies.Set(cms);
                }
                return Json(new
                {
                    statusCode = 200,
                    status = f,
                    nameFile = f,
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                statusCode = 400,
                status = "Bad Request! Upload Failed",
                nameFile = "",
            }, JsonRequestBehavior.AllowGet);
        }

        // Upload anh san pham
        [HttpPost]
        public JsonResult UploadProducts(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string p = DateTime.Now.ToString("MM-yyyy");
                string directoryPath = Server.MapPath("~/Images/products/") + p;
                if (!System.IO.Directory.Exists(directoryPath))
                    System.IO.Directory.CreateDirectory(directoryPath);
                string[] fileName = file.FileName.Split('.');
                string f = fileName[0] + "-" + DateTime.Now.ToString("HH-mm-dd-MM") + "." + fileName[1];
                var path = Path.Combine(directoryPath, f);
                file.SaveAs(path);
                return Json(new
                {
                    statusCode = 200,
                    status = f,
                    nameFile = "/Images/products/" + p + "/" + f,
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                statusCode = 400,
                status = "Bad Request! Upload Failed",
                nameFile = "",
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
