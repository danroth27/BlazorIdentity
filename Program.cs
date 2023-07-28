using BlazorIdentity;
using BlazorIdentity.Areas.Identity.Data;
using BlazorIdentity.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("BlazorIdentityContextConnection") ??
    throw new InvalidOperationException("Connection string 'BlazorIdentityContextConnection' not found.");

builder.Services.AddDbContext<BlazorIdentityContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<BlazorIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BlazorIdentityContext>();

builder.Services.AddRazorPages();

builder.Services.AddRazorComponents();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
// Need way to access HttpContext from components: https://github.com/dotnet/aspnetcore/issues/48769
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapRazorComponents<App>();
app.MapRazorPages();

app.Run();
