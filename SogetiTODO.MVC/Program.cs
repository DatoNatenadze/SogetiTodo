using Serilog;
using SogetiTODO.Infrastructure.Extensions;
using SogetiTODO.Infrastructure.Middlewares.GlobalExceptionHandling;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.InstallServicesFromAssembly(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler($"/Error/Index?errorCode={404}");
app.UseHsts();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();