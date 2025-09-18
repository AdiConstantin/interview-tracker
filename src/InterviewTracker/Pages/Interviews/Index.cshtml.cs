using InterviewTracker.Data;
using InterviewTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Pages.Interviews;

public class IndexModel(AppDbContext db) : PageModel
{
    public List<Interview> Items { get; set; } = [];

    public bool Obfuscate { get; set; }
    public bool ShowDeleted { get; set; }

    public record AppOption(Guid Id, string Label);
    public List<AppOption> AppOptions { get; set; } = [];

    public Dictionary<string, List<Interview>> GroupedByCompany { get; set; } = [];

    public async Task OnGet(bool? obfuscate, bool? showDeleted)
    {
        Obfuscate = obfuscate ?? false;
        ShowDeleted = showDeleted ?? false;

        var query = db.Interviews
            .Include(i => i.Application).ThenInclude(a => a.Company)
            .AsQueryable();

        if (!ShowDeleted)
            query = query.Where(i => !i.IsDeleted && !i.Application!.IsDeleted);

        Items = await query
            .OrderByDescending(i => i.Date)
            .ToListAsync();

        var appsQuery = db.Applications.Include(a => a.Company).AsQueryable();
        if (!ShowDeleted) appsQuery = appsQuery.Where(a => !a.IsDeleted);

        AppOptions = await appsQuery
            .OrderBy(a => a.Company.Name)
            .ThenBy(a => a.Role)
            .Select(a => new AppOption(a.Id, a.Company.Name + " — " + a.Role))
            .ToListAsync();

        GroupedByCompany = Items
            .Where(i => i.Application?.Company != null)
            .GroupBy(i => i.Application!.Company.Name)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    public async Task<IActionResult> OnPostDelete(Guid id, bool showDeleted)
    {
        var item = await db.Interviews.FindAsync(id);
        if (item == null) return NotFound();
        item.IsDeleted = true;
        await db.SaveChangesAsync();
        return RedirectToPage(new { showDeleted });
    }

    public async Task<IActionResult> OnPostRestore(Guid id, bool showDeleted)
    {
        var item = await db.Interviews.FindAsync(id);
        if (item == null) return NotFound();
        item.IsDeleted = false;
        await db.SaveChangesAsync();
        return RedirectToPage(new { showDeleted });
    }
}
