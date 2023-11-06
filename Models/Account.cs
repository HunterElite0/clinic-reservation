using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace clinic_reservation.Models;

public enum Role
{
    Doctor,
    Patient,
}
[Index(nameof(Email), IsUnique = true)]
public class Account
{
    public Account(string Email, string Password, Role Role)
    {
        this.Email = Email;
        this.Password = Password;
        this.Role = Role;
    }

    public Account() { }
    public int Id { get; set; }


    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}
