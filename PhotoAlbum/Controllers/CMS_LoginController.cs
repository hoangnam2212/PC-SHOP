using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;

namespace CMSWebsite.Controllers
{
    public class CMS_LoginController : Controller
    {
        PhotoAlbumEntities db = new PhotoAlbumEntities();

        // GET: CMS_Login
        public ActionResult Index()
        {
            return View();
        }

        // Xử lý login
        public ActionResult LoginProcess(string user, string pass)
        {
            user = user.Trim();
            pass = pass.Trim();
            CMS_Users users = db.CMS_Users.Where(o => o.UserName == user && o.Pass == pass).FirstOrDefault();
            if (users != null)
            {
                if (!users.Banned.Value)  // Check Active
                {
                    users.LastLogin = DateTime.Now;
                    db.SaveChanges();

                    string roleID = db.CMS_UserRole.Where(o => o.UserID == users.UserID).Select(o => o.RoleID).FirstOrDefault().ToString();
                    // Write Session
                    Session["CMS_UserID"] = users.UserID;
                    Session["CMS_UserName"] = users.UserName;
                    Session["CMS_FullName"] = HttpUtility.UrlEncode(users.FullName);
                    Session["CMS_ImgFace"] = users.ImgFace;
                    Session["CMS_RoleID"] = roleID;

                    // Write Cookie
                    HttpCookie cms = new HttpCookie("CMS_Cookie");
                    cms["CMS_UserID"] = users.UserID.ToString();
                    cms["CMS_UserName"] = users.UserName;
                    cms["CMS_FullName"] = HttpUtility.UrlEncode(users.FullName);
                    cms["CMS_ImgFace"] = users.ImgFace;
                    cms["CMS_RoleID"] = roleID;
                    cms.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(cms);

                    return Content("OK");
                }
                return Content("Tài khoản bạn đã ngừng hoạt động, liên hệ quản trị viên !");
            }
            return Content("Sai tên đăng nhập hoặc mật khẩu !");
        }

        // Đăng xuất
        public ActionResult Logout()
        {
            ClearCookies();
            Session["QLBH_UserID"] = null;
            Session["QLBH_UserName"] = null;
            Session["QLBH_FullName"] = null;
            Session["QLBH_ImgFace"] = null;

            return Redirect("/CMS_Login/Index");
        }

        // Xóa cookie
        public void ClearCookies()
        {
            if (Request.Cookies["CMS_Cookie"] != null)
            {
                HttpCookie myCookie = new HttpCookie("CMS_Cookie");
                myCookie.Value = null;
                myCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(myCookie);
            }
            Session["CMS_UserID"] = null;
        }
    }
}