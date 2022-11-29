using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Context;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly RoleManager<AppRole> _rolemanager;
        private readonly UdemyContext _context;

        public UserController(UserManager<AppUser> usermanager, RoleManager<AppRole> rolemanager, UdemyContext context = null)
        {
            _usermanager = usermanager;
            _rolemanager = rolemanager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // var query = _usermanager.Users;
            // var users = _context.Users.Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId,(user,userRole) => new{
            //     user,
            //     userRole
          // }).Join(_context.Roles,two => two.userRole.RoleId,role => role.Id,(two,role) => new {two.user,two.userRole,role}).Where(x=>x.role.Name != "Admin").Select(x=> new AppUser
            // {
            //     Id=x.user.Id,
            //     AccessFailedCount =x.user.AccessFailedCount,
            //     ConcurrencyStamp = x.user.ConcurrencyStamp,
            //     Email = x.user.Email,
            //     EmailConfirmed = x.user.EmailConfirmed,
            //     Gender = x.user.Gender,
            //     ImagePath = x.user.ImagePath,
            //     LockoutEnabled = x.user.LockoutEnabled,
            //     LockoutEnd = x.user.LockoutEnd,
            //     NormalizedEmail = x.user.NormalizedEmail,
            //     NormalizedUserName = x.user.NormalizedUserName,
            //     PasswordHash = x.user.PasswordHash,
            //     PhoneNumber = x.user.PhoneNumber,
            //     UserName = x.user.UserName
            // }).ToList();
            // // var userlist = await _usermanager.GetUsersInRoleAsync("Member");
            // return View(User);
            List<AppUser> filteredUser =new();
            var users = _usermanager.Users.ToList();

            foreach (var user in users)
            {
                var roles = await _usermanager.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                {
                    filteredUser.Add(user);
                }  
            }
            return View(filteredUser);
        }
       [HttpGet]
        public IActionResult Create()
        {
            return View(new UserAdminCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserAdminCreateModel model)
        {
           if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                 Email = model.Email,
                   UserName = model.Username,
                    Gender = model.Gender
               };
                var result = await _usermanager.CreateAsync(user,model.Username+"123");
                if (result.Succeeded)
                { 
                    var memberRole = await _rolemanager.FindByNameAsync("Member");
                    if (memberRole != null)
                    {
                        await _rolemanager.CreateAsync(new(){
                            Name="Member",
                            CreatedTime = DateTime.Now
                        });
                    }
                    await _usermanager.AddToRoleAsync(user,"Member");
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }

            }
            return View(model);
        }
        public async Task<IActionResult> AssignRole(int id)
        {
            var user = _usermanager.Users.SingleOrDefault(x=>x.Id == id);
            var userRoles =await  _usermanager.GetRolesAsync(user);
            var roles = _rolemanager.Roles.ToList();
            RoleAssignSendModel model = new();
            List<RoleAssignListModel> list = new();
            foreach (var role in roles)
            {
                list.Add(new(){
                    Name=role.Name,
                    RoleId =role.Id,
                    Exits = userRoles.Contains(role.Name)
                });
            }
            model.Roles = list;
            model.UserId = id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleAssignSendModel model)
        {
            var user = _usermanager.Users.SingleOrDefault(x=>x.Id == model.UserId);
            var userRole = await _usermanager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                        if (role.Exits)
                            if (!userRole.Contains(role.Name))
                                await _usermanager.AddToRoleAsync(user,role.Name);
                        else
                            if (userRole.Contains(role.Name))
                                await _usermanager.RemoveFromRoleAsync(user,role.Name);
                            
                        
            }
            return RedirectToAction("Index");
        }
    }
}