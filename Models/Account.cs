using System.ComponentModel.DataAnnotations;

namespace clinic_reservation.Models;

public enum Role
{
    Patient,
    Doctor,
}
public class Account
{
    public Account(string Username, string Email, string Password, Role Role)
    {
        this.Username = Username;
        this.Email = Email;
        this.Password = Password;
        this.Role = Role;
    }

    public Account(){}
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}
