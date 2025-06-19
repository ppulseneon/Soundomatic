using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Soundomatic.Extensions;
using Soundomatic.Hooks;
using Soundomatic.Storage.DatabaseInitialization;

namespace Soundomatic;

/// <summary>
/// Класс строителя приложения
/// </summary>
internal static class Builder
{
    /// <summary>
    /// Путь к файлу внутренних настроек приложения
    /// </summary>
    private const string JsonSettingsFile = "appsettings.json";
    
    /// <summary>
    /// Метод для построения сервисов
    /// </summary>
    public static ServiceProvider BuildServices()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(JsonSettingsFile, optional: false)
            .Build();
            
        var services = new ServiceCollection().AddServices(configuration);

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Инициализирует основные компоненты приложения
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void InitializeApplicationCore(IServiceProvider serviceProvider)
    {
        var hookHandler = serviceProvider.GetService<OnKeyPressedHookHandler>();
        if (hookHandler != null)
        {
            Task.Run(() => hookHandler.StartAsync()).ConfigureAwait(false);
        }
    }
    
    /// <summary>
    /// Инициализирует базу данных приложения
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        ApplicationDatabaseInitializer.Init(serviceProvider);
    }
}