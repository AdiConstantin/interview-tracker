using InterviewTracker.Data;
using InterviewTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Pages.Applications;

public class IndexModel(AppDbContext db) : PageModel
{
    public List<Application> Applications { get; set; } = [];

    public bool Obfuscate { get; set; }
    public bool ShowDeleted { get; set; }


    public int TotalCompanies { get; set; }
    public int RejectedCount { get; set; }

    public async Task OnGet(bool? obfuscate)
    {
        Obfuscate = obfuscate ?? false;

        var apps = await db.Applications
        .Include(a => a.Company)
        .Include(a => a.Interviews)
        .ToListAsync();

        Applications = apps
            .OrderByDescending(a =>
                a.Interviews
                    .Where(i => i.Date.HasValue)
                    .OrderBy(i => i.Date)
                    .Select(i => i.Date ?? a.CreatedAt)
                    .FirstOrDefault(a.CreatedAt)
            )
            .ThenBy(a => a.Company.IsInactive)
            .ToList();

        TotalCompanies = Applications
            .Where(a => a.Company != null)
            .Select(a => a.Company.Id)
            .Distinct()
            .Count();

        RejectedCount = Applications
            .Count(a => a.Company.IsInactive == true);
    }

    public async Task<IActionResult> OnPostDelete(Guid id, bool showDeleted)
    {
        var app = await db.Applications.FindAsync(id);
        if (app == null) return NotFound();
        app.IsDeleted = true;
        await db.SaveChangesAsync();
        return RedirectToPage(new { showDeleted });
    }

    public async Task<IActionResult> OnPostRestore(Guid id, bool showDeleted)
    {
        var app = await db.Applications.FindAsync(id);
        if (app == null) return NotFound();
        app.IsDeleted = false;
        await db.SaveChangesAsync();
        return RedirectToPage(new { showDeleted });
    }
}
