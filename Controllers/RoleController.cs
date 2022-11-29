using System;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _rolemanager;

        public RoleController(RoleManager<AppRole> rolemanager)
        {
            _rolemanager = rolemanager;
        }
        public IActionResult Index()
        {
            var list = _rolemanager.Roles.ToList();
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new RoleCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateModel model)
        {   
            if (ModelState.IsValid)
            {
                var result = await _rolemanager.CreateAsync(new AppRole()
                {
                    CreatedTime = DateTime.Now,
                    Name = model.Name

                });
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
            }

            return View();
        }
    }
}