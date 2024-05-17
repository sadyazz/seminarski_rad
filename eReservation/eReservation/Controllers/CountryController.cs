using eReservation.Data;
using eReservation.Helpers.Auth;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorization]
    public class CountryController : Controller
    {
        private readonly DataContext _db;
        private readonly AuthService _authService;

        public CountryController(DataContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<List<Country>> GetAll()
        {
            var countries = _db.Countries.ToList();
            if (countries.Any())
            {
                return Ok(countries);
            }
            return NoContent();
        }
        [HttpPost]
        public ActionResult<Country> Add([FromBody] Country country)
        {

            KorisnickiNalog k = _authService.GetAuthInfo().korisnickiNalog!;

            if (!(k.isAdmin))
            {
                throw new Exception("nema pravo pristupa");
            }

            if (country == null)
            {
                return BadRequest();
            }
            _db.Countries.Add(country);
            _db.SaveChanges();
            return Ok(country);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Country> Edit([FromBody] Country countries, int id)
        {
            var ecountry = _db.Countries.FirstOrDefault(c => c.ID == id);
            if (ecountry == null)
            {
                return NotFound();
            }
            ecountry.Name = countries.Name;

            _db.SaveChanges();

            return Ok(ecountry);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var countryDelete = _db.Countries.FirstOrDefault(c => c.ID == id);

            if (countryDelete == null)
            {
                return NotFound();
            }
            _db.Countries.Remove(countryDelete);
            _db.SaveChanges();
            return Ok("Object deleted.");
        }
    }
}
