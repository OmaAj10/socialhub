using Microsoft.AspNetCore.Mvc;
using Social_DataAccess.Repository.IRepository;
using Social_Models.Dto;

namespace Social_MVC.Controllers;
public class ParticipantController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ParticipantController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IActionResult> Index(int id)
    {
        var participants = await _unitOfWork.ApplicationUserActivity.GetAll(
            u => u.ActivityId == id,
            includeProperties: "ApplicationUser"
        );
    
        var participantDtos = participants.Select(p => new ApplicationUserDto()
        {
            FullName = p.ApplicationUser.FullName,
            BirthDate = p.ApplicationUser.BirthDate,
            Email = p.ApplicationUser.Email
            
        }).ToList();
    
        return View(participantDtos);
    }
}