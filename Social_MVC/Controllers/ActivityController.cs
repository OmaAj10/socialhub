using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_DataAccess.Repository.IRepository;
using Social_Models;
using Social_Models.ViweModels;
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
        var getAllActivities = await _unitOfWork.Activity.GetAll(includeProperties: "ApplicationUserActivities.ApplicationUser");
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        ViewData["UserEmail"] = userEmail;

        var activityViewModels = getAllActivities.Select(activity => new ActivityModel
        {
            Activity = activity,
            isJoinedActivity = activity.ApplicationUserActivities?.Any(a => a.ApplicationUser.Email == userEmail) ?? false
        }).ToList();
   
        return View(activityViewModels);
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
                FullName = activityModel.CreatedBy,
                BirthDate = activityModel.BirthDate,
                Email = activityModel.Email,
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
        
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var isAdmin = User.IsInRole("Admin");
        var isCreator = activityFromDb.Email == userEmail;
    
        if (!isAdmin && !isCreator)
        {
            return Forbid();
        }
    
        ViewData["UserEmail"] = userEmail;
        ViewData["CreatorEmail"] = activityFromDb.Email;
        ViewData["IsAdmin"] = isAdmin;
        ViewData["IsCreator"] = isCreator;
    
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
    
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var isAdmin = User.IsInRole("Admin");
            var isCreator = activityFromDb.Email == userEmail;
            
            if (!isAdmin && !isCreator)
            {
                return Forbid();
            }
    
            activityFromDb.Title = activity.Title;
            activityFromDb.CreatedBy = activity.CreatedBy;
            activityFromDb.BirthDate = activity.BirthDate;
            activityFromDb.Email = activity.Email;
            activityFromDb.City = activity.City;
            activityFromDb.Address = activity.Address;
            activityFromDb.Date = activity.Date;
    
            var applicationUserActivityFromDb = await _unitOfWork.ApplicationUserActivity.Get(p => p.ActivityId == activityFromDb.Id);
    
            if (applicationUserActivityFromDb != null)
            {
                var applicationUserFromDB =
                    await _unitOfWork.ApplicationUser.Get(u => u.Id == applicationUserActivityFromDb.ApplicationUserId);
    
                if (applicationUserFromDB != null)
                {
                    applicationUserFromDB.FullName = activityFromDb.CreatedBy;
                }
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
    public async Task<IActionResult> Join(int id)
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

        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var applicationUser = await _unitOfWork.ApplicationUser.Get(u => u.Email == userEmail && u.Id == userId);
        
        if (applicationUser == null)
        {
            return NotFound();
        }
        
        var existingUserActivity = await _unitOfWork.ApplicationUserActivity.Get(u =>
            u.ApplicationUserId == applicationUser.Id && u.ActivityId == id);

        if (existingUserActivity != null)
        {
            TempData["error"] = "Du har redan gått med i detta event!";
            return RedirectToAction("Index");
        }


        var applicationUserActivity = new ApplicationUserActivity
        {
            ApplicationUserId = applicationUser.Id,
            ActivityId = activityFromDb.Id
        };

        await _unitOfWork.ApplicationUserActivity.Create(applicationUserActivity);
        await _unitOfWork.Save();
        TempData["success"] = "Du har gått med ett event!";

        return RedirectToAction("Index");
    }

    public async Task<bool> IsHasJoinedEvent(int eventId)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var activity = _unitOfWork.Activity.Get(a => a.Id == eventId).Result;
        var isUserJoined = activity?.ApplicationUserActivities?.Any(a => a.ApplicationUser.Email == userEmail) ?? false;

        return isUserJoined;
    }
    
    [HttpPost, ActionName("LeaveEvent")]
    public async Task<IActionResult> LeaveEvent(int id)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var applicationUser = await _unitOfWork.ApplicationUser.Get(u => u.Email == userEmail && u.Id == userId);

        if (applicationUser == null)
        {
            return NotFound();
        }

        var userActivity = await _unitOfWork.ApplicationUserActivity.Get(u =>
            u.ApplicationUserId == applicationUser.Id && u.ActivityId == id);
        
        if (userActivity == null)
        {
            return NotFound();
        }

        var activity = await _unitOfWork.Activity.Get(a => a.Id == id);
        
        if (activity == null)
        {
            return NotFound();
        }
        
        await _unitOfWork.ApplicationUserActivity.Delete(userActivity);
        await _unitOfWork.Save();
        TempData["success"] = "Du har lämnat eventet!";

        return RedirectToAction("Index");
    }
}