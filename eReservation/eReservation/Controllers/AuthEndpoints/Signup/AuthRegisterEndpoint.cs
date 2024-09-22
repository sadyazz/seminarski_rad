using eReservation.Controllers.AuthEndpoints.Login;
using eReservation.Data;
using eReservation.Helpers.Auth;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eReservation.Controllers.AuthEndpoints.Signup
{
    [Route("Autentifikacija")]
    public class AuthRegisterEndpoint : BaseEndpoint<AuthRegisterRequest, IActionResult>
    {
        private readonly DataContext _applicationDbContext;
        private readonly EmailSenderService _emailSenderService;

        public AuthRegisterEndpoint(DataContext applicationDbContext, EmailSenderService emailSenderService)
        {
            _applicationDbContext = applicationDbContext;
            _emailSenderService = emailSenderService;
        }

        [HttpPost("register")]
        public override async Task<IActionResult> Obradi([FromBody] AuthRegisterRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _applicationDbContext.User
                .FirstOrDefaultAsync(k => k.Username == request.Username || k.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                return BadRequest(new { message = "Username or email is already in use." });
            }


            var newUser = new User
            {
                Username = request.Username,
                Password = request.Password,
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Phone = request.Phone,
                DateOfRegistraion = DateTime.Now,
                DateBirth = request.DateOfBirth,
                Is2FActive = true
            };

            _applicationDbContext.User.Add(newUser);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var welcomeEmailContent = $"Hello {newUser.Name},\n\n" +
                             "Welcome to our platform! We are thrilled to have you.\n" +
                             "Best regards,\neReservation Team";

            _emailSenderService.Posalji(newUser.Email, "Welcome to Our Platform", welcomeEmailContent, false);


            return Ok(new { message = "Registration successful!", user = newUser });
        }
    }
}
