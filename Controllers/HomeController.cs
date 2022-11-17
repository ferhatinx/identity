using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _usermanager;

        public HomeController(UserManager<AppUser> usermanager)
        {
            _usermanager = usermanager;
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
    }
}