using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Soundomatic.Hooks;
using Soundomatic.ViewModels;
using Soundomatic.Views;
using Microsoft.Extensions.Configuration;
using Soundomatic.Extensions;
using Soundomatic.Storage.DatabaseInitialization;

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
}