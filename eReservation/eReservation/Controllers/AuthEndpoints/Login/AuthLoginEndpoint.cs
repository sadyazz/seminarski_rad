using Azure.Core;
using eReservation.Data;
using eReservation.Helpers;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eReservation.Controllers.AuthEndpoints.Login
{

    [Route("Autentifikacija")]
    public class AuthLoginEndpoint : BaseEndpoint<AuthLoginRequest, MyAuthInfo>
    {
        private readonly DataContext _applicationDbContext;

        public AuthLoginEndpoint(DataContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpPost("login")]
        public override async Task<MyAuthInfo> Obradi([FromBody] AuthLoginRequest request, CancellationToken cancellationToken)
        {
            //1- provjera logina
            KorisnickiNalog? logiraniKorisnik = await _applicationDbContext.KorisnickiNalog
                .FirstOrDefaultAsync(k =>
                    k.Username == request.KorisnickoIme && k.Password == request.Lozinka, cancellationToken);

            if (logiraniKorisnik == null)
            {
                //pogresan username i password
                return new MyAuthInfo(null);
            }

            //2- generisati random string
            string randomString = TokenGenerator.Generate(10);

            //3- dodati novi zapis u tabelu AutentifikacijaToken za logiraniKorisnikId i randomString
            var noviToken = new AutentifikacijaToken()
            {
                ipAdresa = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                vrijednost = randomString,
                korisnickiNalog = logiraniKorisnik,
                vrijemeEvidentiranja = DateTime.Now
            };

            _applicationDbContext.Add(noviToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            //4- vratiti token string
            return new MyAuthInfo(noviToken);
        }

    }
}
