using Azure.Core;
using Azure;
using eReservation.Helpers;
using Microsoft.AspNetCore.Mvc;
using eReservation.Data;
using eReservation.Models;

namespace eReservation.Controllers.AuthEndpoints.Logout
{

    [Route("Autentifikacija")]
    public class AuthLogoutEndpoint : BaseEndpoint<NoRequest, NoResponse>
    {
        private readonly DataContext _applicationDbContext;
        private readonly AuthService _authService;

        public AuthLogoutEndpoint(DataContext applicationDbContext, AuthService authService)
        {
            _applicationDbContext = applicationDbContext;
            _authService = authService;
        }

        [HttpPost("logout")]
        public override async Task<NoResponse> Obradi([FromBody] NoRequest request, CancellationToken cancellationToken)
        {
            AutentifikacijaToken? autentifikacijaToken = _authService.GetAuthInfo().autentifikacijaToken;

            if (autentifikacijaToken == null)
                return new NoResponse();

            _applicationDbContext.Remove(autentifikacijaToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return new NoResponse();
        }
    }
}
