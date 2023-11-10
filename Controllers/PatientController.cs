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
        var results = _appointmentController.GetPatientAppointments(id);

        if (results.Count == 0)
        {
            return new JsonResult("You have no appointments.");
        }
        return new JsonResult(results);
    }

    [HttpPost("appointments", Name = "AddAppointment")]
    public JsonResult AddAppointment(int PatientId, int SlotId)
    {
        try
        {
            _appointmentController.MakeAppointment(PatientId, SlotId);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        }
        catch (InvalidOperationException e)
        {
            
            return new JsonResult(e.Message);
        }

        return new JsonResult("Appointment added successfuly.");
    }
    [HttpDelete("appointments", Name = "CancelAppointment")]
    public JsonResult CancelAppointment(int PatientId, int AppointmentId)
    {
        try
        {
            _appointmentController.CancelAppointment(PatientId, AppointmentId);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        }
        return new JsonResult("Appointment cancelled successfuly.");
    }
}