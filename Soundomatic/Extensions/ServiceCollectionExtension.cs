using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Soundomatic.Extensions.Factories;
using Soundomatic.Services;
using Soundomatic.Services.Interfaces;
using Soundomatic.Storage;
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
    /// <param name="configuration">Конфигурация приложения</param>
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton(configuration)
            .AddViewModels()
            .AddAppSettings()
            .AddStorageServices(configuration)
            .AddApplicationServices();
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
    /// Метод для регистрации AppSettings в DI
    /// </summary>
    /// <param name="services">Абстрактная коллекция зависимостей</param>
    private static IServiceCollection AddAppSettings(this IServiceCollection services)
    {
        var fileStorage = new FileSettingsStorage();
        return services.AddSingleton(fileStorage.Load());
    }
    
    /// <summary>
    /// Метод для регистрации сервисов хранилища
    /// </summary>
    /// <param name="services">Абстрактная коллекция зависимостей</param>
    /// <param name="configuration">Конфигурация приложения</param>
    private static IServiceCollection AddStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        return services
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
    }
    
    /// <summary>
    /// Метод для регистрации сервисов приложения
    /// </summary>
    /// <param name="services">Абстрактная коллекция зависимостей</param>
    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ISoundStorageService, SoundStorageService>()
            .AddScoped<IKeyBindingService, KeyBindingService>()
            .AddScoped<ISoundFileService, SoundFileService>()
            .AddScoped<ISoundPlayer, SoundPlayer>()
            .AddScoped<IPlaybackService, PlaybackService>();
    }

    /// <summary>
    /// Метод для регистрации сервиса системных уведомлений
    /// </summary>
    private static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        var manager = NotificationManagerFactory.CreateNotificationManager();
        return services.AddSingleton(manager);
    }
}