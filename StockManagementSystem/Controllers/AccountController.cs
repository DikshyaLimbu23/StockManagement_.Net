using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagementSystem.Models;
using StockManagementSystem.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using System.Net;
using System;

namespace StockManagementSystem.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly InventoryManagementContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IDataProtector _dataProtector;
        public AccountController(InventoryManagementContext context, IDataProtectionProvider provider, DataSecurityKey key, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            _dataProtector = provider.CreateProtector(key.Key);

        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserEdit u)
        {
            var user =  _context.Users.ToList();
          
            if (user == null)
            {
                return BadRequest("Not Found");
            }
            else
            {
                var us = user.Where(x => x.EmailAddress.ToUpper().Equals(u.EmailAddress.ToUpper()) && _dataProtector.Unprotect(x.UserPassword!).Equals((u.UserPassword))).FirstOrDefault();

                if (us != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,us.Userid.ToString()),
                        new Claim(ClaimTypes.Role,us.UserRole!.ToString()),

                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName or Password");
                    return View();
                }
            }
                   
        }

        [HttpGet]
        public IActionResult Changepassword()
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult Changepassword(ChangePassword ps)
        {
            var u = _context.Users.Where(x=>x.Userid.Equals(Convert.ToInt32(User.Identity!.Name))).FirstOrDefault();
            if (_dataProtector.Unprotect(u.UserPassword!)!= ps.EmailAddress)
            {
                ModelState.AddModelError("", "Check your current password");
            }
            if (ps.NewPassword == ps.ConfirmPassword)
            {
                u.UserPassword =_dataProtector.Protect( ps.NewPassword);
                _context.Users.Update(u);
                _context.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("", "Confirm password is incorrect");

            }

            return View();
        }

    
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Static");
        }

    
        public IActionResult Dashboard()
        {
           
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index","Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignUp() 
        {

            return View();

        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(UserEdit u)
        {
            var user = _context.Users.Where(e=>e.EmailAddress.Equals(u.EmailAddress)).FirstOrDefault();
            try
            {
                if (user == null)
                {
                    int maxid;
                    if (_context.Users.Any())
                        maxid = Convert.ToInt32(_context.Users.Max(x => x.Userid) + 1);
                    else
                        maxid = 1;

                    u.Userid = maxid;

                    if (u.Userphoto != null)
                    {
                        string fileName = "Image" + Guid.NewGuid() + Path.GetExtension(u.Userphoto.FileName);
                        string filePath = Path.Combine(_env.WebRootPath, "UserPhoto", fileName);
                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            u.Userphoto.CopyTo(stream);
                        }
                        u.Userfile = fileName;
                    }

                    User us = new()
                    {
                        Userid = u.Userid,
                        UserPassword = _dataProtector.Protect(u.UserPassword),
                        Username = u.Username,
                        EmailAddress = u.EmailAddress,
                        Locations = u.Locations,
                        UserRole = u.UserRole,
                        RegisterDate = u.RegisterDate,
                        Userfile = u.Userfile,

                    };
                    _context.Users.Add(us);
                    _context.SaveChanges();
                    return RedirectToAction("Login");

                   }

                else
                {
                    ModelState.AddModelError("", "Email Address is already exists.");
                }
            }
            catch(Exception)
            {

                ModelState.AddModelError("", "");
                return View(u);
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ForgetPassword(UserEdit edit)
        {
           

            if (edit.EmailAddress != null)
            {
                Random r = new Random();
                HttpContext.Session.SetString("token", r.Next(9999).ToString());
                var token = HttpContext.Session.GetString("token");

                var user = _context.Users.Where(u => u.EmailAddress == edit.EmailAddress).FirstOrDefault();
                if (user != null)
                {

                    try
                    {
                        SmtpClient s = new()
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential("dikshyalimbu786@gmail.com", "ovtr ussq jtyg dgxw"),
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network
                        };

                        int sessionTimeoutMinutes = 5;

                        MailMessage m = new()
                        {
                            From = new MailAddress("dikshyalimbu786@gmail.com"),
                            Subject = "Forgot Password token",
                            Body = $@"
                            <div>
                                <a href='https://localhost:7221/Account/ResetPassword/{_dataProtector.Protect(token!)}' 
                                   style='background-color:green; color:white; padding:10px; width:200px; border-radius:4px;'>
                                    Reset Password
                                </a> 
                                <p>Token number: {token}</p>
                                <p>This link is valid for the next {sessionTimeoutMinutes} minutes.</p>
                            </div>",
                            IsBodyHtml = true
                        };

                        HttpContext.Session.SetString("email", user.EmailAddress);
                        m.To.Add(user.EmailAddress);
                        s.Send(m);

                    }
                    catch (Exception ex)
                    {
                        return Json(ex);
                    }
                    return RedirectToAction("VerifyToken");
                }
                else
                {
                    ModelState.AddModelError("", "This Email-Address is not register.");
                    return View(edit);
                }
            }
            return Json("failed");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult VerifyToken()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult VerifyToken(UserEdit e)
        {
            var token = HttpContext.Session.GetString("token");
            if (token == e.EmailToken)
            {
                var et = _dataProtector.Protect(e.EmailToken!);
                return RedirectToAction("ResetPassword", new { id = et });
            }
            else
            {
                return Json("failed");
            }

        }
        [AllowAnonymous]
        public IActionResult ResetPassword(string id)
        {
            try
            {
                var token = HttpContext.Session.GetString("token");
                var eToken = _dataProtector.Unprotect(id);
                if (token == eToken)
                {
                    return View(new ChangePassword { EmailToken = id });
                }
                else
                {
                    return RedirectToAction("ForgotPassword");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("ForgotPassword");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ResetPassword(ChangePassword c)
        {
            var token = HttpContext.Session.GetString("token");
            var eToken = _dataProtector.Unprotect(c.EmailToken!);
            if (token == eToken)
            {
                var email = HttpContext.Session.GetString("email");
                var us = _context.Users.Where(u => u.EmailAddress == email).FirstOrDefault();
                if (us != null)
                {
                    if (c.NewPassword == c.ConfirmPassword)
                    {
                        us.UserPassword = _dataProtector.Protect(c.NewPassword);
                        _context.Users.Update(us);
                        _context.SaveChanges();
                        return RedirectToAction("Login", "Authentication");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Confirn Password does not matched. Please Try Again!.");
                        return View(c);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "failed");
                    return View(c);
                }
            }
            else
            {
                return RedirectToAction("ForgotPassword");
            }
        }


        }
}
