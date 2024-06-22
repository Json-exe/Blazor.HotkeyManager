using Microsoft.Extensions.DependencyInjection;

namespace Json_exe.Blazor.HotkeyManager;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddHotkeyManager(this IServiceCollection services)
    {
        services.AddTransient<HotkeyManager>();
        return services;
    }
}