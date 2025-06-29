using Avalonia;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Soundomatic.Exceptions;
using Soundomatic.Exceptions.Base;

namespace Soundomatic;

/// <summary>
/// Класс для инициализации приложения
/// </summary>
sealed class Program
{
    /// <summary>
    /// Точка входа в приложение. Инициализирует и запускает приложение
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            var serviceProvider = Builder.BuildServices();
            BuildAvaloniaApp(serviceProvider) 
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            ProcessStartupException(ex);
        }
    }
    
    /// <summary>
    /// Создает и настраивает экземпляр приложения с сервисами для работы приложения.
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов для внедрения зависимостей</param>
    /// <returns>Настроенный экземпляр AppBuilder</returns>
    private static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<App>>();
        logger.LogInformation("Application service provider built. Configuring App for runtime.");
        
        return AppBuilder.Configure(() => new App(serviceProvider)) 
           .UsePlatformDetect()
           .WithInterFont()
           .LogToTrace();
    }

    /// <summary>
    /// Обрабатывает исключения, возникающие во время запуска приложения.
    /// Логирует ошибку и выбрасывает асинхронное исключение для дальнейшей обработки.
    /// </summary>
    /// <param name="ex">Исключение, которое произошло во время запуска.</param>
    private static void ProcessStartupException(Exception ex)
    {
        try
        {
            var serviceProvider = Builder.BuildServices(); 
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An unhandled exception occurred during application startup.");
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Failed to log startup exception: {exception}");
            Console.Error.WriteLine($"Original startup exception: {ex}");
        }
        
        BaseException.ThrowAsync<StartupException>();
    }
}