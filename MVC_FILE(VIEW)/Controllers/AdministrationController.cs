using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC_FILE_VIEW_.Model;
using MVC_FILE_VIEW_.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC_FILE_VIEW_.Controllers
{
    //[Authorize(Roles = "user,Admin1")]
    //[Authorize(Roles = "user")]
    //[Authorize(Roles = "Admin1")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager
                                        , UserManager<ApplicationUser> userManager
                                        , ILogger<AdministrationController> logger
                                        )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreatingRloeViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole { Name = model.RoleName };
                IdentityResult Result = await roleManager.CreateAsync(identityRole);
                if (Result.Succeeded)
                {
                    return RedirectToAction("ListRole", "Administration");
                }
                foreach (IdentityError Error in Result.Errors)
                {
                    ModelState.AddModelError("", Error.Description);
                }
            }
            return View(model);

        }
        [HttpGet]
        public IActionResult ListRole()
        {
            var role = roleManager.Roles;
            return View(role);
        }
        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);// check from database if this role with this id exist or not
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The ROle With Id {id} Not Found";
                return View("NotFound");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name

            };
            foreach (var user in await userManager.Users.ToListAsync())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
       // [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);//check from EditRoleViewModel if this role with this id exist or not
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The ROle With Id {model.Id} Not Found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The ROle With Id {roleId} Not Found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in await userManager.Users.ToListAsync())
            {
                var UserRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    UserRoleViewModel.IsSelected = true;
                }
                else
                {
                    UserRoleViewModel.IsSelected = false;
                }
                model.Add(UserRoleViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The ROle With Id {roleId} Not Found";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!(model[i].IsSelected) && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    // means there is more User selected so reprocces and get it 
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    return RedirectToAction("EditRole", new { id = roleId });

                }
            }
            return RedirectToAction("EditRole", new { id = roleId });
        }
        [HttpGet]
        public IActionResult ListUser()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        
        public async Task<IActionResult> EditUser(string id)
        {

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The ROle With Id {id} Not Found";
                return View("NotFound");
            }
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                Roles = userRoles,
                Claims = userClaims.Select(c => c.Type+": "+c.Value).ToList()
            };
            return View(model);


        }
        [HttpPost]
        
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {

            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The ROle With Id {model.Id} Not Found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUser");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The ROle With Id {id} Not Found";
                return View("NotFound");
            }
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ListUser");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUser");
            }
        }
        [HttpPost]
        //To Delete user that we login by him must have Claim (Delete Role)
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The ROle With Id {id} Not Found";
                return View("NotFound");
            }

            try
            {
                //To Test Diffrent Between Exception And specify the Error(DbUpdateException)
                //throw new Exception("New");
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRole");
                }
            }
            // we use DbUpdateException to specify the Error (to be DataBase Error (in Deleting Role and this role Having A User))
            catch (DbUpdateException ex)
            {
                logger.LogError($"Error Deleting Role: {ex}");
                ViewBag.ErrorTitle = $"{role.Name} Role Is in use";
                ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted " +
                   " There Is Role Having A User Please Delete User First";
                return View("~/Views/Home/Error.cshtml");
            }
        }
        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy1")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The ROle With Id {userId} Not Found";
                return View("NotFound");
            }
            var model = new List<ManageUserRoles>();
            foreach (var role in await roleManager.Roles.ToListAsync())
            {
                var ManageUserRoles = new ManageUserRoles()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    ManageUserRoles.IsSelected = true;
                }
                else
                {
                    ManageUserRoles.IsSelected = false;
                }

                model.Add(ManageUserRoles);

            }
            return View(model);
        }
        [HttpPost]
       // [Authorize(Policy = "EditRolePolicy1")]
        public async Task<IActionResult> ManageUserRoles(List<ManageUserRoles> model, string userId)
        {

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The ROle With Id {userId} Not Found";
                return View("NotFound");
            }
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "cannot Remove user Existing Roles");
                return View(model);
            }
            result = await userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "cannot Add Selected Roles To User");
                return View(model);
            }
            return RedirectToAction("EditUser", new { id = userId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            // Loop through each claim we have in our application
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value=="true"))
                {
                    userClaim.IsSelected = true;
                }

                model.Cliams.Add(userClaim);
            }

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }
            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }
            // this is for only claims type not claim value
            //result = await userManager.AddClaimsAsync(user, model.Cliams.Where(c => c.IsSelected)
            //    .Select(c => new Claim(c.ClaimType, c.ClaimType)));
            result = await userManager.AddClaimsAsync(user, model.Cliams
                .Select(c => new Claim(c.ClaimType, c.IsSelected ? "true":"false")));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }
            return RedirectToAction("EditUser", new { Id = model.UserId });

        }
    }
}
