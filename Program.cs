using Microsoft.EntityFrameworkCore;
using MapYourMeal.DAL;
using MapYourMeal.Models;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionStrings:ApplicationDbContextConnection"]);
});

// Add this in the brackets if you want to RequireconfirmAccount options => options.SignIn.RequireConfirmedAccount = true
//builder.Services.AddDefaultIdentity<User>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    //Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 6;

//Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    //User settings
    options.User.RequireUniqueEmail = true;

    //Sign-in settings
    options.SignIn.RequireConfirmedAccount = false; // Set to true to enable email confirmation
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Ensure this path is valid
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie.Name = ".AdventureWorks.Identity";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
    options.Cookie.SameSite = SameSiteMode.Lax; // 
}
);
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();


builder.Services.AddRazorPages();
//builder.Services.AddSession();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(1200); // 20  mins
    options.Cookie.IsEssential = true;
}
);

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information() //levels: Trace < Information < Warning < Error < Fatal
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log");

loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) && 
                            e.Level == LogEventLevel.Information &&
                            e.MessageTemplate.Text.Contains("Executed DbCommand"));    

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Ensure the roles and users are seeded when the application starts
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        // Seed the database, including roles and users
        await DBInit.Seed(app, userManager, roleManager);
    }
    
}




app.UseStaticFiles();
app.UseSession(); // Must come before app.UseAuthentication()

// Middleware for debugging cookies and session
app.Use(async (context, next) =>
{
    if (context.Session.IsAvailable)
    {
        Console.WriteLine("Session is available.");
    }
    else
    {
        Console.WriteLine("Session is NOT available.");
    }

    // Check session keys and values
    foreach (var key in context.Session.Keys)
    {
        Console.WriteLine($"Session Key: {key}, Value: {context.Session.GetString(key)}");
    }

    await next();
});






app.UseAuthentication(); // Must come before app.UseAuthorization();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapRazorPages();
app.Run();