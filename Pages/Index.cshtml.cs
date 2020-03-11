using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClaimsDemo.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClaimsDemo.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public string userid { get; set; }
        [BindProperty]
        public string username { get; set; }
        [BindProperty]
        public string usertoken { get; set; }

      
        public async Task<IActionResult> OnGet()
        {

            var Comm = new BaseController();
            var userIdentity = (ClaimsIdentity)User.Identity;
            var UserData = Comm.ClaimResult(userIdentity, "");
            string userid = "";
            if (UserData.Count() > 0)
            {
                userid = UserData.Where(c => c.Type == ClaimTypes.GroupSid).SingleOrDefault().Value;
                username = UserData.Where(c => c.Type == ClaimTypes.Name).SingleOrDefault().Value;
                usertoken = UserData.Where(c => c.Type == ClaimTypes.GroupSid).SingleOrDefault().Value;
            }

            return Page();
        }
    }
}
