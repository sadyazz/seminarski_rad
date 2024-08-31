using eReservation.Data;
using eReservation.Models;
using eReservation.Helpers.Auth;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<User> Edit([FromQuery]int id, [FromBody] User updatedUser)
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

            existingUser.Name = updatedUser.Name;
            existingUser.Surname = updatedUser.Surname;
            existingUser.Email = updatedUser.Email;
            existingUser.Phone = updatedUser.Phone;
            existingUser.DateOfRegistraion = updatedUser.DateOfRegistraion;
            existingUser.DateBirth = updatedUser.DateBirth;

            

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
