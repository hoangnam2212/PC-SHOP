using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;

namespace CMSWebsite.Controllers
{
    public class CMS_PageController : CMS_BaseController
    {
        // GET: CMS_Page
        public ActionResult Index()
        {
            return View();
        }

        #region [Tất cả trang tĩnh]

        public ActionResult AllPage()
        {
            InitView();
            List<PostView> model = new List<PostView>();
            try
            {
                List<Web_Post> lstPost = new List<Web_Post>();
                lstPost = dbCMS.Web_Post.Where(o => o.Active == true && o.PostType == "page").OrderByDescending(o => o.PostCreateDate).ToList();
                List<Web_PostCategory> arr = new List<Web_PostCategory>();
                PostView temp;
                int authorID = 0;
                for (int i = 0; i < lstPost.Count; i++)
                {
                    temp = new PostView();
                    authorID = lstPost[i].PostAuthor.Value;
                    temp.PostAuthor = dbCMS.CMS_Users.Where(o => o.UserID == authorID).Select(o => o.FullName).FirstOrDefault();

                    temp.PostDate = Convert.ToDateTime(lstPost[i].PostCreateDate).ToString("dd/MM/yyyy lúc HH:mm");
                    
                    temp.UrlFriendly = lstPost[i].URLFriendly;
                    temp.PostId = lstPost[i].PostId;
                    temp.PostTitle = lstPost[i].PostTitle;
                    temp.PostCate = "";
                    model.Add(temp);
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("AllPage", model);
        }

        #endregion

        #region [Thêm mới trang tĩnh]

        public ActionResult NewPage()
        {
            InitView();

            return View();
        }

        // Thêm mới bài viết 
        [HttpPost]
        public ActionResult AddPage(PostModel models)
        {
            InitView();
            try
            {
                Web_Post newPage = new Web_Post();
                // int id = Convert.ToInt32(Session["TMDTUserID"]);
                newPage.PostAuthor = this.UserID;
                newPage.PostContent = models.contentPost;
                newPage.PostCreateDate = DateTime.Now;
                newPage.PostFaceImage = "";
                newPage.PostSeoDesc = models.seoDescPost;
                newPage.PostType = "page";
                newPage.PostSeoKeyword = models.seoKeywordPost;
                newPage.PostTitle = models.titlePost;
                newPage.Active = true;
                newPage.URLFriendly = "";
                newPage.PostDescription = "";
                newPage.URLFriendly = "trang/" + ToFriendlyUrl(models.titlePost);
                dbCMS.Web_Post.Add(newPage);
                dbCMS.SaveChanges();
               
                dbCMS.SaveChanges();
                WriteLogToDB(this.UserID, this.Fullname, "Thêm mới trang", this.Fullname + " tạo trang : " + newPage.PostTitle, DateTime.Now);
                return Content("OK");
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }

        // Xóa trang
        public ActionResult DeletePage(int pageID)
        {
            InitView();
            try
            {
                Web_Post delete = dbCMS.Web_Post.Where(o => o.PostId == pageID).FirstOrDefault();
                delete.Active = false;
                dbCMS.SaveChanges();
                WriteLogToDB(this.UserID, this.Fullname, "Xóa trang ", this.Fullname + " xóa trang : " + delete.PostTitle, DateTime.Now);
                return Content("Success");
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }

        // Sửa trang
        public ActionResult ViewEdit(int id)
        {
            InitView();
            EditPostModel model = new EditPostModel();
            model.PostEdit = dbCMS.Web_Post.Where(o => o.PostId == id && o.Active == true).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPage(PostModel models)
        {
            InitView();
            try
            {
                int pid = Convert.ToInt32(models.postId);
                Web_Post newPost = dbCMS.Web_Post.Where(o => o.PostId == pid).FirstOrDefault();
                newPost.PostContent = models.contentPost;
                newPost.PostUpdateDate = DateTime.Now;
                newPost.PostSeoDesc = models.seoDescPost;
                newPost.PostSeoKeyword = models.seoKeywordPost;
                newPost.PostTitle = models.titlePost;
                newPost.URLFriendly = "trang/" + ToFriendlyUrl(models.titlePost);
                dbCMS.SaveChanges();
               
                WriteLogToDB(this.UserID, this.Fullname, "Sửa trang", this.Fullname + " sửa nội dung trang : " + newPost.PostTitle, DateTime.Now);
                return Content("OK");
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }


        #endregion

    }
}