using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace clinic_reservation;

[ApiController]
[Route("[controller]")]
[EnableCors]
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
  public JsonResult GetAppointments([FromQuery] String id)
  {
    try
    {
      int AccountIdInt = Int32.Parse(EncryptionHelper.Decrypt(id));
      var results = _appointmentController.GetPatientAppointments(AccountIdInt);
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
  public JsonResult AddAppointment(string AccountId, int SlotId)
  {
    try
    {
      int AccountIdInt = Int32.Parse(EncryptionHelper.Decrypt(AccountId));
      _appointmentController.MakeAppointment(AccountIdInt, SlotId);
    }
    catch (InvalidDataException e)
    {
      return new JsonResult(e.Message);
    }
    catch (Exception e)
    {

      return new JsonResult(e.Message);
    }

    return new JsonResult("Appointment added successfuly.");

  }
  [HttpDelete("appointments", Name = "CancelAppointment")]
  public JsonResult CancelAppointment(string AccountId, int AppointmentId)
  {
    try
    {
      int AccountIdInt = Int32.Parse(EncryptionHelper.Decrypt(AccountId));
      _appointmentController.CancelAppointment(AccountIdInt, AppointmentId);
    }
    catch (InvalidDataException e)
    {
      return new JsonResult(e.Message);
    }
    return new JsonResult("Appointment cancelled successfuly.");
  }

  [HttpPut("appointments", Name = "EditAppointment")]
  public JsonResult EditAppointment(string AccountId, int AppointmentId, int SlotId)
  {
    try
    {
      int AccountIdInt = Int32.Parse(EncryptionHelper.Decrypt(AccountId));
      _appointmentController.EditAppointment(AccountIdInt, AppointmentId, SlotId);
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
