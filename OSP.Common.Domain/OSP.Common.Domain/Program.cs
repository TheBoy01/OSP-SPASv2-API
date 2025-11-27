using OSP.Common.Domain.APIFactory.OSPService;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


//controller constructor
builder.Services.AddScoped<IHttpOSPService, HttpOSPService>();


//httpclient
builder.Services.AddHttpClient("OSPServiceClient", config =>
{
    config.BaseAddress = new Uri(configuration["APIBaseURLCommon:Common.Service"]);
    //config.Timeout = new TimeSpan(0, 0, 30);
    config.DefaultRequestHeaders.Clear();
});

var app = builder.Build();







app.MapGet("/", () => "Hello World!");

app.Run();
