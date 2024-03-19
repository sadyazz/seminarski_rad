using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WishlistController : Controller
    {
        private readonly DataContext _db;

        public WishlistController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<Wishlist>> GetAll()
        {
            var wishlists = _db.Wishlist.ToList();
            if (wishlists.Any())
            {
                return Ok(wishlists);
            }
            return NoContent();
        }

        [HttpGet("{userId}")]
        public ActionResult<List<Wishlist>> GetByUserId(int userId)
        {
            var userWishlists = _db.Wishlist.Where(w => w.UserID == userId).ToList();
            if (userWishlists.Any())
            {
                return Ok(userWishlists);
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<Wishlist> Add([FromBody] Wishlist wishlist)
        {
            if (wishlist == null)
            {
                return BadRequest();
            }
            _db.Wishlist.Add(wishlist);
            _db.SaveChanges();
            return Ok(wishlist);
        }

        [HttpDelete("{userId}/{propertiesId}")]
        public ActionResult Delete(int userId, int propertiesId)
        {
            var wishlistToDelete = _db.Wishlist.FirstOrDefault(w => w.UserID == userId && w.PropertiesID == propertiesId);

            if (wishlistToDelete == null)
            {
                return NotFound();
            }

            _db.Wishlist.Remove(wishlistToDelete);
            _db.SaveChanges();
            return Ok("Wishlist item deleted.");
        }
    }
}
