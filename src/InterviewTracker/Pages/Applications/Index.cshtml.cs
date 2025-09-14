using System.Collections.Generic;
using InterviewTracker.Data;
using InterviewTracker.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Pages.Applications;

public class IndexModel(AppDbContext db) : PageModel
{
    public List<Application> Applications { get; set; } = [];

    public async Task OnGet()
    {
        Applications = await db.Applications
            .Include(a => a.Company)
            .OrderByDescending(a => a.CreatedAt)
            .OrderBy(i => i.Company.IsInactive)
            .ToListAsync();
    }
}
