using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClaimsDemo
{
    public class Helper
    {
        public static string GetUserName(IHttpContextAccessor httpContextAccessor)
        {
            string userName = string.Empty;
            if (httpContextAccessor != null &&
               httpContextAccessor.HttpContext != null &&
               httpContextAccessor.HttpContext.User != null &&
               httpContextAccessor.HttpContext.User.Identity != null)
            {
                userName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                //string[] usernames = userName.Split('\\');
                //userName = usernames[1].ToUpper();
            }
            return userName;
        }
    }
}
