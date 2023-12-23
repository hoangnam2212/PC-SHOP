using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSWebsite.Models
{
    public class CMS_SettingDataModel
    {

    }

    public class MenuWebDataModel
    {
        public List<Web_Menu> ListMenuParent { get; set; }
        public List<Web_Menu> ListMenuChild { get; set; }
        public List<Web_MenuType> ListMenuType { get; set; }
    }
    public class ListMenuChild
    {
        public int id { get; set; }
        public List<MenuChild> children { get; set; }
    }

    public class MenuChild
    {
        public int id { get; set; }
    }

    public class LogManageDataModel
    {
        public List<CMS_Users> LstUser { get; set; }
        public List<CMS_Log> LstLog { get; set; }
    }
    
}