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
        public IActionResult Authenticate(ShoppingCart.Models.User userModel)
        {
            string userId;

            if (userModel.Username == null || userModel.Password == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                userId = UserData.FindUser(userModel);
            }

            if (userId == null)
            {
                //return RedirectToAction("productpage", "Login");
                return RedirectToAction("Index", "Login");
            }
            else if (HttpContext.Session.GetString("userid") != null)
            {
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

                //  return RedirectToAction("Index", "Login");
                return RedirectToAction("DisplayProduct", "Product");
            }
        }
        public IActionResult Index2()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Authenticate2(ShoppingCart.Models.User userModel)
        {
            string userId;

            if (userModel.Username == null || userModel.Password == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                userId = UserData.FindUser(userModel);
            }

            if (userId == null)
            {
                //return RedirectToAction("productpage", "Login");
                return RedirectToAction("Index", "Login");
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
