﻿using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static eReservation.Controllers.PropertiesController;
using eReservation.DTO;
using Newtonsoft.Json;

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

        [HttpGet]
        [Route("/GetReviewsByPropertyId")]
        public ActionResult<List<Reviews>> GetByPropertiesId([FromQuery] int propertyId)
        {
            var propertyReviews = _db.Reviews
      .Include(r => r.User) // Uključi korisnika
      .Where(r => r.PropertiesID == propertyId)
      .ToList();

            if (propertyReviews.Any())
            {
                var reviewDtos = propertyReviews.Select(r => new ReviewDto
                {
                    ID = r.ID,
                    UserID = r.UserID,
                    UserName = r.User != null ? r.User.Username : "Unknown",
                    UserFullName = r.User != null ? $"{r.User.Name} {r.User.Surname}" : "Unknown",
                    PropertiesID = r.PropertiesID,
                    Review = r.Review,
                    Comment = r.Comment,
                    DateReview = r.DateReview
                }).ToList();

                return Ok(reviewDtos);
            }
            return NoContent();
        }

        [HttpPost]
        public ActionResult<Reviews> Add([FromBody] ReviewCreateDto reviewDto )
        {
            Console.WriteLine($"Received Review: {JsonConvert.SerializeObject(reviewDto)}");

            if (reviewDto == null || reviewDto.UserID==0 || reviewDto.PropertiesID==0)
            {
                return BadRequest("Invalid review data.");
            }

            var review = new Reviews
            {
                UserID = reviewDto.UserID,
                PropertiesID = reviewDto.PropertiesID,
                Review = reviewDto.Review,
                Comment = reviewDto.Comment,
                DateReview = DateTime.Now
            };

            _db.Reviews.Add(review);
            _db.SaveChanges();

            return Json(new { message = "Review added successfully." });
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
