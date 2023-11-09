namespace clinic_reservation.Dto
{
    public class AppointmentInformationDto
    {
        public int AppointmentId { get; set; }  
        public string DoctorName { get; set; }
        public string DoctorSpeciality { get; set; }
        public string StartTime { get; set; }
        public bool IsBooked { get; set; }

    }
}
