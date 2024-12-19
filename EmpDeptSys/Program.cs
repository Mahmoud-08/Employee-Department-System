using EmpDeptSys.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); 

// Register the DbContext with Identity support
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add ASP.NET Core Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;  
    options.Password.RequireDigit = false;           
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;            
})
    .AddEntityFrameworkStores<AppDbContext>()         
    .AddDefaultTokenProviders();                     

// Configure the application cookie for login and logout
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";          
    options.LogoutPath = "/Identity/Account/Logout";    
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict; 
    options.Cookie.HttpOnly = true;                          
    options.SlidingExpiration = true;                        
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);     
    options.Cookie.IsEssential = true;                     
});


// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".EmpDeptSys.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();         
app.UseAuthentication();  
app.UseAuthorization();   

// Map Razor Pages (required for Identity features)
app.MapRazorPages();

// Add default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();