using Microsoft.EntityFrameworkCore;

namespace clinic_reservation;

public class AppointmentController
{
    private readonly ClinicContext _context;

    public AppointmentController(ClinicContext context)
    {
        this._context = context;
    }

    public ICollection<Appointment> GetPatientAppointments(int Id)
    {
        var query = _context.Appointment
        .Where(a => a.Patient.Id == Id)
        .Include(a => a.Slot)
        .ToList();
        return query;
    }

}
