using Application.Dto.Appointment;
using Application.Features.Appointment.Command.AddAppointmentCommand;
using Application.Interfaces.Services.AppointmentService;
using Domain.Entities;
using Infrastructure.Persistence.Services.AppointmentService;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;

namespace API
{
    [Route("appointment")]
    [ApiController]
    public class AppointmentController : BaseApiController
    {
        private readonly IAppointmentService _appointmentService;

        /// <summary>
        /// Creates a new appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /appointment/bookDoctors
        ///     {
        ///         "Id": 0,
        ///         "Date": "29/12/2024",
        ///         "TimeStart": "09:30 AM",
        ///         "TimeEnd": "10:30 AM",
        ///         "DoctorId": 1,
        ///         "PatientId": 2,
        ///         "Status": "Scheduled",
        ///         "Notes": "Regular checkup"
        ///     }
        /// </remarks>
        [HttpPost("bookDoctors")]

        public async Task<IActionResult> BookDoctor([FromBody] AddAppointmentCommand command)
        {
            try
            {

                var doctor = await Mediator.Send(command);

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPost("BookDoctor")]
        //public async Task<IActionResult> BookDoctors(AppointmentDto model)
        //{
        //    try
        //    {

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest();
        //        }
        //        var isAvailable = await _appointmentService.CheckAvailability(model.DoctorId, model.Date, model.TimeStart);
        //        if (!isAvailable)
        //        {
        //            return BadRequest("Doctor not available");
        //        }
        //        model.TimeEnd = model.TimeStart.AddMinutes(30);
        //        var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //        await _appointmentService.AddAsync(model);

        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
