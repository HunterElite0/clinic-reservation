using System.Globalization;
using clinic_reservation.Models;
using Microsoft.EntityFrameworkCore;

namespace clinic_reservation;

public class SlotController
{
    private readonly ClinicContext _context;

    public SlotController(ClinicContext context)
    {
        this._context = context;
    }

    public ICollection<Slot> GetDoctorSlots(int Id)
    {
        var query = _context.Slot
        .Where(s => s.Doctor.Id == Id)
        .Include(s => s.Appointment)
        .ToList();
        return query;
    }

    public void AddSlot(int DoctorId, string StartTime)
    {
        var doctorQuery = _context.Doctor
            .Where(d => d.Id == DoctorId)
            .FirstOrDefault() ?? throw new InvalidDataException("Doctor not found");
        var slotQuery = _context.Slot
            .Where(s => s.Doctor.Id == DoctorId
            && s.StartTime == StartTime)
            .FirstOrDefault();

        // if slot exists for the same doctor, throw exception 
        if(slotQuery != null)
        {
            throw new InvalidOperationException("Slot already exists");
        }


        var slot = new Slot
        {
            StartTime = DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm"),
            IsBooked = false,
            DoctorId = DoctorId
        };

        _context.Slot.Add(slot);
        _context.SaveChanges();

    }

    public void CancelSlot(int DoctorId, int SlotId)
    {
        var slot =_context.Slot
            .Where(s => s.Id == SlotId && s.Doctor.Id == DoctorId)
            .FirstOrDefault() ?? throw new InvalidDataException("Slot not found");
        
        _context.Slot.Remove(slot);
        _context.SaveChanges();

    }

    public void UpdateSlot(int DoctorId, int SlotId, string StartTime)
    {
        var slot = _context.Slot
            .Where(s => s.Id == SlotId && s.Doctor.Id == DoctorId)
            .FirstOrDefault() ?? throw new InvalidDataException("Slot not found");

        slot.StartTime = DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm");
        _context.SaveChanges();
    }
}
