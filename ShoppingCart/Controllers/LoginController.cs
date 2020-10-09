using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(ShoppingCart.Models.User userModel)
        {
            string userId;
            userId = UserData.FindUser(userModel);

            if (userId == null)
            {
                //cannot find user
                ViewData["errmsg"] = "User not found or incorrect password";
                return View();
            }
            else if (HttpContext.Session.GetString("userid") != null)
            {
                //if user has added to cart before logging in
                HttpContext.Session.SetString("sessionid", HttpContext.Session.GetString("userid"));
                HttpContext.Session.SetString("userid", userId);
                CartData.DeleteCart(userId);
                CartData.UpdateId(HttpContext.Session.GetString("userid"), HttpContext.Session.GetString("sessionid"));
                HttpContext.Session.SetString("username", userModel.Username);

                return RedirectToAction("DisplayProduct", "Product");
            }
            else
            {
                HttpContext.Session.SetString("userid", userId);
                HttpContext.Session.SetString("username", userModel.Username);

                return RedirectToAction("DisplayProduct", "Product");
            }
        }

        //redirect from guest checkout
        public IActionResult Index2()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index2(ShoppingCart.Models.User userModel)
        {
            string userId;
            userId = UserData.FindUser(userModel);

            if (userId == null)
            {
                ViewData["errmsg"] = "User not found or incorrect password";
                return View();
            }
            else
            {
                HttpContext.Session.SetString("sessionid", HttpContext.Session.GetString("userid"));
                HttpContext.Session.SetString("userid", userId);
                CartData.DeleteCart(userId);
                CartData.UpdateId(HttpContext.Session.GetString("userid"), HttpContext.Session.GetString("sessionid"));
                HttpContext.Session.SetString("username", userModel.Username);

                return RedirectToAction("DisplayCart", "Cart");
            }
        }
    }
}
