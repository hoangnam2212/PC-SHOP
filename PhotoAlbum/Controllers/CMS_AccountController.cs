using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;

namespace CMSWebsite.Controllers
{
    public class CMS_AccountController : CMS_BaseController
    {
        PhotoAlbumEntities db = new PhotoAlbumEntities();

        #region [Profile]

        // GET: CMS_Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProfileManager()
        {
            InitView();
            ProfileDataModel model = new ProfileDataModel();
            model.Logs = db.CMS_Log.Where(o => o.UserID == this.UserID).OrderByDescending(o => o.Date).ToList();
            model.User = db.CMS_Users.Where(o => o.UserID == this.UserID).FirstOrDefault();
            return View(model);
        }

        // Cập nhật thông tin tài khoản
        public ActionResult UpdateProfile(FormCollection frm)
        {
            InitView();
            try
            {
                /******Contract****/
                string fullname = frm["txtFullName"];
                string birdday = frm["txtBirdDay"];
                string email = frm["txtEmail"];
                string phone = frm["txtPhone"];
                string address = frm["txtAddress"];

                CMS_Users user = db.CMS_Users.Where(o => o.UserID == this.UserID).FirstOrDefault();
                // user.ImgFace = imgface;
                user.FullName = fullname;
                user.Email = email;
                user.Phone = phone;
                user.Address = address;
                user.Birthday = ConvertDateTime(birdday);
                db.SaveChanges();
                Session["CMS_FullName"] = HttpUtility.UrlEncode(fullname);
                HttpCookie cms = Request.Cookies.Get("CMS_Cookie");
                if (cms != null)
                {
                    cms["CMS_FullName"] = HttpUtility.UrlEncode(fullname);
                    Response.Cookies.Set(cms);
                }
                WriteLogToDB(this.UserID, this.Fullname, "Sửa thông tin cá nhân", "Cập nhật Profile ", DateTime.Now);
            }
            catch (Exception)
            {
                return Content("Lỗi ! Nhập lại đúng định dạng");
            }
            return Content("OK");
        }

        #endregion

        #region [Quản lý người dùng]


        public ActionResult AccountManager()
        {
            InitView();
            UsersDataModel model = new UsersDataModel();
            List<UserViewModel> listUser = new List<UserViewModel>();

            // Danh sách người dùng
            listUser = GetListUser(db.CMS_Users.Where(o => o.Active == true).OrderByDescending(o => o.CreateDate).ToList());

            model.LstUser = listUser;
            model.LstRole = db.CMS_Role.ToList();
            return View(model);
        }

        // Lấy danh sách người dùng
        public List<UserViewModel> GetListUser(List<CMS_Users> lstU)
        {
            UserViewModel uvm = new UserViewModel();
            List<UserViewModel> listUser = new List<UserViewModel>();
            var lstRole = db.CMS_Role.ToList();
            foreach (var item in lstU)
            {
                uvm = new UserViewModel();
                int roleID = db.CMS_UserRole.Where(o => o.UserID == item.UserID).Select(o => o.RoleID).FirstOrDefault();
                uvm.RoleName = lstRole.Where(o => o.RoleID == roleID).Select(o => o.RoleName).FirstOrDefault();
                uvm.FullName = item.FullName;
                uvm.UserName = item.UserName;
                uvm.Phone = item.Phone;
                uvm.ImgFace = item.ImgFace;
                uvm.Address = item.Address;
                uvm.Email = item.Email;
                uvm.UserID = item.UserID;
                uvm.Gender = item.Gender.Value;
                uvm.BirthdayStr = item.Birthday != null ? item.Birthday.Value.ToString("dd/MM/yyyy") : "";
                uvm.Status = item.Banned.Value == true ? "Ngừng hoạt động" : "Đang hoạt động";
                listUser.Add(uvm);
            }
            return listUser;
        }

        // Thêm mới người dùng 
        public ActionResult AddNewUser(FormCollection frm)
        {
            InitView();
            try
            {
                CMS_Users newUser = new CMS_Users();
                string checkUser = frm["userNameNew"].ToString();
                newUser = db.CMS_Users.Where(o => o.UserName == checkUser).FirstOrDefault();
                if (newUser == null)
                {

                    newUser = new CMS_Users();
                    newUser.Active = true;
                    newUser.Address = frm["addNew"].ToString();
                    newUser.Banned = false;
                    newUser.Birthday = ConvertDateTime(frm["txtBirthdayNew"].ToString());
                    newUser.CreateBy = this.UserID;
                    newUser.CreateDate = DateTime.Now;
                    newUser.Email = frm["emailNew"].ToString();
                    newUser.FullName = frm["fullNameNew"].ToString();
                    newUser.Gender = frm["genderNew"].ToString() == "M" ? true : false;
                    newUser.ImgFace = "/Images/cms/icon/user.png";
                    newUser.Pass = frm["passNew"].ToString();
                    newUser.Phone = frm["phoneNew"].ToString();
                    newUser.Status = 1;
                    newUser.UserName = frm["userNameNew"].ToString();
                    db.CMS_Users.Add(newUser);
                    db.SaveChanges();
                    CMS_UserRole newUserRole = new CMS_UserRole();
                    newUserRole.UserID = newUser.UserID;
                    newUserRole.RoleID = Convert.ToInt32(frm["slRoleNew"].ToString());
                    db.CMS_UserRole.Add(newUserRole);
                    db.SaveChanges();
                    return PartialView("_ListUsers", GetListUser(db.CMS_Users.Where(o => o.Active == true).OrderByDescending(o => o.CreateDate).ToList()));
                }
                else
                {
                    return Content("ERROR! Tài khoản đã có trên hệ thống");
                }
            }
            catch (Exception ex)
            {

                return Content("ERROR");
            }
        }

        // Lấy thông tin người dùng
        public ActionResult GetUserByUserID(int userID)
        {
            UserViewModel uvm = new UserViewModel();
            CMS_Users user = new CMS_Users();
            user = db.CMS_Users.Where(o => o.UserID == userID).FirstOrDefault();
            int roleID = db.CMS_UserRole.Where(o => o.UserID == user.UserID).Select(o => o.RoleID).FirstOrDefault();
            uvm.RoleName = db.CMS_Role.Where(o => o.RoleID == roleID).Select(o => o.RoleName).FirstOrDefault();
            uvm.FullName = user.FullName;
            uvm.UserName = user.UserName;
            uvm.Phone = user.Phone;
            uvm.RoleID = roleID;
            uvm.Address = user.Address;
            uvm.Email = user.Email;
            uvm.UserID = user.UserID;
            uvm.Banned = user.Banned.Value;
            uvm.Gender = user.Gender.Value;
            uvm.BirthdayStr = user.Birthday.Value.ToString("dd/MM/yyyy");
            return Json(uvm);
        }


        // Lưu cập nhật người dùng
        public ActionResult SaveUserInfo(FormCollection frm)
        {
            InitView();
            try
            {

                string fullName = frm["fullNameEdit"];
                string userName = frm["userNameEdit"];
                string pass = frm["passEdit"];
                string phone = frm["phoneEdit"];
                string email = frm["emailEdit"];
                string address = frm["addEdit"];
                string birthday = frm["txtBirthdayEdit"];
                string gender = frm["genderEdit"];
                int userID = Convert.ToInt32(frm["UserIDEdit"]);
                int roleID = Convert.ToInt32(frm["slRoleEdit"]);
                int ban = Convert.ToInt32(frm["slBannedEdit"]);
                CMS_Users userEdit = db.CMS_Users.Where(o => o.UserName == userName).FirstOrDefault();
                if(pass.Trim() != "")
                {
                    userEdit.Pass = pass;
                }
                userEdit.FullName = fullName;
                userEdit.UserName = userName;
                userEdit.Banned = ban == 0 ? false : true;
                userEdit.Gender = gender == "M" ? true : false;
                userEdit.Phone = phone;
                userEdit.Address = address;
                userEdit.Email = email;
                userEdit.Birthday = ConvertDateTime(birthday);
                CMS_UserRole ur = db.CMS_UserRole.Where(o => o.UserID == userEdit.UserID).FirstOrDefault();
                ur.RoleID = roleID;
                db.SaveChanges();
                WriteLogToDB(this.UserID, this.Fullname, "Sửa thông tin tài khoản", "Cập nhật thông tin tài khoản : " + userName, DateTime.Now);
                List<UserViewModel> lstUser = GetListUser(db.CMS_Users.Where(o => o.Active == true).OrderByDescending(o => o.CreateDate).ToList());
                return PartialView("_ListUsers", lstUser);
            }
            catch (Exception ex)
            {
                //WriteFileLog.WriteLog(String.Empty, ex);
            }
            return Content("");
        }

        // Tìm kiếm người dùng
        public ActionResult SearchUserName(string character)
        {
            string[] temp;
            var lstUser = db.CMS_Users.ToList();
            List<CMS_Users> lstUserSearch = new List<CMS_Users>();
            if (character == "0")
            {
                lstUserSearch = db.CMS_Users.Where(o => o.Active == true).OrderByDescending(o => o.CreateDate).ToList();
            }
            else
            {
                foreach (var item in lstUser)
                {
                    temp = item.FullName.Split(' ');
                    foreach (var item2 in temp)
                    {
                        if (item2.ToLower().ElementAt(0) == Convert.ToChar(character))
                        {
                            lstUserSearch.Add(item);
                            break;
                        }
                    }
                }
            }

            return PartialView("_ListUsers", GetListUser(lstUserSearch));
        }

        // Khởi tạo phân quyền
        public ActionResult GetListInitRole()
        {
            MenuInitRoleModel model = new MenuInitRoleModel();
            model.LstParent = db.CMS_Menu.Where(o => o.ParentID == 0 && o.Active == true && o.IsShow == true).OrderBy(o => o.OrderMenu).ToList();
            model.LstChild = db.CMS_Menu.Where(o => o.ParentID != 0 && o.Active == true && o.IsShow == true).OrderBy(o => o.OrderMenu).ToList();
            model.LstMenuRole = db.CMS_MenuRole.ToList();
            return PartialView("_InitRole", model);
        }

        // Tạo quyền mới
        // 1 - Xem , 2 Them , 3 - Sửa , 4 - Xóa
        public ActionResult AddNewRole(string lstRole, string titleRole)
        {
            InitView();
            try
            {
                CMS_Role role = new CMS_Role();
                List<CMS_Role> model = new List<CMS_Role>();
                role.Active = true;
                role.CreateBy = this.UserID;
                role.CreateDate = DateTime.Now;
                role.RoleName = titleRole;
                db.CMS_Role.Add(role);
                db.SaveChanges();

                CMS_MenuRole addNew,checkRoleMenu;
                string[] arrRole = lstRole.Split(',');
                string[] arrFunc;
                int action,menuID;
                for (int i = 0; i < arrRole.Length - 1; i++)
                {
                    arrFunc = arrRole[i].Split('-');
                    menuID = Convert.ToInt32(arrFunc[0]);
                    action = Convert.ToInt32(arrFunc[1]);
                    checkRoleMenu = db.CMS_MenuRole.Where(o => o.MenuID == menuID && o.RoleID == role.RoleID).FirstOrDefault();
                    if(checkRoleMenu != null)
                    {
                        switch (action)
                        {
                            case 1:
                                checkRoleMenu.ViewData = true;
                                break;
                            case 2:
                                checkRoleMenu.AddData = true;
                                break;
                            case 3:
                                checkRoleMenu.EditData = true;
                                break;
                            case 4:
                                checkRoleMenu.DelData = true;
                                break;
                        }
                    }
                    else
                    {
                        addNew = new CMS_MenuRole();
                        addNew.RoleID = role.RoleID;
                        addNew.MenuID = menuID;
                        addNew.Import = true;
                        addNew.Export = true;
                        addNew.AddData = false;
                        addNew.DelData = false;
                        addNew.EditData = false;
                        addNew.ViewData = false;
                        switch (action)
                        {
                            case 1:
                                addNew.ViewData = true;
                                break;
                            case 2:
                                addNew.AddData = true;
                                break;
                            case 3:
                                addNew.EditData = true;
                                break;
                            case 4:
                                addNew.DelData = true;
                                break;
                        }
                        db.CMS_MenuRole.Add(addNew);
                        db.SaveChanges();
                    }
                }
                db.SaveChanges();

                model = db.CMS_Role.ToList();
                return PartialView("_ListRoles", model);
            }
            catch (Exception)
            {

            }
            return Content("ERROR");
        }

        // Khởi tạo sửa phân quyền
        public ActionResult GetEditRole(int roleID)
        {
            MenuInitRoleModel model = new MenuInitRoleModel();
            model.LstParent = db.CMS_Menu.Where(o => o.ParentID == 0 && o.Active == true && o.IsShow == true).OrderBy(o => o.OrderMenu).ToList();
            model.LstChild = db.CMS_Menu.Where(o => o.ParentID != 0 && o.Active == true && o.IsShow == true).OrderBy(o => o.OrderMenu).ToList();
            model.LstMenuRole = db.CMS_MenuRole.ToList();
            model.RoleIDEdit = roleID;
            return PartialView("_InitRoleEdit", model);
        }

        // Lưu lại sửa phân quyền 
        public ActionResult SaveEditRole(string lstRole, string titleRole, int roleID)
        {
            InitView();
            try
            {
                CMS_Role role = new CMS_Role();
                List<CMS_Role> model = new List<CMS_Role>();
                List<CMS_MenuRole> roleOld = new List<CMS_MenuRole>();
                roleOld = db.CMS_MenuRole.Where(o => o.RoleID == roleID).ToList();
                role = db.CMS_Role.Where(o => o.RoleID == roleID).FirstOrDefault();
                if (role != null)
                {
                    role.RoleName = titleRole;
                    role.UpdateBy = this.UserID;
                    role.UpdateDate = DateTime.Now;
                    db.SaveChanges();
                }
                CMS_MenuRole addNew, checkRoleMenu;
                string[] arrRole = lstRole.Split(',');
                string[] arrFunc;
                int action, menuID;

                db.CMS_MenuRole.RemoveRange(roleOld);
                db.SaveChanges();

                for (int i = 0; i < arrRole.Length - 1; i++)
                {
                    arrFunc = arrRole[i].Split('-');
                    menuID = Convert.ToInt32(arrFunc[0]);
                    action = Convert.ToInt32(arrFunc[1]);
                    checkRoleMenu = db.CMS_MenuRole.Where(o => o.MenuID == menuID && o.RoleID == role.RoleID).FirstOrDefault();
                    
                    if (checkRoleMenu != null)
                    {
                       
                        switch (action)
                        {
                            case 1:
                                checkRoleMenu.ViewData = true;
                                break;
                            case 2:
                                checkRoleMenu.AddData = true;
                                break;
                            case 3:
                                checkRoleMenu.EditData = true;
                                break;
                            case 4:
                                checkRoleMenu.DelData = true;
                                break;
                        }
                    }
                    else
                    {
                        addNew = new CMS_MenuRole();
                        addNew.RoleID = role.RoleID;
                        addNew.MenuID = menuID;
                        addNew.Import = true;
                        addNew.Export = true;
                        addNew.AddData = false;
                        addNew.DelData = false;
                        addNew.EditData = false;
                        addNew.ViewData = false;
                        switch (action)
                        {
                            case 1:
                                addNew.ViewData = true;
                                break;
                            case 2:
                                addNew.AddData = true;
                                break;
                            case 3:
                                addNew.EditData = true;
                                break;
                            case 4:
                                addNew.DelData = true;
                                break;
                        }
                        db.CMS_MenuRole.Add(addNew);
                        db.SaveChanges();
                    }
                }
                db.SaveChanges();
                
                model = db.CMS_Role.ToList();
                return PartialView("_ListRoles", model);
            }
            catch (Exception)
            {

            }
            return Content("ERROR");
        }

        #endregion
    }
}