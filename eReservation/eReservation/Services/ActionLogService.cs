using eReservation.Data;
using eReservation.Helpers.Auth;
using eReservation.Models;
using Microsoft.AspNetCore.Http.Extensions;

namespace eReservation.Services
{
    public class ActionLogService
    {
        public async Task Create(HttpContext httpContext)
        {
            var authService = httpContext.RequestServices.GetService<AuthService>()!;
            var request = httpContext.Request;

            var queryString = request.Query;

            //if (queryString.Count == 0 && !request.HasFormContentType)
            //    return 0;

            //IHttpRequestFeature feature = request.HttpContext.Features.Get<IHttpRequestFeature>();
            string detalji = "";
            if (request.HasFormContentType)
            {
                foreach (string key in request.Form.Keys)
                {
                    detalji += " | " + key + "=" + request.Form[key];
                }
            }

            // convert stream to string
            StreamReader reader = new StreamReader(request.Body);
            string bodyText = await reader.ReadToEndAsync();

            var x = new LogKretanjePoSistemu
            {
                User = authService.GetAuthInfo().korisnickiNalog!,
                Time = DateTime.Now,
                QueryPath = request.GetEncodedPathAndQuery(),
                PostData = detalji + "" + bodyText,
                IpAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString(),
            };

            //if (exceptionMessage != null)
            //{
            //    x.isException = true;
            //    x.exceptionMessage = exceptionMessage.Error.Message + " |" + exceptionMessage.Error.InnerException;
            //}

            DataContext db = request.HttpContext.RequestServices.GetService<DataContext>();

            db.Add(x);
            await db.SaveChangesAsync();
        }
    }
}