using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CityController : Controller
    {
        private readonly DataContext _db;
        
        public CityController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<City>> GetAll()
        {
            var cities = _db.Cities.Include(i=>i.Country).ToList();
            if (cities.Any())
            {
                return Ok(cities);
            }
            
            return NoContent();
        }

        [HttpPost]


        public ActionResult<City> Add([FromBody] City city)
        {
            if (city == null)
            {
                return BadRequest();
            }
            var newCity = new City()
            {
                Name = city.Name,
                CountryID = city.CountryID
            };
            _db.Cities.Add(newCity);
            _db.SaveChanges();
            return Ok(city);
        }


        [HttpPut("{id:int}")]
        public ActionResult<City> Edit([FromBody] City city, int id)
        {
            var ecity = _db.Cities.FirstOrDefault(c => c.ID == id);
            if (ecity == null)
            {
                return NotFound();
            }
            ecity.Name = city.Name;
            _db.SaveChanges();

            return Ok(ecity);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete (int id)
        {
            var cityDelete= _db.Cities.FirstOrDefault(c=>c.ID==id);

            if(cityDelete == null)
            {
                return NotFound();
            }
            _db.Cities.Remove(cityDelete);
            _db.SaveChanges();
            return Ok("Object deleted.");
        }
    }
}
