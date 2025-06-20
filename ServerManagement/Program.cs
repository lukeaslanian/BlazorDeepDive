using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerManagement.Components;
using ServerManagement.Components.Account;
using ServerManagement.Data;
using ServerManagement.Models;
using ServerManagement.StateStore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Optimize: Use pooled DbContext factory for better performance with SQLite
builder.Services.AddPooledDbContextFactory<ServerManagementContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ServerManagement"));

    // Enable sensitive data logging only in development
    if (builder.Environment.IsDevelopment()) options.EnableSensitiveDataLogging();
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Manager", policy => policy.RequireClaim("Role", "General Manager"));
});

var connectionString = builder.Configuration.GetConnectionString("IdentityDatabase") ??
                       throw new InvalidOperationException("Connection string 'IdentityDatabase' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

//builder.Services.AddTransient<SessionStorage>();
builder.Services.AddScoped<ContainerStorage>();
builder.Services.AddScoped<TorontoOnlineServersStore>();
builder.Services.AddScoped<MontrealOnlineServersStore>();
builder.Services.AddScoped<OttawaOnlineServersStore>();
builder.Services.AddScoped<HalifaxOnlineServersStore>();
builder.Services.AddScoped<CalgaryOnlineServersStore>();

builder.Services.AddTransient<IServersEFCoreRepository, ServersEFCoreRepository>();

var app = builder.Build();

// Ensure databases are created
using (var scope = app.Services.CreateScope())
{
    var serverContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ServerManagementContext>>()
        .CreateDbContext();
    var identityContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        serverContext.Database.EnsureCreated();
        identityContext.Database.EnsureCreated();
        Console.WriteLine("Databases created successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database initialization error: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();