using InventorySystem.Models.Page;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InventorySystem.Controllers.main
{

    public class HomeController() : Controller
    {


        [Route("/home/login")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        //[HttpGet]
        [Route("/login/user")]
        public IActionResult LoginPage()
        {
            var url = Url.Action("LoginPage", "Home");
            Messages.PrintUrl(url);
            //var user = new LoginModel();
            ViewBag.Title = "Login";
            return PartialView();
        }

        [HttpGet]
        [Route("signup")]
        public IActionResult SignUp()
        {
            var url = Url.Action("SignUp", "Home");
            Messages.PrintUrl(url);
            return PartialView();
        }


        public IActionResult Info()
        {
            var url = Url.Action("Info", "Home");
            Messages.PrintUrl(url);
            ViewData["Title"] = "Info";
            var adminuser = "admin || admin@admin.com";
            var demouser = "demo || demo@user.com";
            var adminpass = "@AdminLogger1234";
            var demouserpass = "@demo2024";
            var tempAdminPass = HashHelper.HashPassword(adminpass);
            var demoUserPass = HashHelper.HashPassword(demouserpass);
            Console.WriteLine("Temp Admin Username: {0}\nTemp Demo Username: {1}", adminuser, demouser);
            Console.WriteLine("Unhashed Temp Admin Password: {0}\nUnhashed Demo User Password: {1}", adminpass, demouserpass);
            Console.WriteLine("Hashed Temp Admin Password: {0}\nHashed Demo User Password: {1}", tempAdminPass, demoUserPass);
            return PartialView();
        }

        [Route("/home/access-denied")]
        public IActionResult AccessDenied()
        {
            //return new HttpStatusCodeResult(HttpStatusCode.Forbidden); // 403 Forbidden
            var url = Url.Action("AccessDenied", "Home");
            Messages.PrintUrl(url);
            ViewData["Title"] = "Access Denied";
            return View();
        }



        public IActionResult Error()
        {
            var url = Url.Action("Error", "Home");
            Messages.PrintUrl(url);
            ViewData["Title"] = "Error";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
