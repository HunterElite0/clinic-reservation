namespace clinic_reservation;

public class Appointment
{
    public int Id { get; set; }

    // one to one with slot
    public int SlotId { get; set; }
    public Slot Slot { get; set; } = null!;
    // many to one with patient
    public Patient Patient { get; set; }
    
}
