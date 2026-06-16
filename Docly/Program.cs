using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Docly.Components;
using Docly.Data;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=docly.db"));

// 1. CONFIGURAZIONE IDENTITY E RUOLI
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

var app = builder.Build();

// 2. MODIFICA DEL SEEDER PER PASSARE IL SERVICE PROVIDER
using (var scope = app.Services.CreateScope())
{
    // Passiamo l'intero ServiceProvider al Seeder per potergli far usare RoleManager e UserManager
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Importante per CSS/JS
app.UseAntiforgery();

// 3. MIDDLEWARE DI AUTENTICAZIONE E AUTORIZZAZIONE
app.UseAuthentication();
app.UseAuthorization();

// 4. ENDPOINT INVISIBILE PER IL LOGIN (Aggira il limite di Blazor Server sui cookie)
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
        // Se il login ha successo, andiamo sempre alla Home!
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

app.Run();