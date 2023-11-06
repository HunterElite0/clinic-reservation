namespace clinic_reservation;

public class Appointment
{
    public int Id { get; set; }

    // one to one with slot
    public Slot Slot { get; set; }
    // many to one with patient
    public Patient Patient { get; set; }
    
}
