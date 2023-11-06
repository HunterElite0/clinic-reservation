using clinic_reservation.Models;

namespace clinic_reservation;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Speciality { get; set; }
    public Account Account { get; set; }


    // one to many with appointement
    public ICollection<Appointment> Appointments { get; set; }
}
