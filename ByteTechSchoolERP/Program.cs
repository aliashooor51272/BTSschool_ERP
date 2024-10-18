using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IHostel;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ISubject;
using ByteTechSchoolERP.DataAccess.Repository.IRepository;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.DataAccess.Repository.Services.HostelService;
using ByteTechSchoolERP.DataAccess.Repository.Services.SubjectService;
using ByteTechSchoolERP.DataAccess.Repository.UnitOfWork;
using ByteTechSchoolERP.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ByteTechSchoolERPContextConnection")
    ?? throw new InvalidOperationException("Connection string 'ByteTechSchoolERPContextConnection' not found.");

// Set the license context for EPPlus
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Configure DbContext
builder.Services.AddDbContext<ByteTechSchoolERPContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity
builder.Services.AddDefaultIdentity<ByteTechSchoolERPUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ByteTechSchoolERPContext>();

// Configure Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IHostelRoomTypeService, HostelRoomTypeService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<AuthenticationFilter>();

// Configure MVC and Razor Pages
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation().AddRazorPagesOptions(options =>
{
    options.Conventions.AddAreaPageRoute("Admin", "/Dashboard", "/Index");
});

// Configure Session
builder.Services.AddSession(options =>
    options.IdleTimeout = TimeSpan.FromMinutes(60));

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedAdminUserAndRole(services);
 
 
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Configure Session middleware
app.UseSession();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "adminArea",
    pattern: "/Admin/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

app.MapControllerRoute(
    name: "default",
    pattern: "{area=AccessControl}/{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages();
app.Run();
