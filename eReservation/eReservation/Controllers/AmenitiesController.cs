using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AmenitiesController : Controller
    {
        private readonly DataContext _db;

        public AmenitiesController(DataContext db)
        {
            _db=db;
        }

        [HttpGet]
        public ActionResult<List<Amenities>> GetAll()
        {
            return Ok(_db.Amenities.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Amenities> GetAmenities(int id)
        {
            var amenities = _db.Amenities.Find(id);

            if (amenities == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(amenities);
            }
        }

        [HttpPost]
        public ActionResult<Amenities> Add([FromBody] Amenities amenity)
        {
            if (amenity == null)
            {
                return BadRequest();
            }
            _db.Amenities.Add(amenity);
            _db.SaveChanges();
            return Ok(amenity);
        }

        [HttpPut("{id}")]
        public ActionResult<Amenities> Edit(int id, [FromBody] Amenities updatedAmenity)
        {
            var existingAmenity = _db.Amenities.FirstOrDefault(a => a.ID == id);
            if (existingAmenity == null)
            {
                return NotFound();
            }
            existingAmenity.Name = updatedAmenity.Name;
            existingAmenity.Description = updatedAmenity.Description;

            _db.SaveChanges();

            return Ok(existingAmenity);
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var amenityToDelete = _db.Amenities.FirstOrDefault(a => a.ID == id);

            if (amenityToDelete == null)
            {
                return NotFound();
            }

            _db.Amenities.Remove(amenityToDelete);
            _db.SaveChanges();
            return Ok("Amenity deleted.");
        }

    }
}
