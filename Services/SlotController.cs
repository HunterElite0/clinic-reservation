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

    public ICollection<Slot> GetDoctorSlots(int AccountId)
    {
        var query = _context.Slot
        .Where(s => s.Doctor.AccountId == AccountId)
        .Include(s => s.Appointment)
        .ToList();
        return query;
    }

    public void AddSlot(int AccountId, string StartTime)
    {
        var doctorQuery = _context.Doctor
            .Where(d => d.AccountId == AccountId)
            .FirstOrDefault() ?? throw new InvalidDataException("Doctor not found");
        var slotQuery = _context.Slot
            .Where(s => s.Doctor.AccountId == AccountId
            && s.StartTime == StartTime)
            .FirstOrDefault();

        // if slot exists for the same doctor, throw exception 
        if (slotQuery != null)
        {
            throw new InvalidOperationException("Slot already exists");
        }

        try
        {
            DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm");
        }
        catch
        {
            throw new InvalidDataException("Invalid date format");
        }

        var slot = new Slot
        {
            StartTime = DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm"),
            IsBooked = false,
            DoctorId = doctorQuery.Id
        };

        _context.Slot.Add(slot);
        _context.SaveChanges();

    }

    public void CancelSlot(int AccountId, int SlotId)
    {
        var accountQuery = _context.Account
            .Where(a => a.Id == AccountId)
            .FirstOrDefault() ?? throw new InvalidDataException("Account not found");

        var slot = _context.Slot
            .Where(s => s.Id == SlotId && s.Doctor.AccountId == AccountId)
            .FirstOrDefault() ?? throw new InvalidDataException("Slot not found");

        _context.Slot.Remove(slot);
        _context.SaveChanges();

    }

    public void UpdateSlot(int AccountId, int SlotId, string StartTime)
    {
        var accountQuery = _context.Account
          .Where(a => a.Id == AccountId)
          .FirstOrDefault() ?? throw new InvalidDataException("Account not found");

        var slot = _context.Slot
            .Where(s => s.Id == SlotId && s.Doctor.AccountId == AccountId)
            .FirstOrDefault() ?? throw new InvalidDataException("Slot not found");

        try
        {
            slot.StartTime = DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm");
        }
        catch
        {
            throw new InvalidDataException("Invalid date format");
        }
        _context.SaveChanges();
    }


}
