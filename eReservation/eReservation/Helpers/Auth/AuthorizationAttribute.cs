using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using eReservation.Services;

namespace eReservation.Helpers.Auth
{
    public class AuthorizationAttribute : TypeFilterAttribute
    {
        public AuthorizationAttribute() : base(typeof(MyAuthorizationAsyncActionFilter))
        {
        }
    }
    public class MyAuthorizationAsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authService = context.HttpContext.RequestServices.GetService<AuthService>()!;
            var actionLogService = context.HttpContext.RequestServices.GetService<ActionLogService>()!;

            if (!authService.JelLogiran())
            {
                context.Result = new UnauthorizedObjectResult("niste logirani na sistem");
                return;
            }

            MyAuthInfo myAuthInfo = authService.GetAuthInfo();

            if (myAuthInfo.korisnickiNalog.Is2FActive && !myAuthInfo.autentifikacijaToken.Is2FOtkljucano)
            {
                context.Result = new UnauthorizedObjectResult("niste otkljucali 2f");
                return;
            }

            await next();
            await actionLogService.Create(context.HttpContext);
        }
    }
}