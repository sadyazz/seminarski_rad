using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using eReservation.DTO;
using eReservation.Helpers.Auth;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PropertiesController : Controller
    {
        private readonly DataContext _db;
        private readonly AuthService _authService;
        private readonly FirebaseService _firebaseService;

        public PropertiesController(DataContext db, AuthService authService, FirebaseService firebaseService)
        {
            _db = db;
            _authService = authService;
            _firebaseService = firebaseService;
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
                    Latitude = property.Latitude,
                    Longitude = property.Longitude,
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
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetPropertyCoordinates")]
        public ActionResult<CoordinateDto> GetPropertyCoordinates(int propertyId)
        {
            try
            {
                var property = _db.Properties
                    .FirstOrDefault(p => p.ID == propertyId);

                if (property == null)
                {
                    return NotFound("Property not found.");
                }

                var coordinatesDto = new CoordinateDto
                {
                    Latitude = property.Latitude,
                    Longitude = property.Longitude
                };

                return Ok(coordinatesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<Properties> Add([FromBody] CreatePropertyDto propertyDto)
        {
            if (propertyDto == null)
            {
                return BadRequest("Property data is null.");
            }

            var city = _db.Cities.Find(propertyDto.CityID);
            if (city == null)
            {
                return BadRequest("Invalid City ID.");
            }

            var propertyType = _db.PropertyType.Find(propertyDto.PropertyTypeID);
            if (propertyType == null)
            {
                return BadRequest("Invalid Property Type ID.");
            }

            var user = _db.User.Find(propertyDto.UserID);
            if (user == null)
            {
                return BadRequest("Invalid User ID.");
            }

            var property = new Properties
            {
                Name = propertyDto.Name,
                Adress = propertyDto.Address,
                NumberOfRooms = propertyDto.NumberOfRooms,
                NumberOfBathrooms = propertyDto.NumberOfBathrooms,
                PricePerNight = propertyDto.PricePerNight,
                CityID = propertyDto.CityID,
                PropertyTypeID = propertyDto.PropertyTypeID,
                UserID = propertyDto.UserID,
                Latitude = propertyDto.Latitude,
                Longitude = propertyDto.Longitude,
            };

            if (property.Name == null || property.Adress == null)
            {
                return BadRequest("Property name and address cannot be null.");
            }


            _db.Properties.Add(property);
            _db.SaveChanges();

            if (propertyDto.AmenitiesIDs != null && propertyDto.AmenitiesIDs.Any())
            {
                var existingAmenities = _db.Amenities
                    .Where(a => propertyDto.AmenitiesIDs.Contains(a.ID))
                    .Select(a => a.ID)
                    .ToList();

                if (existingAmenities.Count != propertyDto.AmenitiesIDs.Count)
                {
                    return BadRequest("Some of the provided Amenity IDs are invalid.");
                }

                var propertyAmenities = propertyDto.AmenitiesIDs
                    .Where(amenityId => existingAmenities.Contains(amenityId))
                    .Select(amenityId => new PropertiesAmenities
                    {
                        PropertiesID = property.ID,
                        AmenitiesID = amenityId
                    })
                    .ToList();

                if (propertyAmenities.Any())
                {
                    _db.PropertiesAmenities.AddRange(propertyAmenities);
                    _db.SaveChanges();
                }
            }

            var response = new PropertyDto 
            { Address = property.Adress,
              Name = property.Name,
              NumberOfRooms = property.NumberOfRooms,
              NumberOfBathrooms = property.NumberOfBathrooms,
              PricePerNight = property.PricePerNight,
              CityID = property.CityID,
              PropertyTypeID = property.PropertyTypeID,
              Latitude = property.Latitude,
              Longitude = property.Longitude,
            };

            return Ok(response);
        }

        [HttpPut("{id}/update-coordinates")]
        public ActionResult<Properties> UpdateCoordinates(int id, [FromBody] UpdateCoordinateDto updatedCoordinates)
        {
            var existingProperty = _db.Properties.FirstOrDefault(p => p.ID == id);
            if (existingProperty == null)
            {
                return NotFound("Property not found.");
            }

            existingProperty.Latitude = updatedCoordinates.Latitude;
            existingProperty.Longitude = updatedCoordinates.Longitude;

            _db.SaveChanges();

            return Ok(existingProperty);
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
            // Check if user is logged in (optional, depending on your auth logic)
            if (!_authService.JelLogiran())
            {
                return Unauthorized("Korisnik nije prijavljen.");
            }

            // Find the property by ID
            var property = await _db.Properties.Include(p => p.Images).FirstOrDefaultAsync(p => p.ID == id);
            if (property == null)
            {
                return NotFound("Nekretnina nije pronađena.");
            }

            // Validate the image file
            if (image == null || image.Length == 0)
            {
                return BadRequest("Slika nije validna.");
            }

            try
            {
                // Upload the image to Firebase (or your storage service)
                var uploadUrl = await _firebaseService.UploadImageAsync(image); // Assuming FirebaseService handles this

                // Save the image URL to the Images collection for the property
                property.Images.Add(new Images
                {
                    Path = uploadUrl,
                    PropertyId = id
                });

                // Save changes to the database
                await _db.SaveChangesAsync();

                return Ok(new { message = "Slika uspješno uploadovana.", url = uploadUrl });
            }
            catch (Exception ex)
            {
                // Handle any errors
                return StatusCode(500, "Greška prilikom uploadovanja slike: " + ex.Message);
            }
        }



        [HttpGet]
        [Route("/GetPropertyImages")]
        public ActionResult<IEnumerable<string>> GetPropertyImages([FromQuery] int propertyId)
        {
            var property = _db.Properties.Include(p => p.Images).FirstOrDefault(p => p.ID == propertyId);

            if (property == null || !property.Images.Any())
            {
                Console.WriteLine($"Property with ID {propertyId} not found or has no images.");
                return NotFound("Property or images not found.");
            }

            var imagePaths = property.Images
                .Where(image => !string.IsNullOrEmpty(image.Path))
                .Select(image => image.Path)
                .ToList();

            return Ok(imagePaths);
        }


        //public string GetImageUrl(string imageName, string token)
        //{
        //    string bucketName = "bucket-5c7b9.appspot.com/images"; 
        //    string encodedPath = Uri.EscapeDataString($"images/{imageName}"); 
        //    string url = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{encodedPath}?alt=media&token={token}";

        //    return url;
        //}

        //[HttpGet]
        //[Route("/GetPropertyImages")]
        //public ActionResult<IEnumerable<string>> GetPropertyImages([FromQuery] int propertyId)
        //{
        //    var property = _db.Properties
        //        .Include(p => p.Images)
        //        .FirstOrDefault(p => p.ID == propertyId);

        //    if (property == null || !property.Images.Any())
        //    {
        //        return NotFound("Nekretnina ili slike nisu pronađene.");
        //    }

        //    var imagePaths = property.Images
        //        .Where(image => !string.IsNullOrEmpty(image.Path))
        //        .Select(image => image.Path)
        //        .ToList();

        //    return Ok(imagePaths);
        //}




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
            return Ok(new { message = "Property deleted." });

        }

    }
}
