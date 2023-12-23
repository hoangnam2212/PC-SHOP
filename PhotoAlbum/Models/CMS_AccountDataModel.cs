using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSWebsite.Models
{
    public class CMS_AccountDataModel
    {
    }
    public class ProfileDataModel
    {
        public CMS_Users User { get; set; }
        public List<CMS_Log> Logs { get; set; }
    }

    public class UsersDataModel
    {
        public List<UserViewModel> LstUser { get; set; }
        public List<CMS_Role> LstRole { get; set; }
    }

    public class UserViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string BirthdayStr { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLogin { get; set; }
        public bool Banned { get; set; }
        public bool Active { get; set; }
        public string ImgFace { get; set; }
        public string Status { get; set; }
        public string RoleName { get; set; }
        public int RoleID { get; set; }
    }

    public class MenuInitRoleModel
    {
        public List<CMS_Menu> LstParent { get; set; }
        public List<CMS_Menu> LstChild { get; set; }
        public List<CMS_MenuRole> LstMenuRole { get; set; }
        public int RoleIDEdit { get; set; }
    }

}