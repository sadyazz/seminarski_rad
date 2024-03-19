using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentMethodsController : Controller
    {
        private readonly DataContext _db;

        public PaymentMethodsController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<PaymentMethods>> GetAll()
        {
            var paymentMethods = _db.PaymentMethods.ToList();
            if (paymentMethods.Any())
            {
                return Ok(paymentMethods);
            }
            return NoContent();
        }

        [HttpPost]
        public ActionResult<PaymentMethods> Add([FromBody] PaymentMethods paymentMethod)
        {
            if (paymentMethod == null)
            {
                return BadRequest();
            }
            _db.PaymentMethods.Add(paymentMethod);
            _db.SaveChanges();
            return Ok(paymentMethod);
        }

        [HttpPut("{id}")]
        public ActionResult<PaymentMethods> Edit(int id, [FromBody] PaymentMethods updatedPaymentMethod)
        {
            var existingPaymentMethod = _db.PaymentMethods.FirstOrDefault(p => p.ID == id);
            if (existingPaymentMethod == null)
            {
                return NotFound();
            }
            existingPaymentMethod.Name = updatedPaymentMethod.Name;

            _db.SaveChanges();

            return Ok(existingPaymentMethod);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var paymentMethodToDelete = _db.PaymentMethods.FirstOrDefault(p => p.ID == id);

            if (paymentMethodToDelete == null)
            {
                return NotFound();
            }

            _db.PaymentMethods.Remove(paymentMethodToDelete);
            _db.SaveChanges();
            return Ok("Payment method deleted.");
        }
    }
}
