using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PropertiesController : Controller
    {
        private readonly DataContext _db;

        public PropertiesController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<Properties>> GetAll()
        {
            var properties = _db.Properties
       .Include(p => p.City).ThenInclude(c => c.Country)
       .Include(p => p.PropertyType)
       .ToList();

            if (properties.Any())
            {
                return Ok(properties);
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<Properties> Get(int id)
        {
            var property = _db.Properties.Find(id);
            if (property == null)
            {
                return NotFound();
            }
            return Ok(property);
        }

        [HttpPost]
        public ActionResult<Properties> Add([FromBody] Properties property)
        {
            if (property == null)
            {
                return BadRequest();
            }
            _db.Properties.Add(property);
            _db.SaveChanges();
            return Ok(property);
        }

        [HttpPut("{id}")]
        public ActionResult<Properties> Edit(int id, [FromBody] Properties updatedProperty)
        {
            var existingProperty = _db.Properties.FirstOrDefault(p => p.ID == id);
            if (existingProperty == null)
            {
                return NotFound();
            }
            existingProperty.Name = updatedProperty.Name;
            existingProperty.Adress = updatedProperty.Adress;
            existingProperty.NumberOfRooms = updatedProperty.NumberOfRooms;
            existingProperty.NumberOfBathrooms = updatedProperty.NumberOfBathrooms;
            existingProperty.PricePerNight = updatedProperty.PricePerNight;
            existingProperty.CityID = updatedProperty.CityID;
            existingProperty.PropertyTypeID = updatedProperty.PropertyTypeID;

            _db.SaveChanges();

            return Ok(existingProperty);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var propertyToDelete = _db.Properties.FirstOrDefault(p => p.ID == id);

            if (propertyToDelete == null)
            {
                return NotFound();
            }

            _db.Properties.Remove(propertyToDelete);
            _db.SaveChanges();
            return Ok("Property deleted.");
        }

    }
}
