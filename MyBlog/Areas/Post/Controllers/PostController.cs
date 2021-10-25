using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using MyBlog.Models;
using PagedList;


namespace MyBlog.Areas.Post.Controllers
{
    public class PostController : Controller
    {
        Blog_DBEntities db = new Blog_DBEntities();

        PersianCalendar p = new PersianCalendar();
        // GET: Post/Post
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NComment(int id)
        {
            var temp = id;
            ViewBag.id = temp;
            var test = db.TBL_Comments.OrderByDescending(p => p.Comment_ID).Where(p => p.Comment_Status == 1 && p.Post_ID== id);
            return PartialView("Comments",test);
        }



        [HttpPost]
        public ActionResult NewComment( string Txt,int id)
        {
            int ID = id;

            if (Session["UserLogined"] == "True")
            {
                DateTime t = DateTime.Now;
                TBL_Comments comment = new TBL_Comments()
                {
                    Comment_Text = Txt,
                    User_ID = Convert.ToInt32(Session["UserID"]),
                    Post_ID = ID,
                    Comment_Date = t,
                    Comment_Status = 0,
                };
                db.TBL_Comments.Add(comment);
                db.SaveChanges();
                string mys = "نظر شما با موفقیت ثبت شد و در انتظار تایید ادمین می باشد";
                ViewBag.Message = mys;
                return RedirectToAction("ShowPost", new { id = ID });
            }
            else
            {
                return RedirectToAction("ShowPost", new { id = ID });
            }
        }


        public ActionResult SideBar()
        {
            var temp = db.TBL_Posts.Where(p => p.Post_Status == 1).OrderByDescending(p => p.Post_ID).Take(5);
            var category = db.TBL_Categories.ToList();
            List<TBL_Categories> children = new List<TBL_Categories>();


            string Cat(int Parent_id)
            {
                children = category.Where(p => p.Parent_ID == Parent_id).ToList();
                if (children.Count == 0)
                {
                    return "";
                }
                string S = "";
                foreach (var test in children)
                {
                    S += "<li><a href='/Home/BlogCategory/" + test.Category_ID + "'>" + test.Category_Name + " (" + test.TBL_Posts.Count + ")</a></li>";
                    if (Cat(test.Category_ID) != "")
                    {
                        S += "<ul>" + Cat(test.Category_ID) + "</ul>";

                    }

                }

                return S;

            }
            string mycat = Cat(0);
            ViewBag.Mycat = mycat;

            return PartialView("SidebarHome", temp);
        }

        public ActionResult NewPost()
        {
            if (Session["AdminLogined"] == "True")
            {
                var temp = db.TBL_Categories.ToList();
                return View(temp);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin", new { Area = "Admin" });
            }

        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewPost(string Title, int Category, string Summery, string mytextarea, HttpPostedFileBase Fileup)
        {
            DateTime dt = DateTime.Now;
            PersianCalendar p = new PersianCalendar();
            ViewBag.p = p;
            TBL_Posts Post = new TBL_Posts()
            {
                Admin_ID = Convert.ToInt32(Session["AdminID"]),
                Category_ID = Category,
                Post_Summery = Summery,
                Post_Title = Title,
                Post_Text = mytextarea,
                Post_Date = dt,
                Post_Status = 0,
                Post_CommentStatus =1,
                Post_Image = Guid.NewGuid() + Path.GetExtension(Fileup.FileName),
            };
        
            string MyPath = Server.MapPath("/PageImages");
            if (!Directory.Exists(MyPath))
            {
                Directory.CreateDirectory(MyPath);
            }
            string filename = MyPath + "/" + Post.Post_Image;
            Fileup.SaveAs(MyPath+"/"+Post.Post_Image);
            var thumbName = filename.Split('.').ElementAt(0) + "_thumb." + filename.Split('.').ElementAt(1);
            thumbName = Path.Combine(Server.MapPath("/PageImages"), thumbName);          
            Image MyThumb = Image.FromFile(MyPath + "/" + Post.Post_Image);
            Image thumb = MyThumb.GetThumbnailImage(100, 100, () => false, IntPtr.Zero);
            thumb.Save(thumbName);
            string ThumbName = Path.GetFileName(thumbName);
            Post.Post_Thumbnail = ThumbName;
            db.TBL_Posts.Add(Post);
            db.SaveChanges();
            ViewBag.Title = Title;
            ViewBag.Category = Category;
            ViewBag.Summery = Summery;
            ViewBag.txt = mytextarea;
            return RedirectToAction("ShowAllPostList", "Post");
        }

        [HttpGet]
        public ActionResult ShowPost(int id)
        {
            var post = db.TBL_Posts.First(t => t.Post_ID == id);
            var temp = db.TBL_Posts.First(t => t.Post_ID == id).Post_Date;
            PersianCalendar p = new PersianCalendar();
           string Persian = p.GetYear(temp.Value.Date).ToString()+" / "+ p.GetMonth(temp.Value.Date).ToString() +" / "+ p.GetDayOfMonth(temp.Value.Date).ToString();
            ViewBag.Persian = Persian;
            return View(post);
        }

        [HttpGet]
        public ActionResult ShowPostToAdmin(int id)
        {
            if (Session["AdminLogined"] == "True")
            {
                var temp = db.TBL_Posts.First(p => p.Post_ID == id);
                return View(temp);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin", new { Area = "Admin" });
            }
        }

        public ActionResult ShowAllPostList(int page = 1, int pagesize = 10)
        {
            List<TBL_Posts> products = db.TBL_Posts.OrderByDescending(p => p.Post_ID).ToList();
            PagedList<TBL_Posts> model = new PagedList<TBL_Posts>(products, page, pagesize);
            return View(model);
        }
        public ActionResult ShowPublishedPostList(int page = 1, int pagesize = 10)
        {
            List<TBL_Posts> products = db.TBL_Posts.OrderByDescending(p => p.Post_ID).Where(p => p.Post_Status == 1).ToList();
            PagedList<TBL_Posts> model = new PagedList<TBL_Posts>(products, page, pagesize);
            return View(model);
        }

        public ActionResult ShowUnPublishedPostList(int page = 1, int pagesize = 10)
        {
            List<TBL_Posts> products = db.TBL_Posts.OrderByDescending(p => p.Post_ID).Where(p => p.Post_Status == 0).ToList();
            PagedList<TBL_Posts> model = new PagedList<TBL_Posts>(products, page, pagesize);
            return View(model);
        }


        public ActionResult PublishPost(int id)
        {
            var temp = db.TBL_Posts.First(p => p.Post_ID == id);
            temp.Post_Status = 1;
            db.SaveChanges();
            return RedirectToAction("ShowAllPostList", "Post");
        }

        public ActionResult UnPublishPost(int id)
        {
            var temp = db.TBL_Posts.First(p => p.Post_ID == id);
            temp.Post_Status = 0;
            db.SaveChanges();
            return RedirectToAction("ShowAllPostList", "Post");
        }
        [HttpGet]
        public ActionResult DeletePost(int id)
        {
            var temp = db.TBL_Posts.First(p => p.Post_ID == id);
            db.TBL_Posts.Remove(temp);
            db.SaveChanges();
            return RedirectToAction("ShowAllPostList", "Post");
        }

        
        public ActionResult CommentStatus(int id)
        {
            var temp = db.TBL_Posts.First(p => p.Post_ID == id);
            if(temp.Post_CommentStatus == 1)
            {
                temp.Post_CommentStatus = 0;
            }
            else
            {
                temp.Post_CommentStatus = 1;
            }
            db.SaveChanges();
            db.SaveChanges();
            return RedirectToAction("ShowPublishedPostList", "Post");
        }
    }
}