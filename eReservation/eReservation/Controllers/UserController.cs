using eReservation.Data;
using eReservation.Models;
using eReservation.Helpers.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eReservation.DTO;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly DataContext _db;
        private readonly AuthService _authService;

        public UserController(DataContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
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

            var user = _db.User.FirstOrDefault(u => u.ID == id);
            if (user == null)
            {
                return NotFound("Korisnik nije pronađen.");
            }

            if (image == null || image.Length == 0)
            {
                return BadRequest("Slika nije validna.");
            }

            // Konvertuj sliku u base64 string
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var base64Image = Convert.ToBase64String(imageBytes);

                // Spremi base64 string u bazu
                user.ProfileImage = base64Image;
                _db.SaveChanges();
            }

            return Ok(new { message = "Slika uspješno uploadovana." });
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

            return Ok(user.ProfileImage);
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var userToDelete = _db.User.FirstOrDefault(u => u.ID == id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            _db.User.Remove(userToDelete);
            _db.SaveChanges();
            return Ok("User deleted.");
        }
    }
}
