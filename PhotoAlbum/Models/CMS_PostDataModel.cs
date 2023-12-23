using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSWebsite.Models
{
    public class PostCategoryModel
    {
        public List<Web_Category> LstCate { get; set; }
    }

    public class PostModel
    {
        public string tags { get; set; }
        public string portfolioDate { get; set; }
        public string portfolioAddress { get; set; }
        public string imageGallery { get; set; }
        public string postId
        {
            get;
            set;
        }
        public string titlePost
        {
            get;
            set;
        }
        public string contentPost
        {
            get;
            set;
        }
        public string typePost
        {
            get;
            set;
        }
        public string catePost
        {
            get;
            set;
        }
        public string imagePost
        {
            get;
            set;
        }

        public string seoDescPost
        {
            get;
            set;
        }
        public string seoKeywordPost
        {
            get;
            set;
        }
        public string URL { get; set; }
        public string description { get; set; }
        public bool Active { get; set; }
    }

    public class PostView
    {
        public string UrlFriendly { get; set; }
        public int PostId
        {
            get;
            set;
        }
        public int PostAuthorId { get; set; }
        public string PostTitle
        {
            get;
            set;
        }
        public string PostDate
        {
            get;
            set;
        }
        public string PostCate
        {
            get;
            set;
        }
        public string PostAuthor
        {
            get;
            set;
        }
        public string PostDesc
        {
            get;
            set;
        }
        public string ImgFace { get; set; }
    }

    public class EditPostModel
    {
        public Web_Post PostEdit { get; set; }

        public List<Web_Category> LstCate
        {
            get;
            set;
        }
        public List<Web_PostCategory> LstPostCate { get; set; }
        public string Tags { get; set; }
    }
}