using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using marsian_library.Data;   
using marsian_library.Models;
using marsian_library.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OracleDb");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString));


// ============== Serwisy =============///
// ........................................................
//  :   ,-.      ,-.      ,-.      ,-.      ,-.      ,-.   :
//  : _(*_*)_  _(*_*)_  _(*_*)_  _(*_*)_  _(*_*)_  _(*_*)_ :
//  :(_  o  _)(_  o  _)(_  o  _)(_  o  _)(_  o  _)(_  o  _):
//  :  / o \    / o \    / o \    / o \    / o \    / o \  :
//  : (_/ \_)  (_/ \_)  (_/ \_)  (_/ \_)  (_/ \_)  (_/ \_) :
//  :......................................................:

//Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.SignIn.RequireConfirmedAccount = false; 
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI(); 

//Jakieś wbudowane
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<marsian_library.Services.WeatherRaportService>();

//Serwisy odpowiadające MVC
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IGenreService, GenreService>();


var app = builder.Build();

//seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Opcja 1: Normalne seedowanie (tylko jeśli dane nie istnieją)
        //await DbInitializer.SeedAllAsync(services);
        
        // Opcja 2: Reset i reseed (usuwa wszystkie dane i dodaje od nowa)
        //await DbInitializer.ResetAndReseedAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
// ======================= Koniec serwisów ==================///

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();