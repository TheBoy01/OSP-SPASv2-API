global using OSP.Common.Domain.References;
global using OSP.Common.Domain.Tables;
global using OSP.Common.Domain.View;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OSP.Common.Service;
using OSP.Common.Service.APIRepository.Service;
//global using OSP.SPASv2.Domain.References;
//global using OSP.SPASv2.Domain.Tables;
//global using OSP.SPASv2.Domain.View;

using OSP.Common.Service.Middleware;
using OSP.Common.Service.Middleware.ErrorLoggerModel;
using OSP.Common.Service.OperationContract;
using OSP.Common.Service.ServiceContract;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.Configure<IPWhitelistOptions>(builder.Configuration.GetSection("IPWhitelistOptions"));


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

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<ISendEmailService<TblSendEmail>,SendEmailService >();

builder.Services.Configure<SmtpClientSettings>(builder.Configuration.GetSection("SmtpClient"));



builder.Services.AddSingleton<JWTAuthenticationManager>(new JWTAuthenticationManager(key));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://hris.stpeter.com.ph:89", "https://epps.stpeter.com.ph:55", "http://192.168.1.6:56",
            "http://localhost:5173", "https://localhost:7012", 
            "https://localhost:7234", "https://192.168.23.143:8000", "http://192.168.23.143:8000", 
            "http://127.0.0.1:8081", "http://192.168.23.25", "http://192.168.23.185", "http://192.168.1.6")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials() //added by dave
               .SetIsOriginAllowed(origin => true); //added by dave
               //.AllowAnyOrigin(); //removed by dave
    });
});

builder.Services.AddHttpClient("OSPServiceClient", config =>
{
    config.BaseAddress = new Uri(configuration["APIBaseURLCommon:Common.Service"]);
    //config.Timeout = new TimeSpan(0, 0, 30);
    config.DefaultRequestHeaders.Clear();
});

builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddHostedService<EmailSenderService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();
app.UseErrorLogger();
//app.UseIPWhitelist();
app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
