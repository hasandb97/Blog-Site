using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Models;
using PagedList;


namespace MyBlog.Controllers
{
    


    public class HomeController : Controller
    {
        Blog_DBEntities db = new Blog_DBEntities();
        public ActionResult Index(int page = 1 , int pagesize = 5)
        {
            List<TBL_Posts> products = db.TBL_Posts.OrderByDescending( p => p.Post_ID).ToList();
            PagedList<TBL_Posts> model = new PagedList<TBL_Posts>(products, page, pagesize);
            return View(model);
        }


        public ActionResult Blog(int page = 1, int pagesize = 5)
        {
            List<TBL_Posts> products = db.TBL_Posts.OrderByDescending(p => p.Post_ID).Where(p => p.Post_Status == 1).ToList();
            PagedList<TBL_Posts> model = new PagedList<TBL_Posts>(products, page, pagesize);
            return View(model);
        }

        public ActionResult BlogCategory(int id, int page = 1, int pagesize = 5)
        {
            List<TBL_Posts> products = db.TBL_Posts.OrderByDescending(p => p.Post_ID).Where(p => p.Post_Status == 1 && p.TBL_Categories.Category_ID == id).ToList();
            PagedList<TBL_Posts> model = new PagedList<TBL_Posts>(products, page, pagesize);
            return View("BlogCategory", model);
        }





        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       

        public ActionResult SideBar()
        {
            var temp = db.TBL_Posts.Where(p => p.Post_Status == 1).OrderByDescending(p => p.Post_ID).Take(5);
            var category = db.TBL_Categories.ToList();
            List<TBL_Categories> children = new List<TBL_Categories>();
            

            string Cat(int Parent_id)
            {
                children = category.Where(p => p.Parent_ID == Parent_id).ToList();
                if(children.Count == 0)
                {
                    return "";
                }
                string S = "";
                foreach(var test in children)
                {
                    S += "<li><a href='/Home/BlogCategory/"+ test.Category_ID + "'>" + test.Category_Name + " ("+test.TBL_Posts.Count +")</a></li>";
                    if(Cat(test.Category_ID) != "")
                    {
                        S += "<ul>" + Cat(test.Category_ID) + "</ul>";

                    }

                }

                return S; 

            }
            string mycat = Cat(0);
            ViewBag.Mycat = mycat;

            return PartialView("SidebarHome",temp);
        }

       


    }
}