global using OSP.Common.Domain.References;
global using OSP.Common.Domain.Tables;
global using OSP.Common.Domain.View;

//global using OSP.SPASv2.Domain.View;
//global using OSP.SPASv2.Domain.References;
//global using OSP.SPASv2.Domain.Tables;


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OSP.Common.Domain.APIFactory.OSPService;
using OSP.Common.Repository;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.Middleware.ErrorLoggerModel; 
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<OSPContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("OSPContext"));
//});
 
builder.Services.AddDbContext<OSPContext>();

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


builder.Services.AddSingleton<JWTAuthenticationManager>(new JWTAuthenticationManager(key));
var configuration = builder.Configuration;

builder.Services.AddScoped<IHttpOSPService, HttpOSPService>();


builder.Services.AddHttpClient("OSPServiceClient", config =>
{
    config.BaseAddress = new Uri(configuration["APIBaseURLCommon:Common.Service"]);
    //config.Timeout = new TimeSpan(0, 0, 30);
    config.DefaultRequestHeaders.Clear();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Dictionary<string, string> connStrs = new Dictionary<string, string>();
//connStrs.Add("ONLINESERVER", builder.Configuration["ConnectionStrings:ONLINESERVERConnection"]);
connStrs.Add("SPLPDEVSERVER", builder.Configuration["ConnectionStrings:SPLPDEVSERVERConnection"]);
connStrs.Add("SPLPIS2", builder.Configuration["ConnectionStrings:SPLPIS2Connection"]);
DbContextFactory.SetConnectionString(connStrs);

app.UseErrorLogger();
//app.UseIPWhitelist();
app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
