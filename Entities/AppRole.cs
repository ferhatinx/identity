using System;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class AppRole : IdentityRole<int>
    {
        public DateTime CreatedTime { get; set; }
    }
}