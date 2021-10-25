using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Models;
using PagedList;

namespace MyBlog.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        Blog_DBEntities db = new Blog_DBEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdminRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminRegister(string FullName, string PhoneNumber, string Email, string Password)
        {
            TBL_Admin Admin = new TBL_Admin()
            {
                Admin_FullName = FullName,
                Admin_Email = Email,
                Admin_Password = Password,
                Admin_PhoneNumber = PhoneNumber
            };
            db.TBL_Admin.Add(Admin);
            db.SaveChanges();
            ViewBag.AdminRegisterSucceed = "عضویت شما به عنوان ادمین با موفقیت انجام شد";
            return View("AdminLogin");
        }


        public ActionResult AdminLogin()
        {
            if (Session["AdminLogined"] == "True")
            {
                return View("AdminPanel");
            }
            else
            {
                return View();
            }
        }


        [HttpPost]
        public ActionResult AdminLogin(string Email, string Password)
        {
            var temp = db.TBL_Admin.Where(p => p.Admin_Email == Email && p.Admin_Password == Password).ToList();
           
           
            if (temp.Count != 0)
            {
                Session["AdminID"] = temp[0].Admin_ID;
                Session["AdminLogined"] = "True";
                Session["AdminInfo"] = temp;
                return RedirectToAction("AdminPanel");
            }
            else
            {
                ViewBag.LoginFailed = "اطلاعات وارد شده نادرستند";
                return View("AdminLogin");
            }
        }

        public ActionResult AdminLogout()
        {
            if(Session["AdminLogined"] == "True")
            {
                Session["AdminLogined"] = "False";
                return RedirectToAction("AdminLogin");
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        public ActionResult AdminPanel()
        {
            if (Session["AdminLogined"] == "True")
            {
                ViewBag.AdminInfo = Session["AdminInfo"];
                return View("AdminPanel");
            }
            else
            {
                return View("AdminLogin");
            }
        }


        


        public ActionResult ShowAllUsers(int page = 1, int pagesize = 10)
        {
            if (Session["AdminLogined"] == "True")
            {
                List<TBL_Users> products = db.TBL_Users.OrderByDescending(p => p.User_ID).ToList();
                PagedList<TBL_Users> model = new PagedList<TBL_Users>(products, page, pagesize);
                return View(model);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        
        public ActionResult ShowSubmitedUser(int page = 1 , int pagesize = 10)
            {
                if (Session["AdminLogined"] == "True")
                {
                List<TBL_Users> products = db.TBL_Users.OrderByDescending(p => p.User_ID).Where(p => p.User_Status == 1).ToList();
                PagedList<TBL_Users> model = new PagedList<TBL_Users>(products, page, pagesize);
                return View(model);
            }
                else
                {
                    return RedirectToAction("AdminLogin");
                }
            }


        public ActionResult UserToSbumit(int page = 1 , int pagesize=10)
        {
            if (Session["AdminLogined"] == "True")
            {
                List<TBL_Users> products = db.TBL_Users.OrderByDescending(p => p.User_ID).Where(p => p.User_Status == 0).ToList();
                PagedList<TBL_Users> model = new PagedList<TBL_Users>(products, page, pagesize);
                return View(model);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        public ActionResult DeleteUser(int id)
        {
            if (Session["AdminLogined"] == "True")
            {
                var temp = db.TBL_Users.First(p => p.User_ID == id);
                db.TBL_Users.Remove(temp);
                db.SaveChanges();
                return RedirectToAction("ShowAllUsers");

            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        public ActionResult ShowUserDetail(int id)
        {
            if (Session["AdminLogined"] == "True")
            {
                var temp = db.TBL_Users.First(p => p.User_ID == id);
                return View(temp);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        [HttpPost]
        public ActionResult UserUpdateInfo()
        {
            return View();
        }

        public ActionResult PublisheUser(int id)
        {
            var temp = db.TBL_Users.First(p => p.User_ID == id);
            temp.User_Status = 1;
            db.SaveChanges();
            return RedirectToAction("ShowUserDetail", new { id = temp.User_ID });
        }

        public ActionResult UnPublishUser(int id)
        {
            var temp = db.TBL_Users.First(p => p.User_ID == id);
            temp.User_Status = 0;
            db.SaveChanges();
            return RedirectToAction("ShowUserDetail", new { id = temp.User_ID });
        }



    }
}