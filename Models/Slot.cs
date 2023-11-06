using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using clinic_reservation.Models;

namespace clinic_reservation;

public class Slot
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsBooked { get; set; }


    // one to one with appointment
    public Appointment? Appointment { get; set; }
    public int AppointmentId { get; set; }

    // Many(slots) to One(doctor) with doctor
    public Doctor Doctor { get; set; }

}
