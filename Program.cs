using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;

log4net.Config.XmlConfigurator.Configure();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var serviceName = "Example";

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddOpenTelemetry()
  .WithTracing(b =>
  {
      b
      .AddSource(serviceName)
      .AddHttpClientInstrumentation()
      .AddAspNetCoreInstrumentation();
//      .AddRedisInstrumentation(connection)
//      .AddConsoleExporter();
//      .AddOtlpExporter();
     
  });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

var httpClient = new HttpClient();
app.MapGet("/hello", async () =>
{
//    IDatabase db = connection.GetDatabase();
//    string value = "abcdefg";
//    db.StringSet("mykey", value);
//    string value2 = db.StringGet("mykey");
//    Console.WriteLine(value2); // writes: "abcdefg"
    var html = await httpClient.GetStringAsync("https://opentelemetry.io/");
    if (string.IsNullOrWhiteSpace(html))
    {
        return "Hello, World!";
    }
    else
    {
        return "Hello, World!";
    }
});

app.Run();
