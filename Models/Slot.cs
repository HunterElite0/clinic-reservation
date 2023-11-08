using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using clinic_reservation.Models;

namespace clinic_reservation;

public class Slot
{
    public Slot() {}
    
    public int Id { get; set; }
    public string startTime { get; set; }
    public bool IsBooked { get; set; }


    // one to one with appointment
    public Appointment? Appointment { get; set; }

    // Many(slots) to One(doctor) with doctor
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

}
