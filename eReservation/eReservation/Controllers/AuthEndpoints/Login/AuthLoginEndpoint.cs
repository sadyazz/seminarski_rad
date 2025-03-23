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
    public class AuthLoginEndpoint : BaseEndpoint<AuthLoginRequest, MyAuthInfo>
    {
        private readonly DataContext _applicationDbContext;
        private readonly EmailSenderService _emailSenderService;

        public AuthLoginEndpoint(DataContext applicationDbContext, EmailSenderService emailSenderService)
        {
            _applicationDbContext = applicationDbContext;
            _emailSenderService = emailSenderService;

        }

        [HttpPost("login")]
        public override async Task<MyAuthInfo> Obradi([FromBody] AuthLoginRequest request, CancellationToken cancellationToken)
        {
            KorisnickiNalog? logiraniKorisnik = await _applicationDbContext.KorisnickiNalog
                .FirstOrDefaultAsync(k => k.Username == request.KorisnickoIme && k.Password == request.Lozinka, cancellationToken);

            if (logiraniKorisnik == null)
            {
                return new MyAuthInfo(null);
            }

            string? twoFKey = null;

            var user = logiraniKorisnik as User;
            if (user != null)
            {
                if (logiraniKorisnik.Is2FActive)
                {
                    twoFKey = TokenGenerator.Generate(4);
                    _emailSenderService.Posalji(user.Email, "2f", $"Vas 2f kljuc je {twoFKey}", false);
                }
            }

            string randomString = TokenGenerator.Generate(10);

            var noviToken = new AutentifikacijaToken()
            {
                ipAdresa = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                vrijednost = randomString,
                korisnickiNalog = logiraniKorisnik,
                vrijemeEvidentiranja = DateTime.Now,
                TwoFKey = twoFKey
            };

            _applicationDbContext.Add(noviToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var isAdmin = logiraniKorisnik.isAdmin;

            return new MyAuthInfo(noviToken);
        }


    }
}
