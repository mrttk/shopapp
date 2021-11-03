using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender; 
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,IEmailSender emailSender)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
        }
        
        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel(){
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                ModelState.AddModelError("","Wrong password or user name!");
                return View(model);
            }

            // if (!await _userManager.IsEmailConfirmedAsync(user))
            // {
            //     ModelState.AddModelError("","We have sent you an e-mail to activate your membership. Please confirm your subscription.");
            //     return View(model);
            // }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }
            ModelState.AddModelError("","Wrong password or user name!");
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //generate token
                var url = Url.Action("ConfirmEmail","Account", new {
                    userId = user.Id,
                    token = code
                });
                Console.WriteLine(url);
                //email
                await _emailSender.SendEmailAsync(model.Email, "Confirm your account", $"Please <a href='http://localhost:5000{url}'>click</a> the link to confirm your email account.");
                return RedirectToAction("Login","Account");
            }
            ModelState.AddModelError("","An unknown error has occurred. Please try again");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail (string userId, string token)
        {
            if (userId == null || token == null)
            {
                CreateMessage("Invalid token","danger");
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user,token);
                if (result.Succeeded)
                {
                    CreateMessage("The user has been confirmed!","success");
                    return View();
                }            
            }
            CreateMessage("The user is not registered.","warning");
            return View();
        }

        private void CreateMessage(string message,string alerttype)
        {
            var info = new AlertMessage()
            {
                Message = message,
                AlertType = alerttype
            };

            TempData["message"] = JsonConvert.SerializeObject(info);
        }
    }
}