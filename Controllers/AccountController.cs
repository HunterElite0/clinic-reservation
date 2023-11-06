using System.Data;
using clinic_reservation.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        // string query = @"SELECT * FROM account";
        // DataTable table = new();
        // string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
        // MySqlDataReader myReader;
        // using (MySqlConnection myCon = new(sqlDataSource))
        // {
        //     myCon.Open();
        //     using (MySqlCommand myCommand = new(query, myCon))
        //     {
        //         myReader = myCommand.ExecuteReader();
        //         table.Load(myReader);
        //         myReader.Close();
        //         myCon.Close();
        //     }
        // }

        // return new JsonResult(table);

        var accounts = _context.Account.AsNoTracking().ToList();
        return new JsonResult(accounts);

    }

}
