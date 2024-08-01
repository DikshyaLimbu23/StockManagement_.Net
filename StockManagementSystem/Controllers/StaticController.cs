using Microsoft.AspNetCore.Mvc;

namespace StockManagementSystem.Controllers
{
    public class StaticController : Controller
    {
        public  IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin")) 
                { 
                    return RedirectToAction("Index", "Admin");
                }
                else {

                    return RedirectToAction("Index", "Home");
                }
                        
            }   
            return View();
           
        }
    }
}
