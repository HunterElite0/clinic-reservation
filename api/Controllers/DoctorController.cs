using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using clinic_reservation.Hubs;
using System.Net.WebSockets;
using System.Text;

namespace clinic_reservation;
[ApiController]
[Route("[controller]")]
[EnableCors]
public class DoctorController : ControllerBase
{
  private readonly IConfiguration _configuration;
  private readonly ClinicContext _context;
  private readonly SlotController _slotController;
  public DoctorController(IConfiguration configuration, ClinicContext context)
  {
    _configuration = configuration;
    _context = context;
    _slotController = new(this._context);
  }


  [HttpGet("doctors", Name = "GetDoctors")]
  public JsonResult GetDoctors()
  {
    var results = _context.Doctor
        .Include(d => d.Account)
        .Include(d => d.Slots)
        .ToList();
    return new JsonResult(results);
  }


  [HttpGet("slots", Name = "GetSlots")]
  public JsonResult GetSlots([FromQuery] string id)
  {
    try
    {
      int AccountIdInt = Int32.Parse(EncryptionHelper.Decrypt(id));
      var results = _slotController.GetDoctorSlots(AccountIdInt);
      if (results.Count == 0)
      {
        return new JsonResult("You have no open slots.");
      }
      return new JsonResult(results);
    }
    catch (InvalidDataException e)
    {
      return new JsonResult(e.Message);
    }
  }

  [HttpPost("slots", Name = "AddSlot")]
  public JsonResult AddSlot(string AccountId, [FromBody] string StartDate)
  {
    try
    {
      int AccountIdInt = Int32.Parse(EncryptionHelper.Decrypt(AccountId));
      _slotController.AddSlot(AccountIdInt, StartDate);
    }
    catch (InvalidDataException e)
    {
      return new JsonResult(e.Message);
    }
    catch (InvalidOperationException e)
    {
      return new JsonResult(e.Message);
    }

    return new JsonResult("Slot added successfuly.");
  }

  [HttpDelete("slots", Name = "UpdateSlot")]
  public JsonResult CancelSlot(String AccountId, int SlotId)
  {
    try
    {
      int AccountIdInt = Int32.Parse(EncryptionHelper.Decrypt(AccountId));
      _slotController.CancelSlot(AccountIdInt, SlotId);
    }
    catch (InvalidDataException e)
    {
      return new JsonResult(e.Message);
    }

    return new JsonResult("Slot cancelled successfuly.");
  }

  [HttpPut("slots", Name = "UpdateSlot")]
  public JsonResult UpdateSlot(String AccountId, int SlotId, [FromBody] string StartTime)
  {
    try
    {
      int AccountIdInt = Int32.Parse(EncryptionHelper.Decrypt(AccountId));
      _slotController.UpdateSlot(AccountIdInt, SlotId, StartTime);
    }
    catch (InvalidDataException e)
    {
      return new JsonResult(e.Message);
    }

    return new JsonResult("Slot updated successfuly.");
  }

  [HttpGet("empty-slots", Name = "GetAvailableSlots")]
  public JsonResult GetAvailableSlots([FromQuery] int id)
  {
    try
    {
      var results = _slotController.GetAvailableSlots(id);
      if (results.Count == 0)
      {
        return new JsonResult("You have no open slots.");
      }
      return new JsonResult(results);
    }
    catch (InvalidDataException e)
    {
      return new JsonResult(e.Message);
    }
  }

  [HttpGet("notifications", Name = "GetNotifications")]
  public JsonResult GetNotifications([FromQuery] String id)
  {
    try
    {
      var idInt = Int32.Parse(EncryptionHelper.Decrypt(id));
      var results = new RabbitMqSub(idInt.ToString())._message;
      if (results == "")
      {
        return new JsonResult("You have no notifications.");
      }
      return new JsonResult(results);
    }
    catch (InvalidDataException e)
    {
      return new JsonResult(e.Message);
    }
  }

}
