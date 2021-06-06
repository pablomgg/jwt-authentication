using API.Filters;
using Infrastructure.Identity.Core;
using Infrastructure.Identity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Configurations
{
    [Authorize] 
    [ExceptionActionFilter]
    [EnableCors("_myAllowSpecificOriginsPolicy")]
    public class WebApiBase : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IAuthentication Authentication;
        protected UserModel User;

        public WebApiBase(IHttpContextAccessor httpContextAccessor, IAuthentication authentication)
        {
            _httpContextAccessor = httpContextAccessor;
            Authentication = authentication;
            GetUserInAccessToken();
        }

        protected void GetUserInAccessToken()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            User = Authentication.DecodeToken(accessToken).GetAwaiter().GetResult();
        }
    }
}