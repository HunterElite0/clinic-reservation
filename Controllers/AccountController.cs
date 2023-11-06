using System.Data;
using clinic_reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clinic_reservation;
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ClinicContext _context;
    public AccountController(IConfiguration configuration, ClinicContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpGet(Name = "GetAccounts")]
    public JsonResult GetAccounts()
    {
        var accounts = _context.Account.AsNoTracking().ToList();
        return new JsonResult(accounts);
    }

    [HttpPost("signup", Name = "Signup")]
    public JsonResult Signup(Account account)
    {
        var query = _context.Account
        .Where(a => a.Email == account.Email)
        .FirstOrDefault();

        if (query != null)
        {
            return new JsonResult("Email already exists");
        }

        _context.Account.Add(account);
        _context.SaveChanges();
        return new JsonResult("Account created successfuly");
    }

    [HttpPost("signin", Name = "Signin")]
    public JsonResult Signin(Account account)
    {
        var query = _context.Account
        .Where(a => a.Email == account.Email && a.Password == account.Password)
        .FirstOrDefault();

        if (query == null)
        {
            return new JsonResult("Email or password is incorrect");
        }

        return new JsonResult(query);
    }
}
