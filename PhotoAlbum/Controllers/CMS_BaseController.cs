using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CMSWebsite.Models;

namespace CMSWebsite.Controllers
{
    public class CMS_BaseController : Controller
    {

        public int UserID;
        public int RoleID;
        public string ImgFace = "";
        public string Fullname = "";
        public string LoginName = "";
        public static PhotoAlbumEntities dbCMS = new PhotoAlbumEntities();

        // Khởi tạo thông tin 
        public void InitView()
        {
            try
            {
                if (Request.Browser.IsMobileDevice)
                {
                    if (Session["CMS_UserID"] == null)
                    {
                        ClearCookies();
                        Response.Redirect("~/CMS_Login/Index");
                    }
                    UserID = Convert.ToInt32(Session["CMS_UserID"]);
                    LoginName = Session["CMS_UserName"].ToString();
                    ImgFace = Session["CMS_ImgFace"].ToString();
                    Fullname = HttpUtility.UrlDecode(Session["CMS_FullName"].ToString());
                    RoleID = Convert.ToInt32(Session["CMS_RoleID"].ToString());
                    ViewBag.ImgFace = ImgFace;
                    ViewBag.FullName = Fullname;
                }
                else
                {
                    HttpCookie cms = Request.Cookies.Get("CMS_Cookie");
                    if (cms != null)
                    {
                        UserID = Convert.ToInt32(cms["CMS_UserID"]);
                        LoginName = cms["CMS_UserName"].ToString();
                        ImgFace = cms["CMS_ImgFace"].ToString();
                        Fullname = HttpUtility.UrlDecode(cms["CMS_FullName"].ToString());
                        RoleID = Convert.ToInt32(cms["CMS_RoleID"].ToString());
                        ViewBag.ImgFace = ImgFace;
                        ViewBag.FullName = Fullname;
                    }
                    else
                    {
                        ClearCookies();
                        Response.Redirect("~/CMS_Login/Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ClearCookies();
                Response.Redirect("~/CMS_Login/Index");
            }
        }

        // Xóa Cookie
        public void ClearCookies()
        {
            if (Request.Cookies["CMS_Cookie"] != null)
            {
                HttpCookie myCookie = new HttpCookie("CMS_Cookie");
                myCookie.Value = null;
                myCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(myCookie);

            }
            Session["CMS_UserID"] = null;
        }

        #region [Các hàm tiện ích dùng chung]

        // Chuyển đổi URL chuẩn SEO
        public string ToFriendlyUrl(string title)
        {
            if (title == null) return "";

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåąạậấầẫắằẵẳảặẩă".Contains(s))
            {
                return "a";
            }
            else if ("èéêëęệếềệẻểẹẽễ".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïıịỉ".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőðơờớợốộồỗổởỏỡọ".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭůưứừựụủửữ".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿỹýỳỵ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if ("đđ".Contains(s))
            {
                return "d";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }

        //Chuyển số thành chữ 
        public string ConvertMoney(string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if (i + j == len - 1)
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += dv[n - j - 1] + " ";
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            return doc;
        }

        // Chuyển số thành định dạng có .,
        public static string ConvertDot(string oldvalue)
        {
            int len = oldvalue.Length;
            string returnValue;
            string part1 = "";
            string part2 = "";
            string part3 = "";
            string part4 = "";

            if (len <= 3) { part1 = oldvalue; }
            if (len > 3 & len <= 6)
            {
                part1 = oldvalue.Substring(0, len - 3);
                part2 = ',' + oldvalue.Substring(len - 3, 3);
            }

            if (len > 6 & len <= 9)
            {
                part1 = oldvalue.Substring(0, len - 6);
                part2 = ',' + oldvalue.Substring(len - 6, 3);
                part3 = ',' + oldvalue.Substring(len - 3, 3);
            }

            if (len > 9 & len <= 11)
            {
                part1 = oldvalue.Substring(0, len - 9);
                part2 = ',' + oldvalue.Substring(len - 9, 3);
                part3 = ',' + oldvalue.Substring(len - 6, 3);
                part4 = ',' + oldvalue.Substring(len - 3, 3);
            }

            if (len > 11)
            {
                part1 = "";
            }
            returnValue = part1 + part2 + part3 + part4;

            return returnValue;
        }

        // Chuyển định dạng MM/dd/yyyy thành dd/MM/yyyy. Type = 0 : đầu ngày, = 1 : cuối ngày
        public static DateTime ConvertDateTime(string datetime, int type = 0)
        {
            string[] subDate;
            if (datetime.Contains('-'))
            {
                subDate = datetime.Split('-');
            }
            else
            {
                subDate = datetime.Split('/');
            }
            if (type == 0)
            {
                return new DateTime(Convert.ToInt32(subDate[2]), Convert.ToInt32(subDate[1]), Convert.ToInt32(subDate[0]), 0, 0, 0);
            }
            else
            {
                return new DateTime(Convert.ToInt32(subDate[2]), Convert.ToInt32(subDate[1]), Convert.ToInt32(subDate[0]), 23, 59, 59);
            }
        }

        // Lưu lịch sử thao tác 
        public static void WriteLogToDB(int userID,string fullname, string title, string contentLog, DateTime date)
        {
            CMS_Log newLog = new CMS_Log();
            newLog.ContentLog = contentLog;
            newLog.Date = date;
            newLog.Title = title;
            newLog.UserID = userID;
            newLog.Fullname = fullname;
            dbCMS.CMS_Log.Add(newLog);
            dbCMS.SaveChanges();
        }

        // Chuyển kích thước file thành chữ
        public string ConvertFileSize(long fileSize)
        {
            int kb =  (int)(fileSize / 1024); // kb
            double mb = ((double)kb / 1024);
            if (mb >= 1)
            {
                return Math.Round(mb, 2) + " MB";
            }
            else
            {
                return kb + " KB";
            }
        }

        #endregion

    }
}