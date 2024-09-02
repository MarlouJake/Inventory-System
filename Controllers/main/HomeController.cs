using InventorySystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace InventorySystem.Controllers.main
{

    public class HomeController() : Controller
    {
        public void PrintUrl(string? url)
        {
            if (url != null)
            {
                Console.WriteLine($"Current Page URL: {url}");
            }
            else
            {
                Console.WriteLine("URL is null.");
            }
        }

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
            PrintUrl(url);
            //var user = new LoginModel();
            ViewData["Title"] = "Login";
            return PartialView();
        }



        public IActionResult Info()
        {
            var url = Url.Action("Info", "Home");
            PrintUrl(url);
            ViewData["Title"] = "Info";
            return PartialView();
        }

        //[HttpGet]
        [Route("/login/admin")]
        public IActionResult AdminLogin()
        {
            var admin = new AdminModel();
            var url = Url.Action("AdminLogin", "Home");
            PrintUrl(url);
            ViewData["Title"] = "Login";
            return View(admin);
        }


        [Route("/home/access-denied")]
        public IActionResult AccessDenied()
        {
            //return new HttpStatusCodeResult(HttpStatusCode.Forbidden); // 403 Forbidden
            var url = Url.Action("AccessDenied", "Home");
            PrintUrl(url);
            ViewData["Title"] = "Access Denied";
            return View();
        }



        public IActionResult Error()
        {
            var url = Url.Action("Error", "Home");
            PrintUrl(url);
            ViewData["Title"] = "Error";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
