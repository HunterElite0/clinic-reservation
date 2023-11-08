using clinic_reservation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace clinic_reservation;

public class ClinicContext : DbContext
{
    public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
    {
    }

    public DbSet<Account> Account { get; set; }
    public DbSet<Doctor> Doctor { get; set; }
    public DbSet<Patient> Patient { get; set; }
    public DbSet<Slot> Slot { get; set; }
    public DbSet<Appointment> Appointment { get; set; }
}
