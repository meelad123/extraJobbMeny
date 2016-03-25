using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace menu.Controllers
{
    public class HomeController : Controller
    {
        //Anrop: this.createUser()
        //skapar en ny användare
        //Anrop den i ActionResult Index() sen kör programmet för att skapa en ny användare p.s ändra userId, userName, pass och role
        //Glöm inte att ta bort anropet efter du har skapat användaren så du inte skapar samma användare en gång till
        private void createUser()
        {
            var crypto = new SimpleCrypto.PBKDF2();
            Models.User addUser = new Models.User();
            string pass = "1234";
            addUser.userId = 2;
            addUser.userName = "9207240178";
            addUser.role = 1;
            addUser.password = crypto.Compute(pass);
            addUser.passSalt = crypto.Salt;

            Models.User.createUser(addUser);
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _navBar()
        {
            if (Session["user"] != null)
            {
                int roleId = ((Models.User)Session["user"]).role;
                return PartialView("~/Views/Shared/_navbarView.cshtml", Models.menuLink.getAllLinks(roleId).ToList());
            }
            else 
            {
                List<Models.menuLink> mList = new List<Models.menuLink>();
                return PartialView("~/Views/Shared/_navbarView.cshtml", mList.ToList());
            }
            
        }

        public PartialViewResult _logIn() 
        {
            return PartialView("~/Views/Shared/_logIn.cshtml");
        }

        public ActionResult LogoutPage()
        {
            Session["user"] = null;
            return View("Index");
        }

        //Login logic: Den tar emot data från en ajax post request och sen kollar om användaren finns i databasen
        //och om lösenordet stämmer, om allt stämmer sparas användaren e in Session kallad user.
        public ActionResult Login(FormCollection collection)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            Models.User u = new Models.User();
            u.userName = collection["user"];
            u.password = collection["password"];
            Models.User loggedIn = u.checkUser(u); //kollar om anvädaren finns i databasen
            if (loggedIn != null)
            {
                if (loggedIn.password == crypto.Compute(u.password, loggedIn.passSalt)) //kollar om lösenordet stämmer
                {
                    Session["user"] = loggedIn;
                    return Json(new { msg = "success"});
                }
                else
                {
                    ViewBag.WrongInfo = "Fel lösenord!";
                    return Json(new { msg = "Fel lösenord" });
                }
            }
            else
            {
                ViewBag.WrongInfo = "Fel info. Försök igen!";
                return Json(new { msg = "Fel info. Försök igen." });
            }
        }

    }
}