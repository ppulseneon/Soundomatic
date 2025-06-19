using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Soundomatic.Extensions;
using Soundomatic.Hooks;
using Soundomatic.Storage.DatabaseInitialization;
using Soundomatic.ViewModels;
using Soundomatic.Views;
using Microsoft.Extensions.Configuration;

namespace Soundomatic;

/// <summary>
/// Основной класс приложения
/// </summary>
public class App : Application
{
    private IServiceProvider? _services;
    
    /// <summary>
    /// Инициализация приложения
    /// </summary>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        
        _services = new ServiceCollection().AddServices(configuration).BuildServiceProvider();
        using var scope = _services.CreateScope();

        var scopeServiceProvider = scope.ServiceProvider;
        ApplicationDatabaseInitializer.Init(scopeServiceProvider);

        InitializeHooks();
    }

    /// <summary>
    /// Инициализация глобальных хуков
    /// </summary>
    private void InitializeHooks()
    {
        var hookHandler = _services?.GetService<OnKeyPressedHookHandler>();
        Task.Run(() => hookHandler?.StartAsync());
    }
    
    /// <summary>
    /// Метол для настройки главного окна, после инициализации фреймворка
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = _services?.GetService<MainWindowViewModel>(),
            };
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
    }

    /// <summary>
    /// Полностью закрывает приложение и снимает задачу с процесса
    /// </summary>
    private void CloseApplicationOnClick(object? sender, EventArgs e)
    {
        Environment.Exit(0);
    }
}