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
using System.Runtime.InteropServices;
using Avalonia.Threading;

namespace Soundomatic;

/// <summary>
/// Основной класс приложения
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<App> _logger;
    private const int TrayMenuOffset = 5;

    private TrayIcon? _trayIcon;

    /// <summary>
    /// Получение позиции нажатия левой кнопки мыши для Windows
    /// </summary>
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);


    /// <summary>
    /// DLL для получения позиции нажатия левой кнопки мыши для Linux
    /// </summary>
    [DllImport("libX11")]
    private static extern IntPtr XOpenDisplay(IntPtr display);

    /// <summary>
    /// DLL для получения позиции нажатия левой кнопки мыши для Linux
    /// </summary>
    [DllImport("libX11")]
    private static extern int XQueryPointer(IntPtr display, IntPtr w, out IntPtr root_return,
        out System.IntPtr child_return, out int root_x_return, out int root_y_return,
        out int win_x_return, out int win_y_return, out uint mask_return);


    /// <summary>
    /// Получение позиции нажатия левой кнопки мыши для Linux
    /// </summary>
    private static (int X, int Y) GetCursorPositionX11()
    {
        IntPtr display = XOpenDisplay(System.IntPtr.Zero);
        if (display == IntPtr.Zero)
            return (100, 100);

        IntPtr root, child;
        uint mask;

        int root_x, root_y, win_x, win_y;

        XQueryPointer(display, IntPtr.Zero, out root, out child, out root_x, out root_y, out win_x, out win_y, out mask);
        return (root_x, root_y);
    }


    /// <summary>
    /// DLL для получения позиции нажатия левой кнопки мыши для MacOS
    /// </summary>
    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern IntPtr CGEventCreate(IntPtr source);

    
    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern CGPoint CGEventGetLocation(IntPtr eventRef);


    /// <summary>
    /// Получение позиции нажатия левой кнопки мыши для MacOS
    /// </summary>
    private static (int X, int Y) GetCursorPositionMacOs()
    {
        var eventRef = CGEventCreate(IntPtr.Zero);
        var loc = CGEventGetLocation(eventRef);

        return ((int)loc.X, (int)loc.Y);
    }

    /// <summary>
    /// Структура позиции нажатия мыши для MacOS
    /// </summary>
    private struct CGPoint
    {
        public double X;
        public double Y;
    }

    /// <summary>
    /// Структура позиции нажатия мыши для Windows
    /// </summary>
    private struct POINT
    {
        public int X;
        public int Y;
    }

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
            var pointClickUser = GetCursorPosition();

            trayMenuWindow.Show();
            var windowWidth = trayMenuWindow.Bounds.Width;
            var windowHeight = trayMenuWindow.Bounds.Height;

            var finalX = pointClickUser.X - ((int)trayMenuWindow.Width * 2);
            var finalY = pointClickUser.Y - ((int)trayMenuWindow.Height * 2);

            _logger.LogInformation("Assigning a tray menu position as {x} {y}", finalX, finalY);
            trayMenuWindow.Position = new PixelPoint(finalX, finalY);
        });
    }

    /// <summary>
    /// Получение позиции нажатия левой кнопки мыши на разных операционных системах
    /// </summary>
    private static (int X, int Y) GetCursorPosition()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (GetCursorPos(out var lpPoint))
                return (lpPoint.X, lpPoint.Y);
        }

        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return GetCursorPositionMacOs();

        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetCursorPositionX11();

        return (0, 0);
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