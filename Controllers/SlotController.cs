using System.Globalization;
using clinic_reservation.Models;

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
        .ToList();
        return query;
    }

    public void AddSlot(int DoctorId, string startDate, string endDate)
    {
        var slot = new Slot();
        var doctor = _context.Doctor.Where(d => d.Id == DoctorId).FirstOrDefault();

        if (doctor == null)
        {
            throw new InvalidDataException("Doctor not found");
        }

        startDate = DateTime.Parse(startDate).ToString("yyyy-MM-dd HH:mm");
        endDate = DateTime.Parse(endDate).ToString("yyyy-MM-dd HH:mm");
        var slots = _context.Slot.Where(s => s.Doctor.Id == DoctorId)
                    .Where(s => s.startTime == startDate);


        if (slots.Any())
        {
            throw new InvalidDataException("Slot already exists");
        }
        
        // Console.WriteLine(startDate + "==================================");
        // Console.WriteLine(endDate + "==================================");
        // Console.WriteLine(DateTime.Parse(startDate).Month + "==================================");

        slot.Doctor = doctor;
        slot.startTime = startDate;
        _context.Slot.Add(slot);
        _context.SaveChanges();
    }
}
