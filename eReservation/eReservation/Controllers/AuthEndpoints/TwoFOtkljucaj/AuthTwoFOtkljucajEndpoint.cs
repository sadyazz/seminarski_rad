using Azure.Core;
using eReservation.Data;
using eReservation.Helpers;
using eReservation.Helpers.Auth;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eReservation.Controllers.AuthEndpoints.Login
{

    [Route("Autentifikacija")]
    public class AuthTwoFOtkljucajEndpoint : BaseEndpoint<AuthTwoFOtkljucajRequest, NoResponse>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;

        public AuthTwoFOtkljucajEndpoint(DataContext applicationDbContext, AuthService authService)
        {
            _dataContext = applicationDbContext;
            _authService = authService;
        }

        [HttpPost("2f-otkljucaj")]
        public override async Task<NoResponse> Obradi([FromBody] AuthTwoFOtkljucajRequest request, CancellationToken cancellationToken)
        {
            if (!_authService.GetAuthInfo().isLogiran)
            {
                throw new Exception("Niste logirani.");
            }
            var token = _authService.GetAuthInfo().autentifikacijaToken;

            if (token is null)
                throw new ArgumentNullException(nameof(token));

            if (request.Kljuc == token.TwoFKey)
            {
                token.Is2FOtkljucano = true;
                await _dataContext.SaveChangesAsync(cancellationToken);
            }

            return new NoResponse();
        }


    }