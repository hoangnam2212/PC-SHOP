using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSWebsite.Controllers
{
    public class CMS_HomeController : CMS_BaseController
    {
        // GET: CMS_Home
        public ActionResult Index()
        {
            return View();
        }

        // Trang tổng quan
        public ActionResult Dashboard()
        {
            InitView();


            return View();
        }
    }
}