global using OSP.Common.Domain.References;
global using OSP.Common.Domain.Tables;
global using OSP.Common.Domain.View;
global using OSP.SPASv2.Domain.References;
global using OSP.SPASv2.Domain.Tables;
global using OSP.SPASv2.Domain.View;
global using OSP.SPASv2.Domain.Params;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Web.Areas.Identity.Data;
using OSP.SPASv2.Web.Data;
using OSP.SPASv2.Web.Middleware.ErrorLoggerModel;
using System.Configuration;
using OSP.SPASv2.Web.Models;
using Microsoft.Build.Tasks;
using Microsoft.AspNet.Identity;
using OSP.SPASv2.Web.Utility;
using Microsoft.Extensions.DependencyInjection;
using OSP.SPASv2.Web.APIFactory.SPASv2Repo;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("OSPSPASv2DBContextConnection") ?? throw new InvalidOperationException("Connection string 'OSPSPASv2DBContextConnection' not found.");

builder.Services.AddDbContext<OSPSPASv2DBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<OSPSPASv2ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<OSPSPASv2DBContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireUppercase = false;
});
var configuration = builder.Configuration;
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

configuration
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
.AddJsonFile($"appsettings.{env}.json", true, true);

builder.Services.Configure<Config>(configuration.GetSection("Config"));


builder.Services.AddScoped<IHttpClientServiceImplementation, HttpClientFactoryService>();
builder.Services.AddScoped<IHTTPSPASv2Repo, HTTPSPASv2Repo>();


builder.Services.AddHttpClient("OSPServiceClient", config =>
{
    config.BaseAddress = new Uri(configuration["APIBaseURLCommon:Common.Service"]);
    //config.Timeout = new TimeSpan(0, 0, 30);
    config.DefaultRequestHeaders.Clear();
});



//controller to service
builder.Services.AddTransient<OSP.SPASv2.Web.Controllers.SampleController, OSP.SPASv2.Web.Controllers.SampleController>();
builder.Services.AddTransient<SPASv2.Controllers.HomeController, SPASv2.Controllers.HomeController>();


//builder.Services.AddHttpClient("SPASv2Api", config => config.BaseAddress = new Uri($"http://192.168.23.185/SPASv2Repo/api"));

//builder.Services.AddHttpClient("NoAutomaticCookies")
//    .ConfigurePrimaryHttpMessageHandler(() =>
//        new HttpClientHandler
//        {
//            UseCookies = false
//        });

//builder.Services.AddDbContext<SPASv2Context>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SPASv2Context"));
//});

////var client = new EchoServiceClient(EchoServiceClient.EndpointConfiguration.BasicHttpBinding_IEchoService, "http://localhost:5000/SampleWaService/BasicHttp");

//builder.Services.AddDbContext<SPASv2Context>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("EFSQLConnection"));
//});
//builder.Services.AddScoped<IRepositoryUnit, Repository.MainRepository.RepositoryUnit>();

//builder.Services.Configure<IISServerOptions>(options =>
//{
//    options.MaxRequestBodySize = int.MaxValue; // or your desired value
//});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Home/Error");
    //app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseDeveloperExceptionPage();
app.UseDeveloperExceptionPage();
//app.UseDatabaseErrorPage();

app.UseErrorLogger();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
pattern: "{controller=Home}/{action=DashBoard}/{id?}");
//pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = 
//        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var roles = new[] { "SPASV2-Requester", "SPASV2-Verifier", "SPASV2-Approver" };

//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }
//}

//using (var scope = app.Services.CreateScope())
//{
//    var userManager =
//        scope.ServiceProvider.GetRequiredService<UserManager<OSPSPASv2ApplicationUser>>();

//    OSPSPASv2ApplicationUser user = await userManager.FindByIdAsync("PISPLPI93448");

//    await userManager.AddToRoleAsync(user, "SPASV2-Requester");

//    user = await userManager.FindByIdAsync("PISPLPI18194");

//    await userManager.AddToRoleAsync(user, "SPASV2-Verifier");

//    user = await userManager.FindByIdAsync("PISPLPI06141");

//    await userManager.AddToRoleAsync(user, "SPASV2-Approver");

//    user = await userManager.FindByIdAsync("PISPLPI10251");

//    await userManager.AddToRoleAsync(user, "SPASV2-Approver");

//}


//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<OSPSPASv2ApplicationUser>>();
//    OSPSPASv2ApplicationUser user = await userManager.FindByIdAsync("PISPLPI23094");
//    var result = await userManager.ChangePasswordAsync(user, "J010195e_", "Superjemc26_");
//}



app.Run();
