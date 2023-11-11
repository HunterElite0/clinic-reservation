using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

namespace clinic_reservation;
[ApiController]
[Route("[controller]")]
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
    public JsonResult GetSlots([FromQuery] int id)
    {
            try
            {
                var results = _slotController.GetDoctorSlots(id);
                if (results.Count == 0)
                {
                    return new JsonResult("You have no open slots.");
                }
                return new JsonResult(results);
            }
            catch(InvalidDataException e)
            {
                return new JsonResult(e.Message);    
        }
    }

    [HttpPost("slots", Name = "AddSlot")]
    public JsonResult AddSlot(int AccountId, string StartDate)
    {
        try
        {
            _slotController.AddSlot(AccountId, StartDate);
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
    public JsonResult CancelSlot(int AccountId, int SlotId)
    {
        try
        {
            _slotController.CancelSlot(AccountId, SlotId);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        }

        return new JsonResult("Slot cancelled successfuly.");
    }

    [HttpPut("slots", Name = "UpdateSlot")]
    public JsonResult UpdateSlot(int AccountId, int SlotId, string StartTime)
    {
        try
        {
            _slotController.UpdateSlot(AccountId, SlotId, StartTime);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        }

        return new JsonResult("Slot updated successfuly.");
    }

}
