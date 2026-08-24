using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Docly.Components;
using Docly.Data;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var encKey = builder.Configuration["EncryptionSettings:Key"];
var encIv = builder.Configuration["EncryptionSettings:Iv"];
Docly.Helpers.EncryptionHelper.Initialize(encKey, encIv);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=docly.db"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/";
    options.AccessDeniedPath = "/";
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<BookingService>();
// builder.Services.AddSingleton<ChatNotifierService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/auth/login", async (
    HttpContext context, 
    SignInManager<ApplicationUser> signInManager, 
    UserManager<ApplicationUser> userManager,
    [FromForm] string email, 
    [FromForm] string password) =>
{
    var result = await signInManager.PasswordSignInAsync(email, password, isPersistent: true, lockoutOnFailure: false);
    
    if (result.Succeeded)
    {
        return Results.Redirect("/");
    }

    return Results.Redirect("/login?error=true");
});

app.MapPost("/api/auth/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Redirect("/");
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/api/autologin", async (
    [FromForm] string email, 
    [FromForm] string password, 
    SignInManager<ApplicationUser> signInManager) =>
{
    var result = await signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
    
    if (result.Succeeded)
    {
        return Results.Redirect("/home");
    }
    
    return Results.Redirect("/register");
}).DisableAntiforgery();

app.Run();