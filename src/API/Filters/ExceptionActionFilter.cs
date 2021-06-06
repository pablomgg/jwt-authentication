using System.Net;
using Infrastructure.Application.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";

            if (context.Exception is BusinessException)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(context.Exception.Message);
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new JsonResult(context.Exception.Message);
            }
        }
    }
}