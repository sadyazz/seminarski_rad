using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PropertyTypeController : Controller
    {
        private readonly DataContext _db;

        public PropertyTypeController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<PropertyType>> GetAll()
        {
            var propertyTypes = _db.PropertyType.ToList();
            if (propertyTypes.Any())
            {
                return Ok(propertyTypes);
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<PropertyType> GetById(int id)
        {
            var propertyType = _db.PropertyType.Find(id);
            if (propertyType == null)
            {
                return NotFound();
            }
            return Ok(propertyType);
        }

        [HttpPost]
        public ActionResult<PropertyType> Add([FromBody] PropertyType propertyType)
        {
            if (propertyType == null)
            {
                return BadRequest();
            }
            _db.PropertyType.Add(propertyType);
            _db.SaveChanges();
            return Ok(propertyType);
        }

        [HttpPut("{id}")]
        public ActionResult<PropertyType> Edit(int id, [FromBody] PropertyType updatedPropertyType)
        {
            var existingPropertyType = _db.PropertyType.FirstOrDefault(p => p.ID == id);
            if (existingPropertyType == null)
            {
                return NotFound();
            }
            existingPropertyType.Name = updatedPropertyType.Name;
            existingPropertyType.Description = updatedPropertyType.Description;

            _db.SaveChanges();

            return Ok(existingPropertyType);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var propertyTypeToDelete = _db.PropertyType.FirstOrDefault(p => p.ID == id);

            if (propertyTypeToDelete == null)
            {
                return NotFound();
            }

            _db.PropertyType.Remove(propertyTypeToDelete);
            _db.SaveChanges();
            return Ok("Property type deleted.");
        }
    }
}
