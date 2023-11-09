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
            throw new InvalidDataException("Slot already exists");
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
}
