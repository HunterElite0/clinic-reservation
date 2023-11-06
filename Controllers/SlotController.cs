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

    public void AddSlot(int DoctorId, DateTime date){
        var slot = new Slot();
        var doctor = _context.Doctor.Where(d => d.Id == DoctorId).FirstOrDefault();
        
        if(doctor == null){
            throw new InvalidDataException("Doctor not found");
        }

        slot.Doctor = doctor;
        slot.Date = date;
        _context.Slot.Add(slot);
        _context.SaveChanges();
    }
}
