using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using StockManagementSystem.Models;
using System.Diagnostics;

namespace StockManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InventoryManagementContext _context;
        private readonly IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger, InventoryManagementContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult ProfileImage()
        {
            var p = _context.Users.Where(u => u.Userid.Equals(Convert.ToInt16(User.Identity!.Name))).FirstOrDefault();
            ViewData["Img"] = p?.Userfile;
            return PartialView("_Profile");
        }

        public IActionResult Profile(int id)
        {
            var user = _context.Users.Where(x => x.Userid == id);
            return View(user);
        }

        [HttpGet]
        public IActionResult ProfileUpdate()
        {
            var p = _context.Users.First(x => x.Userid == Convert.ToInt32(User.Identity!.Name));
            UserEdit u = new()
            {
               Userid = p.Userid,
               UserRole = p.UserRole!,
               Userfile = p.Userfile,
               Username = p.Username,
               Locations = p.Locations,
               EmailAddress = p.EmailAddress,
               RegisterDate = p.RegisterDate,
               UserPassword = p.UserPassword,

            };
             return View(u);
        }

        [HttpPost]
        public IActionResult ProfileUpdate(UserEdit user) 
        {
            var userli = _context.Users.Find(user.Userid);
            if (userli!.Userfile != null)
            {
                // Define the path to the existing file
                var oldFilePath = Path.Combine(_env.WebRootPath, "UserPhoto", userli.Userfile);

                // Check if the existing file exists and delete it
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            
            if (user.Userphoto!= null)
            {
                string filename = "UpdateImage" + Guid.NewGuid() + Path.GetExtension(user.Userphoto.FileName);
                string filepath = Path.Combine(_env.WebRootPath, "UserPhoto", filename);
                using(FileStream stream = new FileStream(filepath, FileMode.Create))
                {
                    user.Userphoto.CopyTo(stream);
                }
                user.Userfile = filename;
            };

         
           
            userli!.EmailAddress = user.EmailAddress;
            userli.RegisterDate = user.RegisterDate;
            userli.UserRole = user.UserRole;
            userli.Userid = user.Userid;
            userli.Username = user.Username;
            userli.Userfile = user.Userfile;
            userli.Locations = user.Locations;

           
            _context.Users.Update(userli);
            _context.SaveChanges();          
            return RedirectToAction("ProfileUpdate");
        }

        public IActionResult Delete(int id)
        {
            var u = _context.Users.Find(id);
            if (u != null)
            {
                _context.Users.Remove(u);
                _context.SaveChanges();
                return RedirectToAction("Index", "Homw");
            }
           
            ModelState.AddModelError("", "User not found.");
            return RedirectToAction("Index", "Homw");
        }

        public IActionResult Privacy()
        {
            return View();
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }

