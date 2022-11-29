using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Context;
using CustomDescriber;
using Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace identity
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AppUser,AppRole>(opt=>{
                opt.Password.RequireDigit =false;
                opt.Password.RequiredLength = 1;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric =false;
                opt.SignIn.RequireConfirmedPhoneNumber =false;
                opt.Lockout.MaxFailedAccessAttempts = 3; //3 hatalı girişte kilitlenecek

            })/*.AddErrorDescriber<CustomErrorDescriber>()*/.AddEntityFrameworkStores<UdemyContext>();
            services.AddDbContext<UdemyContext>(opt =>{
                opt.UseSqlServer("server=DESKTOP-3KU2KP7; database=DBidentity; integrated security=true;");
            
            });
            services.ConfigureApplicationCookie(opt =>{
                opt.Cookie.HttpOnly = true;
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.Cookie.Name = "UdemyCookie";
                opt.ExpireTimeSpan=TimeSpan.FromDays(25);
                opt.LoginPath = new PathString("/Home/SignIn");
                opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
            });
            services.AddControllersWithViews();
        
        }

      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles(new StaticFileOptions{
                FileProvider=new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),
                RequestPath="/node_modules"
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
