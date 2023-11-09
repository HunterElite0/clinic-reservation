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
    public JsonResult GetSlots(int DoctorId)
    {
        var results = _slotController.GetDoctorSlots(DoctorId);

        if (results.Count == 0)
        {
            return new JsonResult("You have no open slots.");
        }
        return new JsonResult(results);
    }

    [HttpPost("slots", Name = "AddSlot")]
    public JsonResult AddSlot(int DoctorId, string StartDate)
    {
        try
        {
            _slotController.AddSlot(DoctorId, StartDate);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        };

        return new JsonResult("Slot added successfuly.");
    }

    [HttpDelete("slots", Name = "UpdateSlot")]
    public JsonResult CancelSlot(int DoctorId, int SlotId)
    {
        try
        {
            _slotController.CancelSlot(DoctorId, SlotId);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        }

        return new JsonResult("Slot cancelled successfuly.");
    }

    [HttpPut("slots", Name = "UpdateSlot")]
    public JsonResult UpdateSlot(int DoctorId, int SlotId, string StartTime)
    {
        try
        {
            _slotController.UpdateSlot(DoctorId, SlotId, StartTime);
        }
        catch (InvalidDataException e)
        {
            return new JsonResult(e.Message);
        }

        return new JsonResult("Slot updated successfuly.");
    }

}
