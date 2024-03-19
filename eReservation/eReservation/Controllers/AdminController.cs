using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly DataContext _db;

        public AdminController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<Admin>> GetAll()
        {
            var admins = _db.Admin.ToList();
            if (admins.Any())
            {
                return Ok(admins);
            }
            return NoContent();
        }

        [HttpPost]
        public ActionResult<Admin> Add([FromBody] Admin admin)
        {
            if (admin == null)
            {
                return BadRequest();
            }
            _db.Admin.Add(admin);
            _db.SaveChanges();
            return Ok(admin);
        }

        [HttpPut("{id}")]
        public ActionResult<Admin> Edit(int id, [FromBody] Admin updatedAdmin)
        {
            var existingAdmin = _db.Admin.FirstOrDefault(a => a.ID == id);
            if (existingAdmin == null)
            {
                return NotFound();
            }
            existingAdmin.Username = updatedAdmin.Username;
            existingAdmin.Password = updatedAdmin.Password;

            _db.SaveChanges();

            return Ok(existingAdmin);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var adminToDelete = _db.Admin.FirstOrDefault(a => a.ID == id);

            if (adminToDelete == null)
            {
                return NotFound();
            }

            _db.Admin.Remove(adminToDelete);
            _db.SaveChanges();
            return Ok("Admin deleted.");
        }
    }
}
