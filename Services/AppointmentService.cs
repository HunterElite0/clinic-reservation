using clinic_reservation.Dto;
using Microsoft.EntityFrameworkCore;

namespace clinic_reservation.Services
{
    public class AppointmentService
    {
        private readonly ClinicContext _context;

        public AppointmentService(ClinicContext context)
        {
            _context = context;
        }

        public ICollection<AppointmentInformationDto> getPatientAppointments(int patientId)
        {
            var appointments = _context.Appointment
                .Where(a => a.Patient.Id == patientId)   
                .Select(appointment => new AppointmentInformationDto
                {
                    AppointmentId = appointment.Id,
                    DoctorName = appointment.Slot.Doctor.Name,
                    DoctorSpeciality = appointment.Slot.Doctor.Speciality,
                    StartTime = appointment.Slot.StartTime,
                    IsBooked = appointment.Slot.IsBooked
                }).ToList();

            return appointments;
        }
    }
}
