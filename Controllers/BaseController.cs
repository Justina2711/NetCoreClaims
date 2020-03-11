using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ClaimsDemo.Controllers
{
    public class BaseController : Controller
    {

        public BaseController()
        {


        }



        public HttpContext InitHttpContext(HttpContext context)
        {
            return context;
        }


        #region claims

        public IEnumerable<Claim> ClaimResult(ClaimsIdentity User, string ClaimType)
        {
            try
            {
                var claims = User.Claims;
                return claims;
            }
            catch (Exception)
            {

                throw;
            }


        }



      

        #endregion


        
    }
}