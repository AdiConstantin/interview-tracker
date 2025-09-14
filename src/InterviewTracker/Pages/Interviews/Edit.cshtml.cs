using InterviewTracker.Data;
using InterviewTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Pages.Interviews;

public class EditModel(AppDbContext db) : PageModel
{
    [BindProperty] public Interview Interview { get; set; } = default!;
    public IEnumerable<SelectListItem> TypeOptions { get; set; } = [];
    public IEnumerable<SelectListItem> StageOptions { get; set; } = [];

    public async Task<IActionResult> OnGet(Guid id)
    {
        Interview = await db.Interviews
            .Include(i => i.Application).ThenInclude(a => a.Company)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (Interview == null)
            return NotFound();

        TypeOptions = Enum.GetValues(typeof(InterviewType))
            .Cast<InterviewType>()
            .Select(e => new SelectListItem { Value = e.ToString(), Text = e.ToString() })
            .ToList();

        StageOptions = Enum.GetValues(typeof(InterviewStage))
            .Cast<InterviewStage>()
            .Select(e => new SelectListItem { Value = e.ToString(), Text = e.ToString() })
            .ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        Interview.Application = await db.Applications
            .Include(a => a.Company)
            .FirstOrDefaultAsync(a => a.Id == Interview.ApplicationId);

        db.Attach(Interview).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}