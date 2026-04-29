using System.Text.Json.Serialization;

namespace HrManagementSystem.Dependencies;

public static class BasicService
{
    public static IServiceCollection AddGlobalService(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        services.AddHttpContextAccessor();

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ix0oFS8QJAw9HSQvXkVhQlBad1hJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxWd0VhX39edHxURmdcWER9XEM=");

        services.AddDistributedMemoryCache();
        services.AddSignalR();
        services.AddHybridCache();

        return services;
    }
}
