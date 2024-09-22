using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using eReservation.DTO;

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
                .Select(p => new Properties
                {
                    ID = p.ID,
                    Name = p.Name,
                    Adress = p.Adress,
                    NumberOfRooms = p.NumberOfRooms,
                    NumberOfBathrooms = p.NumberOfBathrooms,
                    PricePerNight = p.PricePerNight,
                    City = p.City,
                    PropertyType = p.PropertyType,
                    Images = p.Images
                        .OrderBy(img => img.ID) 
                        .Take(1)
                        .ToList(),
                })
                .ToList();

            if (properties.Any())
            {
                return Ok(properties);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("/GetPropertyById")]
        public ActionResult<PropertyDto> Get(int id)
        {
            try
            {
                var property = _db.Properties
                    .Include(p => p.City)
                    .ThenInclude(c => c.Country)
                    .Include(p => p.PropertyType)
                    .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                    .Include(p => p.Images)
                    .FirstOrDefault(p => p.ID == id);

                if (property == null)
                {
                    return NotFound();
                }

                var averageRating = property.Reviews.Any() ? property.Reviews.Average(r => r.Review) : 0;
                var averageRatingDecimal = (decimal)averageRating;

                var propertyDto = new PropertyDto
                {
                    ID = property.ID,
                    Name = property.Name,
                    Address = property.Adress,
                    NumberOfRooms = property.NumberOfRooms,
                    NumberOfBathrooms = property.NumberOfBathrooms,
                    PricePerNight = property.PricePerNight,
                    CityID = property.CityID,
                    CityName = property.City != null ? property.City.Name : "Unknown",
                    CountryName = property.City != null && property.City.Country != null ? property.City.Country.Name : "Unknown",
                    PropertyTypeID = property.PropertyTypeID,
                    PropertyTypeName = property.PropertyType != null ? property.PropertyType.Name : "Unknown",
                    Reviews = property.Reviews.Select(r => new ReviewDto
                    {
                        ID = r.ID,
                        UserID = r.UserID,
                        UserName = r.User != null ? r.User.Username : "Unknown",
                        UserFullName = r.User != null ? $"{r.User.Name} {r.User.Surname}" : "Unknown",
                        PropertiesID = r.PropertiesID,
                        Review = r.Review,
                        Comment = r.Comment,
                        DateReview = r.DateReview
                    }).ToList(),
                    AverageRating = averageRatingDecimal,
                    Images = property.Images?.Select(img => new ImageDto
                            {
                                ID = img.ID,
                                Path = img.Path
                            }).ToList() ?? new List<ImageDto>()
                };

                return Ok(propertyDto);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
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
        [HttpPost]
        [Route("/UploadPropertyImage")]
        public async Task<ActionResult> UploadPropertyImage([FromQuery] int id, [FromForm] IFormFile image)
        {
            //if (!_authService.JelLogiran())
            //{
            //    return Unauthorized("Korisnik nije prijavljen.");
            //}

            var property = _db.Properties.Include(p => p.Images).FirstOrDefault(p => p.ID == id);
            if (property == null)
            {
                return NotFound("Nekretnina nije pronađena.");
            }

            if (image == null || image.Length == 0)
            {
                return BadRequest("Slika nije validna.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var base64Image = Convert.ToBase64String(imageBytes);

                property.Images.Add(new Images
                {
                    Path = base64Image,
                    PropertyId = id
                });
                _db.SaveChanges();
            }

            return Ok(new { message = "Slika uspješno uploadovana." });
        }

        [HttpGet]
        [Route("/GetPropertyImage")]
        public ActionResult<string> GetPropertyImage([FromQuery] int id)
        {
            var property = _db.Properties
                .Include(p => p.Images)
                .FirstOrDefault(p => p.ID == id);

            if (property == null || !property.Images.Any())
            {
                return NotFound("Nekretnina ili slika nisu pronađeni.");
            }

            var image = property.Images.FirstOrDefault();
            if (image == null || string.IsNullOrEmpty(image.Path))
            {
                return NotFound("Slika nije pronađena.");
            }

            return Ok(image.Path);
        }
        [HttpGet]
        [Route("/GetPropertyImages")]
        public ActionResult<IEnumerable<string>> GetPropertyImages([FromQuery] int propertyId)
        {
            var property = _db.Properties
                .Include(p => p.Images)
                .FirstOrDefault(p => p.ID == propertyId);

            if (property == null || !property.Images.Any())
            {
                return NotFound("Nekretnina ili slike nisu pronađene.");
            }

            var imagePaths = property.Images
                .Where(image => !string.IsNullOrEmpty(image.Path))
                .Select(image => image.Path)
                .ToList();

            return Ok(imagePaths);
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
