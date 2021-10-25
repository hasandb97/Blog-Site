using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using MyBlog.Models;
using PagedList;


namespace MyBlog.Areas.Comment.Controllers
{
    public class CommentController : Controller
    {
        Blog_DBEntities db = new Blog_DBEntities();

        // GET: Comment
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult ShowAllComments(int page = 1, int pagesize = 10)
        {
            if (Session["AdminLogined"] == "True")
            {
                List<TBL_Comments> products = db.TBL_Comments.OrderByDescending(p => p.Comment_ID).ToList();
                PagedList<TBL_Comments> model = new PagedList<TBL_Comments>(products, page, pagesize);
                return View(model);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin", new { Area = "Admin" });
            }
        }

        public ActionResult ShowSubmitedComment(int page = 1, int pagesize = 10)
        {
            if (Session["AdminLogined"] == "True")
            {
                List<TBL_Comments> products = db.TBL_Comments.OrderByDescending(p => p.Comment_ID).Where(p => p.Comment_Status == 1).ToList();
                PagedList<TBL_Comments> model = new PagedList<TBL_Comments>(products, page, pagesize);
                return View(model);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin", new { Area = "Admin" });
            }
        }

       



        [HttpPost]
        public ActionResult NewComment(string UName , string Email , string Txt)
        {
            string name = UName;
            string email = Email;
            string t = Txt;
            return View();
        }



        public ActionResult ShowCommentToSubmit(int page = 1, int pagesize = 10)
        {
            if (Session["AdminLogined"] == "True")
            {
                List<TBL_Comments> products = db.TBL_Comments.OrderByDescending(p => p.Comment_ID).Where(p => p.Comment_Status == 0).ToList();
                PagedList<TBL_Comments> model = new PagedList<TBL_Comments>(products, page, pagesize);
                return View(model);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin", new { Area = "Admin" });
            }
        }






        public ActionResult Delete(int id)
        {
            if (Session["AdminLogined"] == "True")
            {
                var temp = db.TBL_Comments.First(p => p.Comment_ID == id);
                db.TBL_Comments.Remove(temp);
                db.SaveChanges();
                return RedirectToAction("ShowAllComments", "Comment");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin", new { Area = "Admin" });
            }
        }

        public ActionResult PublishComment(int id)
        {
            if (Session["AdminLogined"] == "True")
            {
                var temp = db.TBL_Comments.First(p => p.Comment_ID == id);
                temp.Comment_Status = 1;
                db.SaveChanges();
                return RedirectToAction("ShowAllComments", "Comment");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin", new { Area = "Admin" });
            }
        }

        public ActionResult UnPublishComment(int id)
        {
            if (Session["AdminLogined"] == "True")
            {
                var temp = db.TBL_Comments.First(p => p.Comment_ID == id);
                temp.Comment_Status = 0;
                db.SaveChanges();
                return RedirectToAction("ShowAllComments", "Comment");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin", new { Area = "Admin" });
            }
        }


        


       

    }
}