using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Context
{
    public class UdemyContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public UdemyContext(DbContextOptions<UdemyContext> options) : base(options)
        {
            
        }
    }
}