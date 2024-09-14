using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static eReservation.Controllers.PropertiesController;

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

        public class UserDto
        {
            public int ID { get; set; }
            public string Username { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime DateJoined { get; set; }
            public List<ReviewDto> Reviews { get; set; }
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

        //[HttpGet()]
        //[Route("/GetReviewByUserId")]
        //public ActionResult<List<Reviews>> GetByUserId([FromQuery]int userId)
        //{
        //    var userReviews = _db.Reviews
        //        .Include(r => r.User)
        //        .Include(r => r.Properties)
        //        .Where(r => r.UserID == userId).ToList();

        //    if (userReviews.Any())
        //    {
        //        return Ok(userReviews);
        //    }
        //    return NotFound();
        //}

        [HttpGet]
        [Route("/GetReviewByUserId")]
        public ActionResult<List<ReviewDto>> GetByUserId([FromQuery] int userId)
        {
            var userReviews = _db.Reviews
                .Include(r => r.User)
                .Include(r => r.Properties)
                .Where(r => r.UserID == userId)
                .ToList();

            if (userReviews.Any())
            {
                var reviewDtos = userReviews.Select(r => new ReviewDto
                {
                    ID = r.ID,
                    UserID = r.UserID,
                    UserName = r.User != null ? r.User.Username : "Unknown",
                    UserFullName = r.User != null ? $"{r.User.Name} {r.User.Surname}" : "Unknown",
                    PropertiesID = r.PropertiesID,
                    PropertyName = r.Properties != null ? r.Properties.Name : "Unknown",
                    Review = r.Review,
                    Comment = r.Comment,
                    DateReview = r.DateReview
                }).ToList();

                return Ok(reviewDtos);
            }
            return NoContent();
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
