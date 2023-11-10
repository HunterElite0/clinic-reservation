using Microsoft.EntityFrameworkCore;

namespace clinic_reservation;

public class AppointmentController
{
    private readonly ClinicContext _context;

    public AppointmentController(ClinicContext context)
    {
        this._context = context;
    }

    public ICollection<Appointment> GetPatientAppointments(int Id)
    {
        var query = _context.Appointment
        .Where(a => a.Patient.Account.Id == Id)
        .Include(a => a.Slot)
        .ToList();
        return query;
    }
    public void MakeAppointment(int PatientId, int SlotId)
    {
        var slot = _context.Slot
            .Where(s => s.Id == SlotId)
            .FirstOrDefault() ?? throw new InvalidDataException("Slot not found");

        if (slot.IsBooked)
        {
            throw new InvalidOperationException("Slot is already booked.");
        }

        var patient = _context.Patient
            .Where(p => p.Id == PatientId)
            .FirstOrDefault() ?? throw new InvalidDataException("Patient not found");

        var appointment = new Appointment
        {
            PatientId = PatientId,
            SlotId = SlotId
        };
        slot.IsBooked = true;
        _context.Appointment.Add(appointment);
        _context.SaveChanges();
    }

    public void CancelAppointment(int PatientId, int AppointmentId)
    {
        var appointment = _context.Appointment
            .Where(a => a.Id == AppointmentId && a.Patient.Id == PatientId)
            .FirstOrDefault() ?? throw new InvalidDataException("Appointment not found");

        var slot = _context.Slot
            .Where(s => s.Id == appointment.SlotId)
            .FirstOrDefault() ?? throw new InvalidDataException("Slot not found");

        slot.IsBooked = false;

        _context.Appointment.Remove(appointment);
        _context.SaveChanges();
    }

}
