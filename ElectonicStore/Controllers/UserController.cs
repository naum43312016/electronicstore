using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectonicStore.Data;
using ElectonicStore.Models;
using ElectonicStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectonicStore.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = SD.ManagerUser)]
        public IActionResult Index()
        {
            List<ApplicationUser> applicationUsers = _db.ApplicationUser.ToList();
            return View(applicationUsers);
        }
        public IActionResult Register()
        {
            return View(new ApplicationUser());
        }

        public IActionResult SignIn()
        {
            return View(new ApplicationUser());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email,
                           model.Password, true, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                  return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //await _userManager.AddToRoleAsync(user, SD.ManagerUser);
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return Content(error.Description);
                    }
                }
            }
            return View(model);
        }

        
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}