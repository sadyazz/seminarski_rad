using eReservation.Data;
using eReservation.Models;
using eReservation.Helpers.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eReservation.DTO;
using eReservation.Services;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly DataContext _db;
        private readonly AuthService _authService;
        private readonly FirebaseService _firebaseService;

        public UserController(DataContext db, AuthService authService, FirebaseService firebaseService)
        {
            _db = db;
            _authService = authService;
            _firebaseService = firebaseService;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            var users = _db.User.ToList();
            if (users.Any())
            {
                return Ok(users);
            }
            return NoContent();
        }

        [HttpGet]
        [Route("/GetUserById")]
        public ActionResult<User> GetById([FromQuery] int id)
        {
            var user = _db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> Add([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            _db.User.Add(user);
            _db.SaveChanges();
            return Ok(user);
        }

        [HttpPut]
        [Route("/EditUser")]
        public ActionResult<User> Edit([FromQuery] int id, [FromBody] UserUpdateDto updatedUser)
        {
            if (!_authService.JelLogiran())
            {
                return Unauthorized("Korisnik nije prijavljen.");
            }

            var existingUser = _db.User.FirstOrDefault(u => u.ID == id);
            if (existingUser == null)
            {
                return NotFound("Korisnik nije pronađen.");
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(updatedUser.Name))
                existingUser.Name = updatedUser.Name;

            if (!string.IsNullOrEmpty(updatedUser.Surname))
                existingUser.Surname = updatedUser.Surname;

            if (!string.IsNullOrEmpty(updatedUser.Email))
                existingUser.Email = updatedUser.Email;

            if (!string.IsNullOrEmpty(updatedUser.Phone))
                existingUser.Phone = updatedUser.Phone;

            if (updatedUser.DateBirth != default(DateTime))
                existingUser.DateBirth = updatedUser.DateBirth;

            //if (!string.IsNullOrEmpty(updatedUser.ProfileImage))
            //    existingUser.ProfileImage = updatedUser.ProfileImage;

            if (!string.IsNullOrEmpty(updatedUser.Username))
                existingUser.Username = updatedUser.Username;

            if (!string.IsNullOrEmpty(updatedUser.NewPassword))
            {
                existingUser.Password = updatedUser.NewPassword;
            }

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Greška prilikom ažuriranja korisnika: " + ex.Message);
            }

            return Ok(existingUser);
        }

        [HttpPost]
        [Route("/UploadProfileImage")]
        public async Task<ActionResult> UploadProfileImage([FromQuery] int id, [FromForm] IFormFile image)
        {
            if (!_authService.JelLogiran())
            {
                return Unauthorized("Korisnik nije prijavljen.");
            }

            var user = await _db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound("Korisnik nije pronađen.");
            }

            if (image == null || image.Length == 0)
            {
                return BadRequest("Slika nije validna.");
            }

            var uploadUrl = await _firebaseService.UploadImageAsync(image);
            user.ProfileImage = uploadUrl; // Save the download URL in the user's profile
            await _db.SaveChangesAsync();

            return Ok(new { message = "Slika uspješno uploadovana.", url = uploadUrl });
        }



        [HttpGet]
        [Route("/GetProfileImage")]
        public ActionResult<string> GetProfileImage([FromQuery] int id)
        {
            var user = _db.User.FirstOrDefault(u => u.ID == id);
            if (user == null || string.IsNullOrEmpty(user.ProfileImage))
            {
                return NotFound("Korisnik nema profilnu sliku.");
            }

            // Return the stored URL for the profile image
            return Ok(user.ProfileImage);
        }


        //[HttpDelete("{id}")]
        //public ActionResult Delete(int id)
        //{
        //    var userToDelete = _db.User.FirstOrDefault(u => u.ID == id);

        //    if (userToDelete == null)
        //    {
        //        return NotFound();
        //    }

        //    _db.User.Remove(userToDelete);
        //    _db.SaveChanges();
        //    return Ok("User deleted.");
        //}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Pronađite korisnika
            var userToDelete = await _db.User.FirstOrDefaultAsync(u => u.ID == id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            var tokensToDelete = await _db.AutentifikacijaToken
                                           .Where(token => token.KorisnickiNalogId == id)
                                           .ToListAsync();

            if (tokensToDelete.Any())
            {
                _db.AutentifikacijaToken.RemoveRange(tokensToDelete);
            }

            _db.User.Remove(userToDelete);
            await _db.SaveChangesAsync();

            return Ok("User deleted.");
        }

    }
}
