using InterviewTracker.Data;
using InterviewTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Pages.Interviews;

public class CreateModel(AppDbContext db) : PageModel
{
    [BindProperty] public Guid ApplicationId { get; set; }
    [BindProperty] public InterviewType Type { get; set; } = InterviewType.TechCoding;
    [BindProperty] public InterviewStage Stage { get; set; } = InterviewStage.Round1;
    [BindProperty] public DateTime? Date { get; set; }
    [BindProperty] public int? DurationMin { get; set; }
    [BindProperty] public string? Outcome { get; set; }
    [BindProperty] public string? Notes { get; set; }

    public List<(Guid Id, string Label)> Apps { get; set; } = [];
    public IEnumerable<SelectListItem> ApplicationOptions { get; set; } // Add this property to fix CS1061
    public IEnumerable<SelectListItem> TypeOptions { get; set; } = [];
    public IEnumerable<SelectListItem> StageOptions { get; set; } = [];

    public async Task<IActionResult> OnGet()
    {
        Apps = await GetApps();

        ApplicationOptions = Apps.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = a.Label
        }).ToList();

        // Populează TypeOptions și StageOptions din enum
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
        if (ApplicationId == Guid.Empty)
        {
            ModelState.AddModelError("", "Selectează Application.");
            Apps = await GetApps();
            return Page();
        }

        var i = new Interview
        {
            ApplicationId = ApplicationId,
            Type = Type,
            Stage = Stage,
            Date = Date,
            DurationMin = DurationMin,
            Outcome = Outcome,
            Notes = Notes
        };

        db.Interviews.Add(i);
        await db.SaveChangesAsync();
        return RedirectToPage("Index");
    }

    private async Task<List<(Guid, string)>> GetApps()
    {
        return await db.Applications.Include(a => a.Company)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new ValueTuple<Guid, string>(a.Id, $"{a.Company.Name} — {a.Role}"))
            .ToListAsync();
    }
}
