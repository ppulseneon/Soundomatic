using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Soundomatic.Models.Settings;
using Soundomatic.Storage.Context;
using Soundomatic.ViewModels;

namespace Soundomatic.Extensions;

/// <summary>
/// Статический класс для регистрации сервисов приложения в контейнере DI 
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Метод регистрирует пользовательские сервисы в контейнере DI
    /// </summary>
    /// <param name="services">Абстрактная коллекция зависимостей</param>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddViewModels()
            .AddStorageServices();
    }
    
    /// <summary>
    /// Метод для регистрации ViewModels в DI
    /// </summary>
    /// <param name="services">Абстрактная коллекция зависимостей</param>
    private static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        return services
            .AddTransient<ViewModelBase>()
            .AddTransient<MainWindowViewModel>();
    }

    /// <summary>
    /// Метод для регистрации сервисов хранилища
    /// </summary>
    /// <param name="services">Абстрактная коллекция зависимостей</param>
    private static IServiceCollection AddStorageServices(this IServiceCollection services)
    {
        var settings = new DbSettings();
        
        return services
            .AddDbContextFactory<ApplicationDbContext>(builder => builder.UseSqlite(settings.ConnectionString));
    }
}