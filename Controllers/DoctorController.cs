using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

namespace clinic_reservation;
[ApiController]
[Route("[controller]")]
public class DoctorController
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
            .Include(d => d.Slots.Count)
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

}
