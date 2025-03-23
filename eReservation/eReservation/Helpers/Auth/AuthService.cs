using eReservation.Data;
using eReservation.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace eReservation.Helpers.Auth
{
    public class AuthService
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool JelLogiran()
        {
            return GetAuthInfo().isLogiran;
        }

        public bool isAdmin()
        {
            return GetAuthInfo().korisnickiNalog?.isAdmin ?? false;
        }

        public bool isKorisnik()
        {
            return GetAuthInfo().korisnickiNalog?.isUser ?? false;
        }

        public MyAuthInfo GetAuthInfo()
        {
            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["my-auth-token"];

            AutentifikacijaToken? autentifikacijaToken = _dbContext.AutentifikacijaToken
                .Include(x => x.korisnickiNalog)
                .SingleOrDefault(x => x.vrijednost == authToken);

            return new MyAuthInfo(autentifikacijaToken);
        }
    }
    public class MyAuthInfo
    {
        public MyAuthInfo(AutentifikacijaToken? autentifikacijaToken)
        {
            this.autentifikacijaToken = autentifikacijaToken;
        }

        [JsonIgnore]
        public KorisnickiNalog? korisnickiNalog => autentifikacijaToken?.korisnickiNalog;
        public AutentifikacijaToken? autentifikacijaToken { get; set; }

        public bool isLogiran => korisnickiNalog != null;

        public bool IsAdmin => korisnickiNalog?.isAdmin ?? false;

    }
}
