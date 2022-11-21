using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser> _signinmanager;
        private readonly RoleManager<AppRole> _rolemanager;

        public HomeController(UserManager<AppUser> usermanager, SignInManager<AppUser> signinmanager)
        {
            _usermanager = usermanager;
            _signinmanager = signinmanager;
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
                    UserName=model.UserName,
                    Gender=model.Gender
                };
                var identityResult = await _usermanager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
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
                var signinresult =await _signinmanager.PasswordSignInAsync(model.UserName,model.Password,false,false);
                if (signinresult.Succeeded)
                {
                    await _rolemanager.CreateAsync(new AppRole{
                        Name ="Admin",
                        CreatedTime=System.DateTime.Now
                    });
                    //başarılı ise
                }
                else if(signinresult.IsLockedOut)
                {
                    //hesap kilitli
                }
                else if(signinresult.IsNotAllowed)
                {
                    //email ve phonenumber doğrulanmamış
                }
            }
            return View();
        }
        [Authorize]
        public IActionResult GetUserInfo()
        {
            return View();
        }
    }
}