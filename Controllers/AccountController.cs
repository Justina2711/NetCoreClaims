using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ClaimsDemo.Controllers
{
    public class AccountController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly RequestDelegate _requestDelegate;
        public AccountController(IHttpContextAccessor httpContextAccessor/* RequestDelegate requestDelegate*/)
        {
            _httpContextAccessor = httpContextAccessor;
            //_requestDelegate = requestDelegate;
        }


        [HttpGet]
        public IActionResult Login()
        {



            return View();


        }


        [HttpPost]
        public async Task<IActionResult> LoginWithEmail(string username, string Password)
        {
            var claims = new List<Claim>();
            try
            {
                const string Issuer = "https://www.femori.com/";

                claims.Add(new Claim(ClaimTypes.Name, username, ClaimValueTypes.String, Issuer));
                claims.Add(new Claim(ClaimTypes.Role, "femori", ClaimValueTypes.String, Issuer));
                claims.Add(new Claim(ClaimTypes.GroupSid, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJqdXN0aW5hQHBpa2F0ZWNrLmNvbSIsImp0aSI6ImEwM2YzNmU4LWEzN2ItNDY4MC05NmM1LTA1ZTFmYjA0NzcxMyIsImV4cCI6MTU4MzA0NjEzNywiaXNzIjoiaHR0cHM6Ly9mdXNpb25hY2Nlc3MuY29tIiwiYXVkIjoiaHR0cHM6Ly9mdXNpb25hY2Nlc3MuY29tIn0.xq8tReoxndf6NHW9OJejFaIlxwdhIYU_aIvQJl2ooNE", ClaimValueTypes.String, Issuer));
                claims.Add(new Claim(ClaimTypes.Sid, "33434354-17a4-425f-d848-08d7402b3f24", ClaimValueTypes.String, Issuer));
                claims.Add(new Claim(ClaimTypes.Actor, username, ClaimValueTypes.String, Issuer));
                claims.Add(new Claim(ClaimTypes.Locality, Password, ClaimValueTypes.String, Issuer));//Location

                var userIdentity = new ClaimsIdentity("SecureClaims");
                userIdentity.AddClaims(claims);
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(1),
                        IsPersistent = true
                    });

                return Json(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult DisplayClaims()
        {
            //ViewBag.Name =  Helper.GetUserName(_httpContextAccessor);
            ImgHandler imgHandler = new ImgHandler();
            imgHandler.ProcessRequest(_httpContextAccessor);
            return View();
        }


       


    }
}