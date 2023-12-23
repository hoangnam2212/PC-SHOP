using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;

namespace CMSWebsite.Controllers
{
    public class CMS_PostController : CMS_BaseController
    {
        PhotoAlbumEntities db = new PhotoAlbumEntities();
        #region [Tất cả bài viết]


        public ActionResult AllPost()
        {
            InitView();
            List<PostView> model = new List<PostView>();
            try
            {
                List<Web_Post> lstPost = new List<Web_Post>();
                lstPost = db.Web_Post.Where(o => o.Active == true).OrderByDescending(o => o.PostCreateDate).ToList();
                List<Web_PostCategory> arr = new List<Web_PostCategory>();
                PostView temp;
                int authorID = 0;
                for (int i = 0; i < lstPost.Count; i++)
                {
                    temp = new PostView();
                    authorID = lstPost[i].PostAuthor.Value;
                    temp.PostAuthor = db.CMS_Users.Where(o => o.UserID == authorID).Select(o => o.FullName).FirstOrDefault();

                    temp.PostDate = Convert.ToDateTime(lstPost[i].PostCreateDate).ToString("dd/MM/yyyy lúc HH:mm");
                    if (lstPost[i].PostDescription == null)
                    {
                        temp.PostDesc = "";
                    }
                    else
                    {
                        temp.PostDesc = lstPost[i].PostDescription.Length > 100 ? lstPost[i].PostDescription.Substring(0, 100) : lstPost[i].PostDescription;
                    }
                    temp.ImgFace = lstPost[i].PostFaceImage;
                    temp.UrlFriendly = lstPost[i].URLFriendly;
                    temp.PostId = lstPost[i].PostId;
                    temp.PostTitle = lstPost[i].PostTitle;
                    temp.PostCate = "";
                    arr = new List<Web_PostCategory>();
                    authorID = lstPost[i].PostId;
                    arr = db.Web_PostCategory.Where(o => o.PostID == authorID).ToList();
                    foreach (var item in arr)
                    {
                        temp.PostCate += db.Web_Category.Where(o => o.CategoryId == item.CateID).Select(o => o.CategoryName).FirstOrDefault() + ", ";
                    }
                    if (temp.PostCate.Length > 2)
                    {
                        temp.PostCate = temp.PostCate.Substring(0, temp.PostCate.Length - 2);
                    }

                    model.Add(temp);
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("AllPost", model.OrderByDescending(o => o.PostId).ToList());
        }

        // Xóa bài viết
        public ActionResult DeletePost(int postId)
        {
            InitView();
            try
            {
                Web_Post delete = db.Web_Post.Where(o => o.PostId == postId).FirstOrDefault();
                delete.Active = false;
                db.SaveChanges();
                WriteLogToDB(this.UserID, this.Fullname, "Xóa bài viết", this.Fullname + " xóa bài : " + delete.PostTitle, DateTime.Now);
                return Content("Success");
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }

        // Sửa bài viết
        public ActionResult ViewEdit(int id)
        {
            InitView();
            EditPostModel model = new EditPostModel();
            model.LstPostCate = db.Web_PostCategory.Where(o => o.PostID == id).ToList();
            model.LstCate = db.Web_Category.Where(o => o.CateType == 1 && o.Active == true).ToList();
            model.PostEdit = db.Web_Post.Where(o => o.PostId == id && o.Active == true).FirstOrDefault();

            model.Tags = "";
            int temp;
            try
            {
                if (model.PostEdit.Tags != "")
                {
                    string[] lstTag = model.PostEdit.Tags.Split(',');
                    for (int i = 1; i < lstTag.Length - 1; i++)
                    {
                        temp = Convert.ToInt32(lstTag[i]);
                        model.Tags += db.Web_Tags.Where(o => o.TagID == temp).Select(o => o.TagName).FirstOrDefault() + ",";
                    }
                    model.Tags = model.Tags.Substring(0, model.Tags.Length - 1);
                }
            }
            catch (Exception)
            {

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPost(PostModel models)
        {
            InitView();
            try
            {
                int pid = Convert.ToInt32(models.postId);
                Web_Post newPost = db.Web_Post.Where(o => o.PostId == pid).FirstOrDefault();
                newPost.PostContent = models.contentPost;
                newPost.PostUpdateDate = DateTime.Now;
                newPost.IsHome = true;
                newPost.PostFaceImage = models.imagePost;
                newPost.PostSeoDesc = models.seoDescPost;
                newPost.PostSeoKeyword = models.seoKeywordPost;
                newPost.PostTitle = models.titlePost;

                newPost.PostDescription = models.description;
                newPost.URLFriendly = "tin-tuc/" + ToFriendlyUrl(models.titlePost) + "-" + pid;
                db.SaveChanges();
                List<Web_PostCategory> lstPostCate = new List<Web_PostCategory>();

                lstPostCate = db.Web_PostCategory.Where(o => o.PostID == pid).ToList();
                int cateID = 0;
                Web_PostCategory check, postCate;
                string[] cateSplit = models.catePost.Split(',');
                for (int i = 0; i < cateSplit.Length - 1; i++)
                {
                    cateID = Convert.ToInt32(cateSplit[i]);
                    check = lstPostCate.Where(o => o.CateID == cateID && o.PostID == pid).FirstOrDefault();
                    if (check == null)
                    {
                        postCate = new Web_PostCategory();
                        postCate.CateID = cateID;
                        postCate.PostID = pid;
                        db.Web_PostCategory.Add(postCate);
                    }
                    else
                    {
                        lstPostCate.Remove(check);
                    }
                }

                foreach (var item in lstPostCate)
                {
                    db.Web_PostCategory.Remove(item);
                }

                db.SaveChanges();

                string tagString = ",";
                if (models.tags != null)
                {
                    string[] tags = models.tags.Split(',');
                    foreach (var item in tags)
                    {
                        var checkT = db.Web_Tags.Where(o => o.TagName.ToLower() == item.ToLower()).FirstOrDefault();
                        if (checkT == null)
                        {
                            checkT = new Web_Tags();
                            checkT.TagName = item;
                            checkT.CountTag = 0;
                            checkT.URL = "tags/" + ToFriendlyUrl(item);
                            db.Web_Tags.Add(checkT);
                        }

                        checkT.CountTag++;

                        db.SaveChanges();
                        tagString += checkT.TagID.ToString() + ",";
                    }
                }

                newPost.Tags = tagString;
                db.SaveChanges();

                WriteLogToDB(this.UserID, this.Fullname, "Sửa bài viết", this.Fullname + " sửa nội dung bài : " + newPost.PostTitle, DateTime.Now);
                return Content("OK");
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }

        #endregion

        #region [Thêm mới bài viết]

        public ActionResult NewPost()
        {
            InitView();
            List<Web_Category> lstCate = new List<Web_Category>();
            lstCate = db.Web_Category.Where(o => o.CateType == 1 && o.Active == true).ToList();
            return View(lstCate);
        }

        // Thêm mới bài viết 
        [HttpPost]
        public ActionResult AddPost(PostModel models)
        {
            InitView();
            try
            {
                Web_Post newPost = new Web_Post();
                // int id = Convert.ToInt32(Session["TMDTUserID"]);
                newPost.PostAuthor = this.UserID;
                newPost.PostContent = models.contentPost;
                newPost.PostCreateDate = DateTime.Now;
                newPost.PostFaceImage = models.imagePost;
                newPost.PostSeoDesc = models.seoDescPost;
                newPost.PostType = "post";
                newPost.PostSeoKeyword = models.seoKeywordPost;
                newPost.PostTitle = models.titlePost;
                newPost.Tags = "";
                newPost.IsHome = true;
                newPost.Active = true;
                newPost.URLFriendly = "";
                newPost.PostDescription = models.description;
                db.Web_Post.Add(newPost);
                db.SaveChanges();
                newPost.URLFriendly = "tin-tuc/" + ToFriendlyUrl(models.titlePost) + "-" + newPost.PostId;
                string[] cateSplit = models.catePost.Split(',');
                for (int i = 0; i < cateSplit.Length - 1; i++)
                {
                    Web_PostCategory postCate = new Web_PostCategory();
                    postCate.CateID = Convert.ToInt32(cateSplit[i]);
                    postCate.PostID = newPost.PostId;
                    db.Web_PostCategory.Add(postCate);

                }
                db.SaveChanges();

                string tagString = ",";
                if (models.tags != null)
                {
                    string[] tags = models.tags.Split(',');
                    foreach (var item in tags)
                    {
                        var check = db.Web_Tags.Where(o => o.TagName.ToLower() == item.ToLower()).FirstOrDefault();
                        if (check == null)
                        {
                            check = new Web_Tags();
                            check.TagName = item;
                            check.CountTag = 0;
                            check.URL = "tags/" + ToFriendlyUrl(item);
                            db.Web_Tags.Add(check);
                        }

                        check.CountTag++;

                        db.SaveChanges();
                        tagString += check.TagID.ToString() + ",";
                    }
                }

                newPost.Tags = tagString;
                db.SaveChanges();
                WriteLogToDB(this.UserID, this.Fullname, "Thêm mới bài viết", this.Fullname + " tạo bài mới : " + newPost.PostTitle, DateTime.Now);
                return Content("OK");
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }

        #endregion

        #region [Danh mục bài viết]

        // CateType == 1 : Danh mục bài viết 
        public ActionResult CategoryPost()
        {
            InitView();

            PostCategoryModel model = new PostCategoryModel();
            List<Web_Category> lstCate = new List<Web_Category>();
            lstCate = db.Web_Category.Where(o => o.Active == true && o.CateType == 1).ToList();

            model.LstCate = lstCate;

            return View(model);
        }

        // Thêm mới loại bài viết
        public ActionResult AddCategory(string cateName, int parentId)
        {
            try
            {
                Web_Category addNew = new Web_Category();
                addNew.CategoryName = cateName;
                addNew.ParentId = parentId;
                addNew.Active = true;
                addNew.CateIcon = "";
                addNew.CateType = 1;
                addNew.CateURL = ToFriendlyUrl(cateName);
                db.Web_Category.Add(addNew);
                db.SaveChanges();

                return View("_TblPostCate", db.Web_Category.Where(o => o.Active == true && o.CateType == 1).ToList());
            }
            catch (Exception ex)
            {
                // WriteFileLog.WriteLog(string.Empty, ex);
                return Content("Error");
            }
        }

        public ActionResult GetParentCategory()
        {
            return View("_SelectParentCate", db.Web_Category.Where(o => o.Active == true && o.ParentId == 0).ToList());
        }

        // Sửa loại bài viết
        public ActionResult EditCateById(int cId, string cateName, int pId)
        {
            try
            {
                Web_Category editCate = db.Web_Category.Where(o => o.CategoryId == cId).FirstOrDefault();
                editCate.CategoryName = cateName;
                editCate.CateURL = ToFriendlyUrl(cateName);
                editCate.ParentId = pId;
                db.SaveChanges();
                return PartialView("_TblPostCate", db.Web_Category.Where(o => o.Active == true && o.CateType == 1).ToList());
            }
            catch (Exception ex)
            {
                return Content("Error");
            }
        }

        // Xóa danh mục bài viết
        public ActionResult DeleteCategory(int cateId)
        {
            try
            {
                Web_Category model = db.Web_Category.Where(o => o.CategoryId == cateId).FirstOrDefault();
                var lstChild = db.Web_Category.Where(o => o.ParentId == cateId).ToList();
                foreach (var item in lstChild)
                {
                    item.ParentId = 0;
                }
                model.Active = false;
                db.SaveChanges();
                return View("_TblPostCate", db.Web_Category.Where(o => o.Active == true && o.CateType == 1).ToList());
            }
            catch (Exception ex)
            {
                return Content("Error");
            }
        }


        #endregion
    }
}