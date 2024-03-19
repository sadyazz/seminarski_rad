using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentsOnReviewsController : Controller
    {
        private readonly DataContext _db;

        public CommentsOnReviewsController(DataContext db)
        {
           _db = db;
        }

        [HttpGet]
        public ActionResult<List<CommentsOnReviews>> GetAll()
        {
            var comments = _db.CommentsOnReviews.ToList();
            if (comments.Any())
            {
                return Ok(comments);
            }
            return NoContent();
        }

        [HttpPost]
        public ActionResult<CommentsOnReviews> Add([FromBody] CommentsOnReviews comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }
            _db.CommentsOnReviews.Add(comment);
            _db.SaveChanges();
            return Ok(comment);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var commentToDelete = _db.CommentsOnReviews.FirstOrDefault(c => c.ID == id);

            if (commentToDelete == null)
            {
                return NotFound();
            }

            _db.CommentsOnReviews.Remove(commentToDelete);
            _db.SaveChanges();
            return Ok("Comment deleted.");
        }
    }
}
