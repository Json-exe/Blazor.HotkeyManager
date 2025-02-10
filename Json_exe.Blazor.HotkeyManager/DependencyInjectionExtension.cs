using Microsoft.Extensions.DependencyInjection;

namespace Json_exe.Blazor.HotkeyManager;

/// <summary>
/// Extension methods for setting up HotkeyManager in an <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjectionExtension
{
    /// <summary>
    /// Adds the <see cref="HotkeyManager"/> to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add services to.
    /// </param>
    public static IServiceCollection AddHotkeyManager(this IServiceCollection services)
    {
        services.AddTransient<HotkeyManager>();
        return services;
    }
}