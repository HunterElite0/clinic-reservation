using clinic_reservation.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace clinic_reservation.Controllers
{
    public class PatientController:ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ClinicContext _context;
        private readonly AppointmentService _appointmentService;
        public PatientController(IConfiguration configuration, ClinicContext context)
        {
            _configuration = configuration;
            _context = context;
            _appointmentService= new(_context);
        }

        [HttpGet("appointments/{patientId}", Name = "GetAppointments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAppointments(int patientId)
        {
            var patientQuery = _context.Patient
                .Where(p => p.Id == patientId)
                .FirstOrDefault();
            if (patientQuery == null)
            {
                return NotFound("Patient does not exist");
            }

            var results = _appointmentService.getPatientAppointments(patientId);
            if (results.Count == 0)
            {
                return new OkObjectResult("You have no appointments");
            }
            return new OkObjectResult(results);
        }

    }
}
