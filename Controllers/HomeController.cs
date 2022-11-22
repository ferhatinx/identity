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
            _rolemanager =rolemanager;
        }
        public IActionResult Index()
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
                
                    Email=model.Email,
                    UserName=model.Username,
                    Gender=model.Gender
                
                };
              
                var identityResult = await _usermanager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                await _rolemanager.CreateAsync(new(){
                    Name="Admin",
                    CreatedTime=DateTime.Now
                });
                    await _usermanager.AddToRoleAsync(user,"Admin");
                    return RedirectToAction("Index");
                    
                }
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                
            }
            return View(model);
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel model)
        {
            if (ModelState.IsValid)
            {
                var signresult = await _signInManager.PasswordSignInAsync(model.Username,model.Password,false,false);
                if (signresult.Succeeded)
                {
                    
                }
            
            
            }
            return View(model);
        }
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var Username = User.Identity.Name;
            var role = User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Role);
            return View();
        }
       
    }
}