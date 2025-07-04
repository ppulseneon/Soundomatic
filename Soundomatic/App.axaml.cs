using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Soundomatic.Views;
using NAudio.CoreAudioApi;
using Avalonia.Controls;
using System.Runtime.InteropServices;
using Avalonia.Threading;
using Avalonia.Platform;

namespace Soundomatic;

/// <summary>
/// Основной класс приложения
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<App> _logger;

    // TODO: Удалить, когда будет реализован механизм изменения громкости приложения
    private readonly MMDeviceEnumerator _deviceEnumerator;
    private readonly MMDevice _device;

    private TrayIcon? _trayIcon;

    /// <summary>
    /// Получение позиции нажатия левой кнопки мыши для Windows
    /// </summary>
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);


    /// <summary>
    /// DLL дл получения позиции нажатия левой кнопки мыши для Linux
    /// </summary>
    [System.Runtime.InteropServices.DllImport("libX11")]
    private static extern System.IntPtr XOpenDisplay(System.IntPtr display);

    [System.Runtime.InteropServices.DllImport("libX11")]
    private static extern int XQueryPointer(System.IntPtr display, System.IntPtr w, out System.IntPtr root_return,
        out System.IntPtr child_return, out int root_x_return, out int root_y_return,
        out int win_x_return, out int win_y_return, out uint mask_return);


    /// <summary>
    /// Получение позиции нажатия левой кнопки мыши для Linux
    /// </summary>
    private static (int X, int Y) GetCursorPositionX11()
    {
        System.IntPtr display = XOpenDisplay(System.IntPtr.Zero);
        if (display == System.IntPtr.Zero)
            return (100, 100);

        System.IntPtr root, child;

        int root_x, root_y, win_x, win_y;
        uint mask;

        XQueryPointer(display, System.IntPtr.Zero, out root, out child, out root_x, out root_y, out win_x, out win_y, out mask);
        return (root_x, root_y);
    }


    /// <summary>
    /// DLL дл получения позиции нажатия левой кнопки мыши для MacOS
    /// </summary>
    [System.Runtime.InteropServices.DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern System.IntPtr CGEventCreate(System.IntPtr source);

    [System.Runtime.InteropServices.DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern CGPoint CGEventGetLocation(System.IntPtr eventRef);


    /// <summary>
    /// Получение позиции нажатия левой кнопки мыши для MacOS
    /// </summary>
    private static (int X, int Y) GetCursorPositionMacOS()
    {
        System.IntPtr eventRef = CGEventCreate(System.IntPtr.Zero);
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
        _deviceEnumerator = new MMDeviceEnumerator();
        _device = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
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
        if (Current?.ApplicationLifetime is not ClassicDesktopStyleApplicationLifetime desktopLifitime) return;

        Dispatcher.UIThread.Post(() =>
        {
            // TODO: Передавать громкость самого приложения. Сейчас в меню передается громкость системы
            var _currentProgramVolume = (int)(_device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);

            _logger.LogInformation("Open tray menu");
            var _trayMenuWindow = new TrayMenuWindow(_serviceProvider, _currentProgramVolume);
            var _pointClickUser = GetCursorPosition();

            _logger.LogInformation("Assigning a tray menu position");
            _trayMenuWindow.Position = new PixelPoint(_pointClickUser.X - ((int)_trayMenuWindow.Width * 2), _pointClickUser.Y - ((int)_trayMenuWindow.Height * 2));
            _trayMenuWindow.Show();
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
            return GetCursorPositionMacOS();

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