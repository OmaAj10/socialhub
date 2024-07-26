using Microsoft.AspNetCore.Mvc;
using Social_DataAccess.Repository.IRepository;
using Social_Models;

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
        var getAllActivities = await _unitOfWork.Activity.GetAll();
        return View(getAllActivities);
    }
    
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        Activity activityFromDb = await _unitOfWork.Activity.Get(a => a.Id == id);

        if (activityFromDb == null)
        {
            return NotFound();
        }

        await _unitOfWork.Activity.Delete(activityFromDb);
        await _unitOfWork.Save();
        TempData["success"] = "Ditt event har tagits bort!";
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

        return View(activityFromDb);
    }
}