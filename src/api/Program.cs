using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using LevanaTracker.Api.Common.Endpoint;
using LevanaTracker.Api.Common.Extensions;
using LevanaTracker.Api.Configuration;
using LevanaTracker.Api.Persistence;
using System.Reflection;

namespace LevanaTracker.Api;

public class Program
{
    public const int ShutdownTimeout = 5000;

    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

    public static Startup Startup { get; private set; } = null!;
    public static WebApplication Application { get; private set; } = null!;
    public static IServiceProvider Provider { get; private set; } = null!;
    public static ILogger Logger { get; private set; } = null!;

    public static async Task Main(string[] args)
    {
        Application = CreateApplication(args);
        Provider = Application.Services;
        Logger = Application.Services.GetRequiredService<ILogger<Startup>>();

        ValidateOptions();

        await MigrateDatabaseAsync();

        await Provider.InitializeApplicationAsync(Assembly);

        RegisterEndpoints();

        Provider.RunApplication(Assembly);

        string url = GetListenUrl();
        Application.Run(url);
    }

    private static WebApplication CreateApplication(string[] args)
    {
        var webApp = WebApplication.CreateBuilder(args);
        _ = webApp.Logging.AddConsole();

        Startup = new Startup(webApp.Configuration);
        Startup.ConfigureServices(webApp.Services);

        var host = webApp.Build();
        Startup.ConfigurePipeline(host, webApp.Environment);

        return host;
    }

    private static void ValidateOptions()
        => _ = Assembly.GetApplicationOptionTypes()
            .Select(x => Provider.GetRequiredService(x))
            .ToArray();

    private static void RegisterEndpoints()
    {
        var endpointTypes = Assembly.GetExecutingAssembly().GetHttpEndpointTypes();
        using var scope = Provider.CreateScope();

        foreach(var endpointType in endpointTypes)
        {
            var endpoint = (IRouteOwner) scope.ServiceProvider.GetRequiredService(endpointType);
            _ = endpoint.RegisterRoute(Application);
        }

        _ = Application.MapFallbackToFile("index.html");
    }

    private static string GetListenUrl()
    {
        var bindingOptions = Provider.GetRequiredService<BindingOptions>();
        return $"http://{bindingOptions.BindAddress}:{bindingOptions.ApplicationPort}";
    }

    private static async Task MigrateDatabaseAsync()
    {
        using var scope = Provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}