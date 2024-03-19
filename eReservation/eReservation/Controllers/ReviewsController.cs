using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private readonly DataContext _db;

        public ReviewsController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<Reviews>> GetAll()
        {
            var reviews = _db.Reviews.ToList();
            if (reviews.Any())
            {
                return Ok(reviews);
            }
            return NoContent();
        }

        [HttpGet("{userId}")]
        public ActionResult<List<Reviews>> GetByUserId(int userId)
        {
            var userReviews = _db.Reviews.Where(r => r.UserID == userId).ToList();
            if (userReviews.Any())
            {
                return Ok(userReviews);
            }
            return NotFound();
        }

        [HttpGet("{propertiesId}")]
        public ActionResult<List<Reviews>> GetByPropertiesId(int propertiesId)
        {
            var propertyReviews = _db.Reviews.Where(r => r.PropertiesID == propertiesId).ToList();
            if (propertyReviews.Any())
            {
                return Ok(propertyReviews);
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<Reviews> Add([FromBody] Reviews review)
        {
            if (review == null)
            {
                return BadRequest();
            }
            _db.Reviews.Add(review);
            _db.SaveChanges();
            return Ok(review);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var reviewToDelete = _db.Reviews.FirstOrDefault(r => r.ID == id);

            if (reviewToDelete == null)
            {
                return NotFound();
            }

            _db.Reviews.Remove(reviewToDelete);
            _db.SaveChanges();
            return Ok("Review deleted.");
        }
    }
}
