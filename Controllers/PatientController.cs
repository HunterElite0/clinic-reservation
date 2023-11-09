using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clinic_reservation;

public class PatientController
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
            .Include(p => p.Appointments.Count)
            .ToList();
        return new JsonResult(results);
    }

    [HttpGet("appointments", Name = "GetAppointments")]
    public JsonResult GetAppointments(int PatientId)
    {
        var results = _appointmentController.GetPatientAppointments(PatientId);

        if (results.Count == 0)
        {
            return new JsonResult("You have no appointments.");
        }
        return new JsonResult(results);
    }

    [HttpPost("appointments", Name = "AddAppointment")]
    public JsonResult AddAppointment(int PatientId, int SlotId)
    {
        var slot = _context.Slot
            .Where(s => s.Id == SlotId)
            .FirstOrDefault() ?? throw new InvalidDataException("Slot not found");

        if (slot.IsBooked)
        {
            return new JsonResult("Slot is already booked.");
        }

        var patient = _context.Patient
            .Where(p => p.Id == PatientId)
            .FirstOrDefault() ?? throw new InvalidDataException("Patient not found");

        var appointment = new Appointment
        {
            PatientId = PatientId,
            SlotId = SlotId
        };

        _context.Appointment.Add(appointment);
        _context.SaveChanges();

        return new JsonResult("Appointment added successfuly.");
    }
}