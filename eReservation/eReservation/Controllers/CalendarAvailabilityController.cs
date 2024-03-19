using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalendarAvailabilityController : Controller
    {
        private readonly DataContext _db;

        public CalendarAvailabilityController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<CalendarAvailability>> GetAll()
        {
            var calendarAvailabilities = _db.CalendarAvailability.ToList();
            if (calendarAvailabilities.Any())
            {
                return Ok(calendarAvailabilities);
            }
            return NoContent();
        }

        [HttpGet("{propertiesId}")]
        public ActionResult<List<CalendarAvailability>> GetByPropertiesId(int propertiesId)
        {
            var propertyCalendarAvailabilities = _db.CalendarAvailability.Where(ca => ca.PropertiesID == propertiesId).ToList();
            if (propertyCalendarAvailabilities.Any())
            {
                return Ok(propertyCalendarAvailabilities);
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<CalendarAvailability> Add([FromBody] CalendarAvailability calendarAvailability)
        {
            if (calendarAvailability == null)
            {
                return BadRequest();
            }
            _db.CalendarAvailability.Add(calendarAvailability);
            _db.SaveChanges();
            return Ok(calendarAvailability);
        }

        [HttpPut("{id}")]
        public ActionResult<CalendarAvailability> Edit(int id, [FromBody] CalendarAvailability updatedCalendarAvailability)
        {
            var existingCalendarAvailability = _db.CalendarAvailability.FirstOrDefault(ca => ca.ID == id);
            if (existingCalendarAvailability == null)
            {
                return NotFound();
            }
            existingCalendarAvailability.Date = updatedCalendarAvailability.Date;
            existingCalendarAvailability.Availability = updatedCalendarAvailability.Availability;

            _db.SaveChanges();

            return Ok(existingCalendarAvailability);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var calendarAvailabilityToDelete = _db.CalendarAvailability.FirstOrDefault(ca => ca.ID == id);

            if (calendarAvailabilityToDelete == null)
            {
                return NotFound();
            }

            _db.CalendarAvailability.Remove(calendarAvailabilityToDelete);
            _db.SaveChanges();
            return Ok("Calendar availability deleted.");
        }
    }
}
