using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSWebsite.Controllers
{
    public class Web_RegistryController : CMS_BaseController
    {
        // GET: Web_Registry
        public ActionResult SignUp()
        {
            InitView();



            return View();
        }
    }
}