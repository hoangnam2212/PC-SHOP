using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;
using System.IO;

namespace CMSWebsite.Controllers
{
    public class CMS_MediaController : CMS_BaseController
    {
        public const string PathPhoto = "/Images/photo/Articleimages";

        // GET: CMS_Media
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MediaManage()
        {
            InitView();
            string directoryPath = Server.MapPath("~" + PathPhoto);
            List<string> filesName = Directory.GetFiles(directoryPath).Select(Path.GetFileName).ToList();
            List<CMS_MediaDataModel> model = new List<CMS_MediaDataModel>();
            CMS_MediaDataModel temp;
            for (int i = 0; i < filesName.Count; i++)
            {
                temp = new CMS_MediaDataModel();
                temp.FileName = PathPhoto + "/" + filesName[i];
                temp.Date = System.IO.File.GetCreationTime(directoryPath+"/" + filesName[i]);
                temp.FileSize = ConvertFileSize(new System.IO.FileInfo(directoryPath + "/" + filesName[i]).Length);
                temp.ID = i.ToString();
                model.Add(temp);
            }
            return View(model.OrderByDescending(o=>o.Date).ToList());
        }

        public void DelImage(string path)
        {
            try
            {
                string directoryPath = Server.MapPath("~" + path);
                if (System.IO.File.Exists(directoryPath))
                {
                    System.IO.File.Delete(directoryPath);
                }
            }
            catch (Exception)
            {
            }
        }

        public ActionResult GetAllImage() {
            string directoryPath = Server.MapPath("~" + PathPhoto);
            List<string> filesName = Directory.GetFiles(directoryPath).Select(Path.GetFileName).ToList();
            List<CMS_MediaDataModel> model = new List<CMS_MediaDataModel>();
            CMS_MediaDataModel temp;
            for (int i = 0; i < filesName.Count; i++)
            {
                temp = new CMS_MediaDataModel();
                temp.FileName = PathPhoto + "/" + filesName[i];
                temp.Date = System.IO.File.GetCreationTime(directoryPath + "/" + filesName[i]);
                temp.FileSize = ConvertFileSize(new System.IO.FileInfo(directoryPath + "/" + filesName[i]).Length);
                temp.ID = i.ToString();
                model.Add(temp);
            }
            return PartialView("_LstGallery", model.OrderByDescending(o => o.Date).ToList());
        }
    }
}