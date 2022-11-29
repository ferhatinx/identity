using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace TagHelpers
{
    [HtmlTargetElement("getuserinfo")]
    public class GetUserInfo : TagHelper
    {
        public int UserId { get; set; }
        private readonly UserManager<AppUser> _usermanager;

        public GetUserInfo(UserManager<AppUser> usermanager)
        {
            _usermanager = usermanager;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var html = "";
            var user = await _usermanager.Users.SingleOrDefaultAsync(x=>x.Id == UserId);
            var roles = await _usermanager.GetRolesAsync(user);

            foreach (var item in roles)
            {
                html+= item + " ";
            }
            output.Content.SetHtmlContent(html);
        }
    }
}