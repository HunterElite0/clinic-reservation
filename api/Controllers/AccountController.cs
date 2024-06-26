﻿using System.Data;
using BCrypt.Net;
using clinic_reservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clinic_reservation;
[ApiController]
[Route("[controller]")]
[EnableCors]
public class AccountController : ControllerBase
{
  private readonly IConfiguration _configuration;
  private readonly ClinicContext _context;
  public AccountController(IConfiguration configuration, ClinicContext context)
  {
    _configuration = configuration;
    _context = context;
  }


  // [HttpGet(Name = "GetAccounts")]
  // public JsonResult GetAccounts()
  // {
  //     var accounts = _context.Account.AsNoTracking().ToList();
  //     return new JsonResult(accounts);
  // }


  [HttpPost("signup", Name = "Signup")]
  public JsonResult Signup(AccountInformationDto accountInformationDto)
  {

    var accountQuery = _context.Account
    .Where(a => a.Email == accountInformationDto.Account.Email)
    .FirstOrDefault();

    if (accountQuery != null)
    {
      return new JsonResult("Email already exists");
    }

    var account = new Account
    {
      Email = accountInformationDto.Account.Email,
      Password = BCrypt.Net.BCrypt.HashPassword(accountInformationDto.Account.Password),
      Role = accountInformationDto.Account.Role
    };
    _context.Account.Add(account);

    if (accountInformationDto.Account.Role == Role.Doctor)
    {
      var doctor = new Doctor
      {
        Name = accountInformationDto.Name,
        Speciality = accountInformationDto.Speciality,
        Account = account
      };
      _context.Doctor.Add(doctor);
    }
    else if (accountInformationDto.Account.Role == Role.Patient)
    {
      var patient = new Patient
      {
        Name = accountInformationDto.Name,
        Account = account
      };
      _context.Patient.Add(patient);
    }
    else
    {
      return new JsonResult("Error creating account");
    }

    _context.SaveChanges();

    return new JsonResult("Account created successfuly");
  }

  [HttpPost("signin", Name = "Signin")]
  public JsonResult Signin(Account account)
  {
    try
    {
      var query = _context.Account
      .Where(a => a.Email == account.Email)
      .FirstOrDefault();
      var nameQuery = "";

      if (query == null)
      {
        return new JsonResult("Email or password is incorrect2");
      }

      if (!BCrypt.Net.BCrypt.Verify(account.Password, query.Password))
      {
        return new JsonResult("Email or password is incorrect");
      }

      if (query?.Role == Role.Doctor)
      {
        nameQuery = _context.Doctor
        .Where(d => d.AccountId == query.Id)
        .Select(d => d.Name)
        .FirstOrDefault();
      }
      else if (query?.Role == Role.Patient)
      {
        nameQuery = _context.Patient
        .Where(p => p.AccountId == query.Id)
        .Select(p => p.Name)
        .FirstOrDefault();
      }

      if (query == null)
      {
        return new JsonResult("Error getting account information");
      }

      var accountDto = new AccountDto
      {
        Role = (query.Role == Role.Doctor ? 0 : 1),
        Id = EncryptionHelper.Encrypt(query.Id.ToString()),
        Name = nameQuery!.ToString(),
      };

      return new JsonResult(accountDto);
    }
    catch (System.Exception)
    {
      return new JsonResult("Server Error");
    }
  }
}
