using clinic_reservation.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clinic_reservation;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly ClinicContext _context;
    private readonly IConfiguration _configuration;
    private readonly AppointmentController _appointmentController;
    public PatientController(ClinicContext context, IConfiguration configuration)
    {
        this._context = context;
        this._configuration = configuration;
        this._appointmentController = new(this._context);
    }

    [HttpGet("patients", Name = "GetPatients")]
    public JsonResult GetPatients()
    {
        var results = _context.Patient
            .Include(p => p.Account)
            .Include(p => p.Appointments)
            .ToList();
        return new JsonResult(results);
    }

    [HttpGet("appointments", Name = "GetAppointments")]
    public JsonResult GetAppointments([FromQuery] int id)
    {
        try
        {
            var results = _appointmentController.GetPatientAppointments(id);
            if (results.Count == 0)
            {
                return new JsonResult("You have no appointments.");
            }
            return new JsonResult(results);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        }


    }

    [HttpPost("appointments", Name = "AddAppointment")]
    public JsonResult AddAppointment(int AccountId, int SlotId)
    {
        try
        {
            _appointmentController.MakeAppointment(AccountId, SlotId);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        }
        catch (Exception e)
        {

            return new JsonResult(e.Message);
        }

        string message = $"patientAccountId: {AccountId}, Operation: ReservationCreated";
        var sender = new RabbitMq(message);
        return new JsonResult("Appointment added successfuly.");

    }
    [HttpDelete("appointments", Name = "CancelAppointment")]
    public JsonResult CancelAppointment(int AccountId, int AppointmentId)
    {
        try
        {
            _appointmentController.CancelAppointment(AccountId, AppointmentId);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        }
        return new JsonResult("Appointment cancelled successfuly.");
    }

    [HttpPut("appointments", Name = "EditAppointment")]
    public JsonResult EditAppointment(int AccountId, int AppointmentId, int SlotId)
    {
        try
        {
            _appointmentController.EditAppointment(AccountId, AppointmentId, SlotId);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return new JsonResult(e.Message);
        }
        return new JsonResult("Appointment updated successfuly.");
    }
}