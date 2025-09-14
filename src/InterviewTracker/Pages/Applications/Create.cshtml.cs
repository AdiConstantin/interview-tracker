using InterviewTracker.Data;
using InterviewTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterviewTracker.Pages.Applications;

public class CreateModel(AppDbContext db) : PageModel
{
    [BindProperty] public string CompanyName { get; set; } = default!;
    [BindProperty] public string? CompanyLocation { get; set; }
    [BindProperty] public string Role { get; set; } = default!;
    [BindProperty] public string? Seniority { get; set; }
    [BindProperty] public string? Source { get; set; }
    [BindProperty] public decimal? SalaryExpectation { get; set; }
    [BindProperty] public bool IsInactive { get; set; }

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(CompanyName) || string.IsNullOrWhiteSpace(Role))
        {
            ModelState.AddModelError("", "CompanyName și Role sunt obligatorii.");
            return Page();
        }

        var existing = db.Companies.FirstOrDefault(c => c.Name == CompanyName);
        var company = existing ?? new Company { Name = CompanyName, Location = CompanyLocation, IsInactive = IsInactive };

        var app = new Application
        {
            Company = company,
            Role = Role,
            Seniority = Seniority,
            Source = Source,
            SalaryExpectation = SalaryExpectation
        };

        db.Applications.Add(app);
        await db.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
