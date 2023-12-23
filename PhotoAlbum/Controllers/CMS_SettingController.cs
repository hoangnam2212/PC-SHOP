using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CMSWebsite.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMSWebsite.Controllers
{
    public class CMS_SettingController : CMS_BaseController
    {
        PhotoAlbumEntities db = new PhotoAlbumEntities();

        #region [Cài đặt chung]

        // GET: CMS_Setting
        public ActionResult General()
        {
            InitView();

            Web_Config config = new Web_Config();

            config = dbCMS.Web_Config.FirstOrDefault();
            if(config == null)
            {
                config = new Web_Config();
                config.Address = "";
                config.CodeAnalytics = "";
                config.CompanyName = "";
                config.Email = "";
                config.Facebook = "";
                config.GooglePlus = "";
                config.Hotline = "";
                config.Youtube = "";
                dbCMS.Web_Config.Add(config);
                dbCMS.SaveChanges();
            }
            
            return View(config);
        }
        
        // Cập nhật thông tin config web
        public ActionResult UpdateConfigGeneral(FormCollection frm)
        {
            InitView();
            try
            {
                Web_Config config = new Web_Config();
                config = dbCMS.Web_Config.FirstOrDefault();
                config.Address = frm["address"].ToString();
                config.CompanyName = frm["companyName"].ToString();
                config.Email = frm["email"].ToString();
                config.Facebook = frm["facebook"].ToString();
                config.GooglePlus = frm["googleplus"].ToString();
                config.Hotline = frm["hotline"].ToString();
                config.Youtube = frm["youtube"].ToString();
                dbCMS.SaveChanges();
                WriteLogToDB(this.UserID, this.Fullname, "Cập nhật thông tin cơ bản website", "Cập nhật thông tin cơ bản website", DateTime.Now);
                return Content("OK");
            }
            catch (Exception)
            {
                return Content("Error!");
            }
        }

        // Cập nhật thông tin nâng cao web
        public ActionResult UpdateConfigAdvance(string analytics)
        {
            InitView();
            try
            {   
                Web_Config config = new Web_Config();
                config = dbCMS.Web_Config.FirstOrDefault();
                config.CodeAnalytics = analytics;
                                                        
                dbCMS.SaveChanges();
                WriteLogToDB(this.UserID, this.Fullname, "Cập nhật thông tin nâng cao website", "Cập nhật thông tin nâng cao website", DateTime.Now);
                return Content("OK");
            }
            catch (Exception)
            {
                return Content("Error!");
            }
        }

        #endregion

        #region [Cài đặt Slider]

        public ActionResult Slider()
        {
            InitView();
            List<Web_Banner> banner = new List<Web_Banner>();
            banner = dbCMS.Web_Banner.OrderBy(o => o.Position.Value).ToList();
            return View(banner);
        }

        // Thêm Slider
        public ActionResult AddSliderHTML()
        {
            int pos = dbCMS.Web_Banner.Count();
            Web_Banner banner = new Web_Banner();
            banner.Position = pos + 1;
            dbCMS.Web_Banner.Add(banner);
            dbCMS.SaveChanges();
            return PartialView("_SliderBox",banner);
        }

        // Xóa Slider 
        public void DeleteSlider(int bID)
        {
            Web_Banner banner = new Web_Banner();
            banner = dbCMS.Web_Banner.Where(o => o.BannerID == bID).FirstOrDefault();
            if(banner != null)
            {
                dbCMS.Web_Banner.Remove(banner);
                dbCMS.SaveChanges();
            }
            List<Web_Banner> lstBanner = new List<Web_Banner>();
            lstBanner = dbCMS.Web_Banner.OrderBy(o => o.Position).ToList();
            for (int i = 0; i < lstBanner.Count; i++)
            {
                lstBanner[i].Position = i + 1;
            }
            dbCMS.SaveChanges();
        }
        
        // Sắp xếp Slider
        public void SortSlider(string order)
        {
            try
            {
                string[] slider = order.Split(',');
                int bID = 0;
                Web_Banner banner;
                for (int i = 0; i < slider.Length - 1; i++)
                {
                    bID = Convert.ToInt32(slider[i]);
                    banner = dbCMS.Web_Banner.Where(o => o.BannerID == bID).FirstOrDefault();
                    banner.Position = i+1;
                   

                }
                dbCMS.SaveChanges();
            }
            catch (Exception)
            {
                
            }
        }

        public void AddSliderImg(string currentImg ,int bID)
        {
            Web_Banner banner = new Web_Banner();
            banner = dbCMS.Web_Banner.Where(o => o.BannerID == bID).FirstOrDefault();
            banner.Images = currentImg;
            banner.Link = currentImg;
            dbCMS.SaveChanges();
        }

        // Cập nhật thông tin tiêu đề slider
        public void UpdateTitleSlider(string title,int bID)
        {
            Web_Banner banner = new Web_Banner();
            banner = dbCMS.Web_Banner.Where(o => o.BannerID == bID).FirstOrDefault();
            banner.Title = title;
            dbCMS.SaveChanges();
        }

        #endregion

        #region [Cài đặt MENU Web site]

        public ActionResult MenuManage()
        {
            InitView();

            MenuWebDataModel model = new MenuWebDataModel();
            model.ListMenuType = db.Web_MenuType.ToList();
            model.ListMenuParent = db.Web_Menu.Where(o => o.MenuTypeId == 1 && o.ParentId == 0).OrderBy(o => o.MenuOrder).ToList();
            model.ListMenuChild = db.Web_Menu.Where(o => o.MenuTypeId == 1 && o.ParentId != 0).OrderBy(o => o.MenuOrder).ToList();

            return View(model);
        }

       
         public ActionResult AddMenuHTML(int menuType)
        {
            int pos = db.Web_Banner.Count();
            Web_Menu menu = new Web_Menu();
            menu.Icon = "";
            menu.Active = true;
            menu.MenuName = "";
            menu.MenuOrder = 1;
            menu.MenuTarget = "_self";
            menu.MenuTypeId = menuType;
            menu.ParentId = 0;
            menu.URL = "";
            db.Web_Menu.Add(menu);
            db.SaveChanges();

            return PartialView("_MenuBox", menu);
        }

        // Xóa Menu 
        public void DeleteMenu(int mID)
        {
            Web_Menu menu = new Web_Menu();
            menu = db.Web_Menu.Where(o => o.MenuId == mID).FirstOrDefault();
            if (menu != null)
            {
                db.Web_Menu.Remove(menu);
                db.SaveChanges();
            }
            
        }

        // Cập nhật menu
        public void SaveMenu(FormCollection frm)
        {
            try
            {
                string lstID = frm["lstMenuID"].ToString();
                object jValue = JsonConvert.DeserializeObject(lstID);

                IEnumerable<object> en = (IEnumerable<object>)jValue;
                Web_Menu menu = new Web_Menu();
                Web_Menu child = new Web_Menu();
                string jsonValue = "";
                int loop = en.Count();
                for (int i = 0; i < loop; i++)
                {
                    jsonValue = en.ElementAt(i).ToString();
                    ListMenuChild s = JsonConvert.DeserializeObject<ListMenuChild>(jsonValue);
                    menu = db.Web_Menu.Where(o => o.MenuId == s.id).FirstOrDefault();
                    menu.MenuName = frm["title-"+s.id].ToString();
                    menu.MenuOrder = i + 1;
                    menu.ParentId = 0;
                    menu.URL = frm["link-" + s.id].ToString();
                    if(s.children != null)
                    {
                        foreach (var item in s.children)
                        {
                            child = db.Web_Menu.Where(o => o.MenuId == item.id).FirstOrDefault();
                            child.ParentId = menu.MenuId;
                            child.MenuName = frm["title-" + item.id].ToString();
                            child.MenuOrder = i+ child.MenuId;
                            child.URL = frm["link-" + item.id].ToString();
                        }
                    }
                }
                db.SaveChanges();

                //List<Root2> r = JsonConvert.DeserializeObject<List<Root2>>(lstID);
                
               

                //for (int i = 0; i < lstID.Length - 1; i++)
                //{
                //    id = Convert.ToInt32(lstID[i]);
                //    menu = db.Web_Menu.Where(o => o.MenuId == id).FirstOrDefault();
                    
                //    string a = "title-" + id;
                //    a = frm[a].ToString();
                //}
            }
            catch (Exception e)
            {
                string ss = e.ToString();
            }
            
        }

        // Thay đổi vị trí menu
        public ActionResult ChangeMenu(int mID)
        {
            MenuWebDataModel model = new MenuWebDataModel();
            model.ListMenuParent = db.Web_Menu.Where(o => o.MenuTypeId == mID && o.ParentId == 0).OrderBy(o => o.MenuOrder).ToList();
            model.ListMenuChild = db.Web_Menu.Where(o => o.MenuTypeId == mID && o.ParentId != 0).OrderBy(o => o.MenuOrder).ToList();


            return PartialView("_ChangeMenu",model);
        }

        #endregion


        #region [Lịch sử thao tác]

        public ActionResult LogManage()
        {
            InitView();
            // model = db.CMS_Log.OrderByDescending(o => o.Date).Take(50).ToList();
            LogManageDataModel model = new LogManageDataModel();
            model.LstLog = db.CMS_Log.OrderByDescending(o => o.Date).Take(50).ToList();
            model.LstUser = db.CMS_Users.Where(o => o.Banned == false).ToList();
            return View(model);
        }

        // Lọc theo người dùng
        public ActionResult FilterLogByAcc(int search)
        {
            List<CMS_Log> model = new List<CMS_Log>();
            model = db.CMS_Log.Where(o => o.UserID == search).OrderByDescending(o => o.Date).ToList();
            return PartialView("_ListLog", model);
        }

        // Lọc theo ngày
        public ActionResult FilterLogByDate(string date)
        {
            string[] dateSplit = date.Split('-');
            DateTime dateFromLookup = ConvertDateTime(dateSplit[0], 0);
            DateTime dateToLookup = ConvertDateTime(dateSplit[1], 1);
            List<CMS_Log> model = new List<CMS_Log>();
            model = db.CMS_Log.Where(o => o.Date >= dateFromLookup && o.Date <= dateToLookup).OrderByDescending(o => o.Date).ToList();
            return PartialView("_ListLog", model);

        }

        #endregion 
    }
}