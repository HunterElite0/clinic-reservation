namespace clinic_reservation.Services
{
    public class AppointmentSlotService
    {
        private SlotService _slotService;
        private AppointmentService _appointmentService;
        private ClinicContext _context;
        public AppointmentSlotService(ClinicContext context)
        {
            _context = context;
            _slotService = new(context);
            _appointmentService = new(context);
        }


    }
}
