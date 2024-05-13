using Azure.Core;
using eReservation.Data;
using eReservation.Helpers;
using eReservation.Helpers.Auth;
using eReservation.Models;

using Microsoft.AspNetCore.Mvc;

namespace eReservation.Controllers.AuthEndpoints.Get
{
    [Route("Autentifikacija")]
    public class AuthGetEndpoint : BaseEndpoint<NoRequest, MyAuthInfo>
    {
        private readonly DataContext _DbContext;
        private readonly AuthService _authService;
        public AuthGetEndpoint(DataContext applicationDbContext, AuthService authService)
        {
            _DbContext = applicationDbContext;
            _authService = authService;
        }

        [HttpPost("get")]
        public override async Task<MyAuthInfo> Obradi([FromBody] NoRequest request, CancellationToken cancellationToken)
        {
            AutentifikacijaToken? autentifikacijaToken = _authService.GetAuthInfo().autentifikacijaToken;

            return new MyAuthInfo(autentifikacijaToken);
        }
    }
}
