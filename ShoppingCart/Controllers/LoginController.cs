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
        //when user click 'login'
        public IActionResult Index()
        {
            return View();
        }

        //authenticate user
        [HttpPost]
        public IActionResult Index(ShoppingCart.Models.User userModel)
        {
            //check if user exist in db
            string userId = UserData.FindUserId(userModel);

            if (userId == null)
            {
                //if cannot find user

                ViewData["errmsg"] = "User not found or incorrect password";

                return View();
            }
            else if (HttpContext.Session.GetString("userid") != null)
            {
                //if user has added to cart before logging in

                //store GUID as session id
                HttpContext.Session.SetString("sessionid", HttpContext.Session.GetString("userid"));

                //replace user id with real user id from db
                HttpContext.Session.SetString("userid", userId);

                //overwrite existing cart with new cart 
                CartData.DeleteCart(userId);
                CartData.UpdateId(HttpContext.Session.GetString("userid"), HttpContext.Session.GetString("sessionid"));

                //set 'name' key with name of user
                HttpContext.Session.SetString("name", UserData.FindName(HttpContext.Session.GetString("userid")));

                return RedirectToAction("DisplayProduct", "Product");
            }
            else
            {
                //if user directly login

                //set session with user id and name
                HttpContext.Session.SetString("userid", userId);
                HttpContext.Session.SetString("name", UserData.FindName(HttpContext.Session.GetString("userid")));

                return RedirectToAction("DisplayProduct", "Product");
            }
        }

        //redirect from guest 'checkout'
        public IActionResult Index2()
        {
            return View();
        }

        //same as HttpPost Index2() but redirect to 'View Cart'
        [HttpPost]
        public IActionResult Index2(ShoppingCart.Models.User userModel)
        {
            string userId;
            userId = UserData.FindUserId(userModel);

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
                HttpContext.Session.SetString("name", UserData.FindName(HttpContext.Session.GetString("userid")));

                return RedirectToAction("DisplayCart", "Cart");
            }
        }
    }
}
