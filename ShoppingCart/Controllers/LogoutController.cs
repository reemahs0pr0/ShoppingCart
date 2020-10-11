using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("DisplayProduct", "Product");
        }
    }
}
