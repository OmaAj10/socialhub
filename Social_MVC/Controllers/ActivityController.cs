using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_DataAccess.Repository.IRepository;
using Social_Models;
using Social_Utility;

namespace Social_MVC.Controllers;

public class ActivityController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ActivityController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IActionResult> Index()
    {
        var getAllActivities = await _unitOfWork.Activity.GetAll(includeProperties: "ApplicationUsers");
        return View(getAllActivities);
    }
    
      
    [Authorize(Roles = RoleDetails.Role_Company + "," + RoleDetails.Role_Admin + "," + RoleDetails.Role_User)]
    [HttpPost, ActionName("CreateEvent")]
    public async Task<IActionResult> CreateEvent(Activity activityModel)
    {
        if (activityModel == null)
        {
            ModelState.AddModelError("", "Något gick fel");
        }

        if (ModelState.IsValid)
        {
            activityModel = await _unitOfWork.Activity.Create(activityModel);
            await _unitOfWork.Save();

            var applicationUser = new ApplicationUser()
            {
                Name = activityModel.CreatedBy,
                BirthDate = activityModel.BirthDate,
                Email = activityModel.Email,
                Activity = activityModel,
                ActivityId = activityModel.Id
            };

            await _unitOfWork.ApplicationUser.Create(applicationUser);
            await _unitOfWork.Save();
            TempData["success"] = "Ditt event har skapats";
            return RedirectToAction(nameof(Index));
        }

        return View();
    }
    
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        Activity activityFromDb = await _unitOfWork.Activity.Get(a => a.Id == id, includeProperties: "ApplicationUsers");

        if (activityFromDb == null)
        {
            return NotFound();
        }

        if (activityFromDb.ApplicationUsers != null)
        {
            foreach (var user in activityFromDb.ApplicationUsers.ToList())
            { 
                await _unitOfWork.ApplicationUser.Delete(user);
            }

            await _unitOfWork.Save();
        }

        await _unitOfWork.Activity.Delete(activityFromDb);
        await _unitOfWork.Save();
        TempData["success"] = "Eventet har tagits bort!";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        Activity? activityFromDb;
        try
        {
            activityFromDb = await _unitOfWork.Activity.Get(a => a.Id == id);
        }
        catch (Exception e)
        {
            return NotFound();
        }

        return View(activityFromDb);
    }
    
    [HttpPost, ActionName("Edit")]
    public async Task<IActionResult> Edit(Activity activity)
    {
        if (ModelState.IsValid)
        {
            var activityFromDb = await _unitOfWork.Activity.Get(a => a.Id == activity.Id);

            if (activityFromDb == null)
            {
                return NotFound();
            }

            activityFromDb.Title = activity.Title;
            activityFromDb.CreatedBy = activity.CreatedBy;
            activityFromDb.BirthDate = activity.BirthDate;
            activityFromDb.Email = activity.Email;
            activityFromDb.City = activity.City;
            activityFromDb.Address = activity.Address;
            activityFromDb.Date = activity.Date;

            var participantFromDb = await _unitOfWork.ApplicationUser.Get(p => p.ActivityId == activityFromDb.Id);

            if (participantFromDb != null)
            {
                participantFromDb.Name = activity.CreatedBy;
            }

            await _unitOfWork.Activity.Update(activityFromDb);
            await _unitOfWork.Save();
            TempData["success"] = "Ditt event har updaterats!";
            return RedirectToAction("Index");
        }

        return View(activity);
    }

    public async Task<IActionResult> Join(int? id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        Activity? activityFromDb = await _unitOfWork.Activity.Get(a => a.Id == id);

        if (activityFromDb == null)
        {
            return NotFound();
        }
        
        ViewData["UserEmail"] = User.FindFirstValue(ClaimTypes.Email);

        return View(activityFromDb);
    }

    [HttpPost, ActionName("Join")]
    public async Task<IActionResult> Join(int id, string applicationUserName, DateTime applicationUserDate, string applicationUserEmail)
    {
        if (id == 0)
        {
            return NotFound();
        }
        
        Activity? activityFromDb = await _unitOfWork.Activity.Get(a => a.Id == id);

        if (activityFromDb == null)
        {
            return NotFound();
        }

        var existingUser = await _unitOfWork.ApplicationUser.Get(u =>
            u.Name == applicationUserName && u.Email == applicationUserEmail && u.ActivityId == id);

        if (existingUser != null)
        {
            TempData["danger"] = "Du har redan gått med i detta event!";
            return RedirectToAction("Index");
        }

        var applicationUser = new ApplicationUser
        {
            Name = applicationUserName,
            BirthDate = applicationUserDate,
            Email = applicationUserEmail,
            Activity = activityFromDb,
            ActivityId = activityFromDb.Id
        };

        await _unitOfWork.ApplicationUser.Create(applicationUser);
        await _unitOfWork.Save();
        TempData["success"] = "Du har gått med ett event!";

        return RedirectToAction("Index");
    }
}