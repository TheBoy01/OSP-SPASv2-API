global using OSP.SPASv2.Domain.Tables;
global using OSP.SPASv2.Domain.References;
global using OSP.SPASv2.Domain.View;
global using OSP.Common.Domain.Tables;
global using OSP.Common.Domain.References;
global using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OSP.SPASv2.Repository;
using OSP.SPASv2.Repository.Middleware.ErrorLoggerModel;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Repository.Context;
using OSP.SPASv2.Repository.IRepository;
using Microsoft.Extensions.Options;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SPASv2Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SPASv2Context"));
});

builder.Services.AddDbContext<SPASv1Context>();
//builder.Services.AddDbContext<SPASv1Context>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SPASv1Context"));

//});
//builder.Services.AddScoped<OSP.SPASv2.Repository.Repository.RepositoryUnit>();

builder.Services.Configure<IPWhitelistOptions>(builder.Configuration.GetSection("IPWhitelistOptions"));

var key = "kygmtest12345678";
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddTransient<OSP.SPASv2.Repository.Controllers.RonController, OSP.SPASv2.Repository.Controllers.RonController>();
builder.Services.AddTransient<OSP.SPASv2.Repository.Controllers.JonController, OSP.SPASv2.Repository.Controllers.JonController>();
builder.Services.AddTransient<OSP.SPASv2.Repository.Controllers.WaController, OSP.SPASv2.Repository.Controllers.WaController>();
builder.Services.AddTransient<OSP.SPASv2.Repository.Controllers.VendorController, OSP.SPASv2.Repository.Controllers.VendorController>();
builder.Services.AddTransient<OSP.SPASv2.Repository.Controllers.WarController, OSP.SPASv2.Repository.Controllers.WarController>();
builder.Services.AddTransient<OSP.SPASv2.Repository.Controllers.RudyController, OSP.SPASv2.Repository.Controllers.RudyController>();


builder.Services.AddSingleton<JWTAuthenticationManager>(new JWTAuthenticationManager(key));

//var services = new ServiceCollection();
//var configurationBuilder = new ConfigurationBuilder();
//configurationBuilder.AddJsonFile("appsettings.json");
//var configuration = configurationBuilder.Build();
//services.AddSingleton<IConfiguration>(configuration);


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Dictionary<string, string> connStrs = new Dictionary<string, string>();

connStrs.Add("TSPG", builder.Configuration["ConnectionStrings:SPASv1Context"]);
connStrs.Add("TFCMCI", builder.Configuration["ConnectionStrings:FCMCIConnection"]);
connStrs.Add("TFHFHS", builder.Configuration["ConnectionStrings:FHFHSConnection"]); 
connStrs.Add("TGGMCV", builder.Configuration["ConnectionStrings:GGMCVConnection"]);
connStrs.Add("TSPCLI", builder.Configuration["ConnectionStrings:SPCLIConnection"]);
connStrs.Add("TSPCMW", builder.Configuration["ConnectionStrings:SPCMWConnection"]);
connStrs.Add("TSPCNL", builder.Configuration["ConnectionStrings:SPCNLConnection"]);
connStrs.Add("TSPCSL", builder.Configuration["ConnectionStrings:SPCSLConnection"]);
connStrs.Add("TSPMCI", builder.Configuration["ConnectionStrings:SPMCIConnection"]);
connStrs.Add("TSPMHL", builder.Configuration["ConnectionStrings:SPMHLConnection"]);
connStrs.Add("TSPMHM", builder.Configuration["ConnectionStrings:SPMHMConnection"]);
connStrs.Add("TSPMHV", builder.Configuration["ConnectionStrings:SPMHVConnection"]);
DbContextFactory.SetConnectionString(connStrs);

app.UseErrorLogger();
//app.UseIPWhitelist();
app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
