using InterviewTracker.Data;
using InterviewTracker.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Pages.Interviews;

public class IndexModel(AppDbContext db) : PageModel
{
    public List<Interview> Items { get; set; } = [];

    public async Task OnGet()
    {
        Items = await db.Interviews
            .Include(i => i.Application).ThenInclude(a => a.Company)
            .OrderByDescending(i => i.Date)
            .ToListAsync();
    }
}
