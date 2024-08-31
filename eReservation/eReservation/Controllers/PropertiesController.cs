using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        public class ReviewDto
        {
            public int ID { get; set; }
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string UserFullName { get; set; }
            public int PropertiesID { get; set; }
            public string PropertyName { get; set; }
            public int Review { get; set; }
            public string Comment { get; set; }
            public DateTime DateReview { get; set; }
        }

        public class PropertyDto
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public int NumberOfRooms { get; set; }
            public int NumberOfBathrooms { get; set; }
            public int PricePerNight { get; set; }
            public int CityID { get; set; }
            public string CityName { get; set; }
            public string CountryName { get; set; }
            public int PropertyTypeID { get; set; }
            public string PropertyTypeName { get; set; }
            public List<ReviewDto> Reviews { get; set; }
            public decimal AverageRating { get; set; }
            public List<ImageDto> Images { get; set; }
        }

        public class ImageDto
        {
            public int ID { get; set; }
            public string Path { get; set; }
        }

        //[HttpGet]
        //public ActionResult<List<Properties>> GetAll()
        //{
        //    var properties = _db.Properties
        //        .Include(p => p.City).ThenInclude(c => c.Country)
        //        .Include(p => p.PropertyType)
        //        .Include(p => p.Images)
        //        //.Include(p => p.Reviews)
        //        .ToList();

        //    if (properties.Any())
        //    {
        //        return Ok(properties);
        //    }



        //   return NoContent();
        //}

       

        //[HttpGet]
        //public ActionResult<List<PropertyDto>> GetAll()
        //{
        //    try
        //    {
        //        var properties = _db.Properties
        //            .Include(p => p.City)
        //            .ThenInclude(c => c.Country)
        //            .Include(p => p.PropertyType)
        //            .Include(p => p.Images)
        //            .Include(p => p.Reviews)
        //            .ThenInclude(r => r.User)
        //            .ToList();

        //        if (properties == null || !properties.Any())
        //        {
        //            return NoContent();
        //        }

        //        var propertyDtos = properties.Select(p => new PropertyDto
        //        {
        //            ID = p.ID,
        //            Name = p.Name,
        //            Address = p.Adress,
        //            NumberOfRooms = p.NumberOfRooms,
        //            NumberOfBathrooms = p.NumberOfBathrooms,
        //            PricePerNight = p.PricePerNight,
        //            CityID = p.CityID,
        //            //CityName = p.City?.Name ?? "Unknown",
        //            //CountryName = p.City?.Country?.Name ?? "Unknown",
        //            PropertyTypeID = p.PropertyTypeID,
        //            //PropertyTypeName = p.PropertyType?.Name ?? "Unknown",
        //            //Reviews = p.Reviews?.Select(r => new ReviewDto
        //            //{
        //            //    ID = r.ID,
        //            //    UserID = r.UserID,
        //            //    UserName = r.User?.Username ?? "Unknown",
        //            //    UserFullName = r.User != null ? $"{r.User.Name} {r.User.Surname}" : "Unknown",
        //            //    PropertiesID = r.PropertiesID,
        //            //    Review = r.Review,
        //            //    Comment = r.Comment,
        //            //    DateReview = r.DateReview
        //            //}).ToList() ?? new List<ReviewDto>(),
        //            AverageRating = p.Reviews?.Any() == true ? (decimal)p.Reviews.Average(r => r.Review) : 0m,
        //            Images = p.Images?.Select(img => new ImageDto
        //            {
        //                ID = img.ID,
        //                Path = img.Path
        //            }).ToList() ?? new List<ImageDto>()
        //        }).ToList();

        //        return Ok(propertyDtos);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpGet]
        //[Route("/GetPropertyById")]
        //public ActionResult<Properties> Get(int id)
        //{
        //    //var property = _db.Properties.Find(id);
        //    try
        //    {
        //        var property = _db.Properties
        //            .Include(p => p.City)
        //            .ThenInclude(c => c.Country)
        //            .Include(p => p.PropertyType)
        //            //.Include(p => p.Reviews)
        //            //.Include(p => p.PropertyImageUrls)
        //            .FirstOrDefault(p => p.ID == id);

        //        if (property == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(property);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log exception here
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

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
