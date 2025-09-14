using InterviewTracker.Data;
using InterviewTracker.Models;
using InterviewTracker.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Migrations auto-applied on startup (dev-friendly)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// Minimal APIs (exemple)
app.MapGet("/api/applications", async (AppDbContext db) =>
    await db.Applications.Include(a => a.Company).OrderByDescending(a => a.CreatedAt).ToListAsync());

app.MapPost("/api/applications", async (AppDbContext db, Application a) =>
{
    // attach Company if existing by name
    if (a.Company is not null && !string.IsNullOrWhiteSpace(a.Company.Name))
    {
        var existing = await db.Companies.FirstOrDefaultAsync(c => c.Name == a.Company.Name);
        if (existing is not null) a.Company = existing;
    }

    db.Applications.Add(a);
    await db.SaveChangesAsync();
    return Results.Created($"/api/applications/{a.Id}", a);
});

app.MapGet("/api/applications/{id:guid}/interviews", async (AppDbContext db, Guid id) =>
    await db.Interviews.Where(i => i.ApplicationId == id).OrderBy(i => i.Date).ToListAsync());

app.MapPost("/api/interviews", async (AppDbContext db, Interview i) =>
{
    db.Interviews.Add(i);
    await db.SaveChangesAsync();
    return Results.Created($"/api/interviews/{i.Id}", i);
});

// Insights simple: top topics
app.MapGet("/api/insights/top-topics", async (AppDbContext db) =>
    await db.TechTopics.Select(t => new { t.Name, Count = t.Questions.Count })
        .OrderByDescending(x => x.Count).Take(20).ToListAsync());

// Dev seed (idempotent-ish)
app.MapPost("/dev/seed", async (AppDbContext db) =>
{
    await SeedData.EnsureSeedAsync(db);
    return Results.Ok(new { status = "ok" });
});

// Razor Pages
app.MapRazorPages();

app.UseStaticFiles();

app.Run();
