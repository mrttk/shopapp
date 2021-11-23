using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Extensions;
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
        private ICartService _cartService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,IEmailSender emailSender, ICartService cartService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
            this._cartService = cartService;
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

                // Create Cart
                _cartService.InitializeCart(user.Id);

                Console.WriteLine(url);
                //email
                // await _emailSender.SendEmailAsync(model.Email, "Confirm your account", $"Please <a href='http://localhost:5000{url}'>click</a> the link to confirm your email account.");
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
                TempData.Put("message", new AlertMessage(){
                    Title = "Invalid Token",
                    Message = "Invalid token!",
                    AlertType = "danger"
                });
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user,token);
                if (result.Succeeded)
                {

                    TempData.Put("message", new AlertMessage(){
                        Title = "Confirm Message",
                        Message = "The user has been confirmed!",
                        AlertType = "success"
                    });
                    return View();
                }            
            }
            TempData.Put("message", new AlertMessage(){
                        Title = "Register Message",
                        Message = "The user is not registered.",
                        AlertType = "warning"
                    });
            return View();
        }

        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = Url.Action("ResetPassword", "Account", new {
                userId = user.Id,
                token = code
            });
            
            await _emailSender.SendEmailAsync(Email,"Reset Password",$"<a href='http://localhost:5000{url}'>Click</a> on the link to reset your password.");

            return View();
        }
        
        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Home","Index");
            }

            var model = new ResetPasswordModel { Token = token };

            return View();            
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("Home", "Index");
            }

            var result = await _userManager.ResetPasswordAsync(user,model.Token,model.Password);

            if (result.Succeeded)
            {
                RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            
            return View();
        }
    }
}