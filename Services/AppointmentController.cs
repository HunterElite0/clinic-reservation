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
        var accountQuery = _context.Account
            .Where(a => a.Id == Id)
            .FirstOrDefault() ?? throw new InvalidDataException("Account not found");

        var query = _context.Appointment
        .Where(a => a.Patient.AccountId == Id)
        .Include(a => a.Slot)
        .Include(s => s.Slot.Doctor)
        .ToList();
        return query;
    }
    public void MakeAppointment(int AccountId, int SlotId)
    {
        var slot = _context.Slot
            .Where(s => s.Id == SlotId)
            .FirstOrDefault() ?? throw new InvalidDataException("Slot not found");

        if (slot.IsBooked)
        {
            throw new InvalidOperationException("Slot is already booked.");
        }

        var patient = _context.Patient
            .Where(p => p.AccountId == AccountId)
            .FirstOrDefault() ?? throw new InvalidDataException("Patient not found");


    
        var appointment = new Appointment
        {
            PatientId = patient.Id,
            SlotId = SlotId
        };
        slot.IsBooked = true;
        _context.Appointment.Add(appointment);
        _context.SaveChanges();
    }

    public void CancelAppointment(int AccountId, int AppointmentId)
    {
        var appointment = _context.Appointment
            .Where(a => a.Id == AppointmentId && a.Patient.AccountId == AccountId)
            .FirstOrDefault() ?? throw new InvalidDataException("Appointment not found");

        var slot = _context.Slot
            .Where(s => s.Id == appointment.SlotId)
            .FirstOrDefault() ?? throw new InvalidDataException("Slot not found");

        slot.IsBooked = false;
        slot.Appointment = null;

        _context.Appointment.Remove(appointment);
        _context.SaveChanges();
    }

    public void EditAppointment(int AccountId, int AppointmentId, int SlotId)
    {
        try
        {
            CancelAppointment(AccountId, AppointmentId);
        }
        catch (Exception)
        {
            throw new InvalidDataException("Appointment not found");
        }

        var newSlot = _context.Slot
            .Where(s => s.Id == SlotId)
            .FirstOrDefault() ?? throw new InvalidDataException("Slot not found");

        if (!newSlot.IsBooked)
        {
            try
            {
                MakeAppointment(AccountId, SlotId);
            }
            catch (Exception)
            {
                throw new InvalidDataException("Slot is already booked.");
            }
        }

        
        _context.SaveChanges();
    }

}
