using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
namespace MyBlog.Areas.User.Controllers
{
    public class UserController : Controller
    {
        Blog_DBEntities db = new Blog_DBEntities();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult UserRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserRegister(string FullName,string Uname , string PhoneNumber , string Email , string Password )
        {
            DateTime time = DateTime.Now;
            var temp = db.TBL_Users.Where(p => p.User_Email == Email).ToList();
            if(temp.Count == 0)
            {
                TBL_Users User = new TBL_Users()
                {
                    User_FullName = FullName,
                    User_PhoneNumber = PhoneNumber,
                    User_UName = Uname,
                    User_Email = Email,
                    User_Password = Password,
                    User_Status = 0,
                    User_RegisterationDate = time
                };
                db.TBL_Users.Add(User);
                db.SaveChanges();
                ViewBag.RegisterSuccessed = "عضویت شما با موفقیت انجام شد";
                return View("UserLogin");

            }
            ViewBag.NotValidPass = "کاربری با این ایمیل ثبت شده است";
            return View("UserRegister");
        }




        public ActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin( string Email , string Password)
        {
            var temp = db.TBL_Users.Where(p => p.User_Email == Email && p.User_Password == Password).ToList();
            if(temp.Count != 0)
            {
                Session["UserLogined"] = "True";
                Session["UserID"] = temp[0].User_ID;
                Session["UserUName"] = temp[0].User_UName;
                return View("UserPanel");
            }
            else
            {
                ViewBag.LoginFailed = "اطلاعات وارد شده نادرستند";
                return View("UserLogin");
            }
        }


       
        
    }
}