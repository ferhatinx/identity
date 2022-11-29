using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _rolemanager;


        public HomeController(UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager, RoleManager<AppRole> rolemanager)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
            _rolemanager = rolemanager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {

            if (ModelState.IsValid)
            {
                AppUser user = new()
                {

                    Email = model.Email,
                    UserName = model.Username,
                    Gender = model.Gender

                };
                await _rolemanager.CreateAsync(new()
                {
                    Name = "Member",
                    CreatedTime = DateTime.Now
                });
                var identityResult = await _usermanager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {

                    await _usermanager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Index");

                }
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }
        public IActionResult SignIn(string returnUrl)
        {

            return View(new UserSignInModel() { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _usermanager.FindByNameAsync(model.Username);
                var signresult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if (signresult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    var username = await _usermanager.FindByNameAsync(model.Username);
                    var userrole = await _usermanager.GetRolesAsync(username);
                    if (userrole.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel");
                    }
                    else
                    {
                        return RedirectToAction("Panel");
                    }
                }
                else if (signresult.IsLockedOut)
                {
                    var lockend = await _usermanager.GetLockoutEndDateAsync(user); //Kilitlenme zamanını alır
                      
                    ModelState.AddModelError("",$"Hesabınız { (lockend.Value.UtcDateTime-DateTime.UtcNow).Minutes} süre ile kilitlendi");
                }
                else
                {
                    string message = String.Empty;
                    var usernamekilit = await _usermanager.FindByNameAsync(model.Username);
                    if (usernamekilit != null)
                    {
                        var failedentrie = await _usermanager.GetAccessFailedCountAsync(usernamekilit);
                        message = $"{(_usermanager.Options.Lockout.MaxFailedAccessAttempts - failedentrie)} kez daha girerseniz hesap kilitlenecek";
                    }
                    else
                    {
                        message = "Kullanıcı yada şifre hatalı";
                    }
                    ModelState.AddModelError("",message);

                }




            }
            return View(model);
        }
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var Username = User.Identity.Name;
            var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }
        [Authorize(Roles = "Member")]
        public IActionResult Panel()
        {
            return View();
        }
        [Authorize(Roles = "Member")]
        public IActionResult MemberPage()
        {
            return View();
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
}