using InterviewTracker.Data;
using InterviewTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Pages.Applications;

public class EditModel(AppDbContext db) : PageModel
{
    [BindProperty] public Application Application { get; set; } = default!;

    public async Task<IActionResult> OnGet(Guid id)
    {
        Application = await db.Applications
            .Include(a => a.Company)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (Application == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var app = await db.Applications.FindAsync(Application.Id);
        if (app != null)
        {
            var company = await db.Companies.FindAsync(app.CompanyId);
            if (company != null)
            {
                company.Name = Application.Company.Name;
                company.Location = Application.Company.Location;
                company.IsInactive = Application.Company.IsInactive;
            }

            app.Company  = company ?? app.Company;

            app.Role = Application.Role;
            app.Seniority = Application.Seniority;
            app.Source = Application.Source;
            app.Status = Application.Status;
            app.SalaryExpectation = Application.SalaryExpectation;
        }

        await db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}