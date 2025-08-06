using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Soundomatic.Views;
using Avalonia.Controls;
using Avalonia.Threading;
using Soundomatic.Helpers;

namespace Soundomatic;

/// <summary>
/// Основной класс приложения
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<App> _logger;

    private TrayIcon? _trayIcon;

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
            _logger.LogInformation("Initializing application core");
            Builder.InitializeApplicationCore(_serviceProvider);
            _logger.LogInformation("Initializing database");
            Builder.InitializeDatabase(_serviceProvider);
            _logger.LogInformation("Database initialization complete");

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            }

            _trayIcon = new TrayIcon
            {
                Icon = new WindowIcon("Assets/Soundomatic.ico"),
                ToolTipText = "Soundomatic"
            };

            _trayIcon.Clicked += TrayIconOnClicked;
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
    /// Открывает кастомное меню трея
    /// </summary>
    private void TrayIconOnClicked(object? sender, EventArgs e)
    {
        if (Current?.ApplicationLifetime is not ClassicDesktopStyleApplicationLifetime) return;

        Dispatcher.UIThread.Post(() =>
        {
            _logger.LogInformation("Open tray menu");
            
            var trayMenuWindow = new TrayMenuWindow(_serviceProvider);
            var cursorPixelPoint = TrayPositioningHelper.GetCursorPosition();

            trayMenuWindow.Show();
            
            trayMenuWindow.InvalidateMeasure();
            trayMenuWindow.UpdateLayout();
            
            var windowSize = new Size(trayMenuWindow.Bounds.Width, trayMenuWindow.Bounds.Height);
            
            var optimalPosition = TrayPositioningHelper.CalculateOptimalPosition(
                cursorPixelPoint, 
                windowSize, 
                _logger);

            _logger.LogInformation("Оптимальная позиция меню трея: {x} {y}", optimalPosition.X, optimalPosition.Y);
            trayMenuWindow.Position = optimalPosition;
        });
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