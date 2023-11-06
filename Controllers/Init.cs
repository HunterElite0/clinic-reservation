using clinic_reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace clinic_reservation;

[ApiController]
[Route("[controller]")]
public class Init : ControllerBase
{   
    private readonly ClinicContext _context;
    private readonly ILogger<Init> _logger;

    public Init(ClinicContext context, ILogger<Init> logger)
    {
        this._context = context;
        this._logger = logger;

    }

    [HttpPost("run")]
    public JsonResult Run()
    {
        _logger.LogInformation("Initiating database");
        _context.Database.EnsureCreated();
        _logger.LogInformation("Database initiated");
        
        Account account = new Account();
        account.Username = "test";
        account.Email = "test@email.com";
        account.Password = "test";
        account.Role = Role.Doctor;
        _context.Account.Add(account);
        _context.SaveChanges();

        var dbAccount = _context.Account
            .Where(a => a.Username == account.Username)
            .AsNoTracking()
            .FirstOrDefault();
        
        return new JsonResult(dbAccount);
    } 
    

    

}
