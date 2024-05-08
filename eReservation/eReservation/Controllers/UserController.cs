using eReservation.Data;
using eReservation.Helpers;
using eReservation.Models;
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

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
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

        [HttpPut("{id}")]
        public ActionResult<User> Edit(int id, [FromBody] User updatedUser)
        {
            if (_authService.JelLogiran())
            {
                throw new Exception("nije logiran");
            }

            var existingUser = _db.User.FirstOrDefault(u => u.ID == id);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.Name = updatedUser.Name;
            existingUser.Surname = updatedUser.Surname;
            existingUser.Email = updatedUser.Email;
            existingUser.Password = updatedUser.Password;
            existingUser.Phone = updatedUser.Phone;
            existingUser.DateOfRegistraion = updatedUser.DateOfRegistraion;
            //existingUser.UserType = updatedUser.UserType;
            existingUser.DateBirth = updatedUser.DateBirth;

            _db.SaveChanges();

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
