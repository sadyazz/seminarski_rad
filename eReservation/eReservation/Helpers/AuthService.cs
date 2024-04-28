using eReservation.Data;

namespace eReservation.Helpers
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
        //samo zapoceto
        public bool JelLogiran()
        {
            string authToken = _httpContextAccessor.HttpContext.Request.Headers["my-auth-token"];

            return false;
        }
    }
}
