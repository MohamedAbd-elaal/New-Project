using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_FILE_VIEW_.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC_FILE_VIEW_.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        public SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,ILogger<AccountController> logger)
        {
                 this.userManager = userManager;
                 this.signInManager = signInManager;
            this.logger = logger;
        }
        //post for send to check email Exist or Not 
        //Get To get Email If Exist before click(Register)
        [AcceptVerbs("Get","Post")] //Allow get and post Http
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if(user==null)
            {
                return Json("true");
            }
            else
            {
                return Json($"This {Email} Is Aready In Use");
            }
        }
            [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,City=model.City };
                var result= await userManager.CreateAsync(user,model.Password);
                if(result.Succeeded)
                {
                    //new for confirmation
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //this how to write Url
                    //new{variable} this is anonymous object and u can look(internet) how use select to select variable in anonymous object
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                      new { userId = user.Id, token = token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, confirmationLink);
                    // for list user (to add new user by Register do not login it and still login with Exist user  
                    if (signInManager.IsSignedIn(User) && User.IsInRole("user"))
                    {
                        return RedirectToAction("ListUser", "Administration");
                    }
                    // add x
                    ViewBag.ErrorTitle = "Registration successful";
                    ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                            "email, by clicking on the confirmation link we have emailed you";
                    return View("~/Views/Home/Error.cshtml");
                    //(isPersistent=>create cookie that is(session or permenant cookie)
                    //session gone where u close browser
                    //permenant didnot go even after close browser 

                    //we add x instead of that (to go confirmationLink to confirm our email first )
                    //this will sighn in and we dont want that before check confirmation

                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("IEnumerable", "home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if(userId == null || token == null)
            {
                return View("~/Views/Home/IEnumerable.cshtml");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("IEnumerable", "home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async  Task<IActionResult> Login(string returnurl)
        {
            LoginViewModel model = new LoginViewModel()
            {
                ReturnUrl = returnurl,
                ExternalLogins =(await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
         
        public async Task<IActionResult> Login(LoginViewModel model,string ReturnUrl)
        {
            // get external login and this reference to confirmed email
            model.ExternalLogins=(await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
           // throw new Exception("hello"); to Exam Client Validation
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user!=null&& !user.EmailConfirmed &&(await userManager.CheckPasswordAsync(user,model.Password)))
                {
                    ModelState.AddModelError("", "Email Not Confirmed Yet");
                    return View(model);
                }
                // The last boolean parameter lockoutOnFailure indicates if the account
                // should be locked on failed logon attempt. On every failed logon
                // attempt AccessFailedCount column value in AspNetUsers table is
                // incremented by 1. When the AccessFailedCount reaches the configured
                // MaxFailedAccessAttempts which in our case is 5, the account will be
                // locked and LockoutEnd column is populated. After the account is
                // lockedout, even if we provide the correct username and password,
                // PasswordSignInAsync() method returns Lockedout result and the login
                // will not be allowed for the duration the account is locked.
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RemmberMe, true);
                if (result.Succeeded)
                {
                    //if we in a specify page then it willnot go to it before login
                    //when we login we almost go to IEnumerable page not the page we choose 
                    //this way get us to the choosen page
                    if(!string.IsNullOrEmpty(ReturnUrl))
                    {
                        //we can write Redirect without local but that can Attack
                        // see screen shot
                        return LocalRedirect(ReturnUrl); 
                    }
                    // anthor way
                    //if (!string.IsNullOrEmpty(ReturnUrl)&& Url.IsLocalUrl(ReturnUrl))
                    //{
                    //    //we can write Redirect without local but that can Attack
                    //    // see screen shot
                    //    return Redirect(ReturnUrl);
                    //}
                    //(isPersistent=>create cookie that is(session or permenant cookie)
                    //session gone where u close browser
                    //permenant didnot go even after close browser 

                    return RedirectToAction("IEnumerable", "home");
                }
                if(result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                
                ModelState.AddModelError(string.Empty, "Login InValid");
                
            }
            return View(model);
        }
        // if the user not Member Of Role So it will go to Access denied view
        // by default so we create that method
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        // for External Google
        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                new { ReturnUrl = returnUrl });
            var properties = signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl=null,string remoteError=null)
        {
            //Null coalescing operator
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = ( await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error From External Provider {remoteError}");
                return View("Login",loginViewModel);

            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if(info==null)
            {
                ModelState.AddModelError(string.Empty, "Error Loading External Login Information..");
                return View("Login", loginViewModel);
            }
            // Get the email claim from external login provider (Google, Facebook etc)
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = null;
            if(email!=null)
            {
                // Find the user
                user = await  userManager.FindByEmailAsync(email);
                // If email is not confirmed, display login view with validation error
                if (user!=null&&!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email Not Confirmed Yet");
                    return View("Login", loginViewModel);
                }

            }
           
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent : false, bypassTwoFactor : true);
            // if signInResult.Succeeded mean we have An Existing user
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            else
            {
                
                if (email != null)
                {
                    
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        await userManager.CreateAsync(user);

                        //new for confirmation
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        //this how to write Url
                        //new{variable} this is anonymous object and u can look(internet) how use select to select variable in anonymous object
                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                          new { userId = user.Id, token = token }, Request.Scheme);

                        logger.Log(LogLevel.Warning, confirmationLink);
                        ViewBag.ErrorTitle = "Registration successful";
                        ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                                "email, by clicking on the confirmation link we have emailed you";
                        return View("~/Views/Home/Error.cshtml");
                    }
                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            
                    ViewBag.ErrorTitle = $"Email claim not received from :{info.LoginProvider}";
                    ViewBag.ErrorMessage = "Please contact support on mohamedadbellal60@gmail.com";
                    return View("Error");

            }
        }

        //for forget password
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                    // Log the password reset link
                    logger.Log(LogLevel.Warning, passwordResetLink);

                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        // Upon successful password reset and if the account is lockedout, set
                        // the account lockout end date to current UTC date time, so the user
                        // can login with the new password
                        if (await userManager.IsLockedOutAsync(user))
                        {
                            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        return View("ResetPasswordConfirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if(user==null)
                {
                    return RedirectToAction("Login");
                }
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if(!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                await signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }
            return View(model);
            
        }

    }
}
