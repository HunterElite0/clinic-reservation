using System.ComponentModel.DataAnnotations.Schema;
using clinic_reservation.Models;

namespace clinic_reservation;

public class Doctor
{

    public Doctor() { }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Speciality { get; set; }


    // One (Doctor) to Many (Slots)
    public ICollection<Slot> Slots { get; set; }

    public int AccountId { get; set; }
    public required Account Account { get; set; } = null!;

}
