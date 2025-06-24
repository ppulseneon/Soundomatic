using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Soundomatic.ViewModels;
using Soundomatic.Views;

namespace Soundomatic;

/// <summary>
/// Основной класс приложения
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<App> _logger;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="serviceProvider">Сервис провайдер</param>
    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = _serviceProvider.GetRequiredService<ILogger<App>>();
    }
    
    /// <summary>
    /// Инициализация приложения
    /// </summary>
    public override void Initialize()
    {
        Resources[typeof(IServiceProvider)] = _serviceProvider;
        AvaloniaXamlLoader.Load(this);
        _logger.LogInformation("Application resources initialized and XAML loaded");
    }
    
    /// <summary>
    /// Метол для настройки главного окна, после инициализации фреймворка
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        _logger.LogInformation("Framework initialization completing");

        try
        {
            _logger.LogInformation("Initializing database");
            Builder.InitializeDatabase(_serviceProvider);
            _logger.LogInformation("Database initialization complete");

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DisableAvaloniaDataAnnotationValidation();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = _serviceProvider.GetService<MainWindowViewModel>(),
                };
            }
        }
        catch (Exception? ex)
        {
            _logger.LogCritical(ex, "A critical error occurred during framework initialization");
            ShutdownApplication(1); 
            return;
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Отключает валидацию данных на основе аннотаций в Avalonia
    /// </summary>
    private static void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
    
    /// <summary>
    /// Открывает приложение по нажатию на иконку в трее
    /// </summary>
    private void TrayIconOnClicked(object? sender, EventArgs e)
    {
        if (Current?.ApplicationLifetime is not ClassicDesktopStyleApplicationLifetime desktopLifitime) return;
        var mainWindow = desktopLifitime.MainWindow;

        if (mainWindow == null || mainWindow.IsVisible) return;
        mainWindow.Show();
        mainWindow.Activate();
    }

    /// <summary>
    /// Открывает приложение по нажатию на пункт в меню в трее
    /// </summary>
    private void OpenApplicationOnClick(object? sender, EventArgs e)
    {
        if (Current?.ApplicationLifetime is not ClassicDesktopStyleApplicationLifetime desktopLifitime) return;
        var mainWindow = desktopLifitime.MainWindow;

        if (mainWindow == null || mainWindow.IsVisible) return;
        mainWindow.Show();
        mainWindow.Activate();
    }

    /// <summary>
    /// Открывает окно настроек по нажатию на пункт меню в трее
    /// </summary>
    private void OpenSettingsOnClock(object? sender, EventArgs e)
    {
        _logger.LogInformation("Tray menu 'Open Settings' clicked. Functionality not yet implemented.");
    }

    /// <summary>
    /// Полностью закрывает приложение и снимает задачу с процесса
    /// </summary>
    private void CloseApplicationOnClick(object? sender, EventArgs e)
    {
        _logger.LogInformation("Tray menu 'Close Application' clicked. Exiting application.");
        ShutdownApplication(0);
    }
    
    /// <summary>
    /// Выполняет попытку корректного или принудительного завершения приложения.
    /// </summary>
    /// <param name="exitCode">Код выхода программы</param>
    private void ShutdownApplication(int exitCode = 0)
    {
        _logger.LogInformation("Attempting to shutdown application with exit code {exitCode}", exitCode);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime && exitCode == 0)
        {
            _logger.LogInformation("Shutting down via IClassicDesktopStyleApplicationLifetime.Shutdown().");
            desktopLifetime.Shutdown(exitCode);
        }
        else
        {
            _logger.LogWarning("Forcing application exit with Environment.Exit({code})", exitCode);
            Environment.Exit(exitCode);
        }
    }
}