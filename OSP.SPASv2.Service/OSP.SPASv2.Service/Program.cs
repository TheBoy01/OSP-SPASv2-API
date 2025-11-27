global using OSP.SPASv2.Domain.Tables;


using OSP.SPASv2.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;
using System.Text;

using OSP.SPASv2.Service.OldServices;
using OSP.SPASv2.Service.Services;
using OSP.Common.Repository.Middleware.ErrorLoggerModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IServices, Services>();
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
//builder.Services.AddSingleton<JWTAuthenticationManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseIPWhitelist();
app.UseErrorLogger();
app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
