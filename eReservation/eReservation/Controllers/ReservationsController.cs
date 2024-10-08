﻿using Azure.Core;
using eReservation.Data;
using eReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReservationsController : Controller
    {
        private readonly DataContext _db;

        public ReservationsController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<Reservations>> GetAll()
        {
            var reservations = _db.Reservations.ToList();
            if (reservations.Any())
            {
                return Ok(reservations);
            }
            return NoContent();
        }

        [HttpGet("{userId}")]
        public ActionResult<List<Reservations>> GetByUserId(int userId)
        {
            var userReservations = _db.Reservations.Where(r => r.UserID == userId).ToList();
            if (userReservations.Any())
            {
                return Ok(userReservations);
            }
            return NotFound();
        }

        [HttpGet("{propertiesId}")]
        public ActionResult<List<Reservations>> GetByPropertiesId(int propertiesId)
        {
            var propertyReservations = _db.Reservations.Where(r => r.PropertiesID == propertiesId).ToList();
            if (propertyReservations.Any())
            {
                return Ok(propertyReservations);
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<Reservations> Add([FromBody] Rezervacija request)
        {
            if (request == null)
            {
                return BadRequest("Reservation data is null.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }

            if (request.PropertiesId <= 0 || request.UserId  <= 0 || request.PaymentMethodsId <= 0)
            {
                return BadRequest("Invalid input data.");
            }

            var reservation = new Reservations
            {
                UserID = request.UserId,
                PaymentMethodsID = request.PaymentMethodsId,
                PropertiesID = request.PropertiesId,
                DateOfArrival = request.DateOfArrival,
                DateOfDeparture = request.DateOfDeparture,
                Status = request.Status,
                TotalPrice = request.TotalPrice
            };


            _db.Reservations.Add(reservation);
            _db.SaveChanges();
            return Ok(reservation);
        }

        [HttpPut("{id}")]
        public ActionResult<Reservations> Edit(int id, [FromBody] Reservations updatedReservation)
        {
            var existingReservation = _db.Reservations.FirstOrDefault(r => r.ID == id);
            if (existingReservation == null)
            {
                return NotFound();
            }
            existingReservation.DateOfArrival = updatedReservation.DateOfArrival;
            existingReservation.DateOfDeparture = updatedReservation.DateOfDeparture;
            existingReservation.Status = updatedReservation.Status;
            existingReservation.PaymentMethodsID = updatedReservation.PaymentMethodsID;

            _db.SaveChanges();

            return Ok(existingReservation);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var reservationToDelete = _db.Reservations.FirstOrDefault(r => r.ID == id);

            if (reservationToDelete == null)
            {
                return NotFound();
            }

            _db.Reservations.Remove(reservationToDelete);
            _db.SaveChanges();
            return Ok("Reservation deleted.");
        }
    }
}
