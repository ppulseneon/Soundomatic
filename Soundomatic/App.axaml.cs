using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Soundomatic.Exceptions;
using Soundomatic.Exceptions.Base;
using Soundomatic.ViewModels;
using Soundomatic.Views;

namespace Soundomatic;

/// <summary>
/// Основной класс приложения
/// </summary>
public class App(IServiceProvider serviceProvider) : Application
{
    /// <summary>
    /// Инициализация приложения
    /// </summary>
    public override void Initialize()
    {
        Resources[typeof(IServiceProvider)] = serviceProvider;
        AvaloniaXamlLoader.Load(this);
    }
    
    /// <summary>
    /// Метол для настройки главного окна, после инициализации фреймворка
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        var logger = serviceProvider?.GetService<ILogger<App>>();

        if (serviceProvider == null)
        {
            BaseException.ThrowAsync<StartupException>();
            ShutdownApplication();
            return;
        }

        logger?.LogInformation("Framework initialization complete. Initializing database");
        Builder.InitializeDatabase(serviceProvider);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = serviceProvider.GetService<MainWindowViewModel>(),
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
    /// Выполняем попытку завершения приложения
    /// </summary>
    private void ShutdownApplication()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            desktopLifetime.Shutdown();
        }
        else
        {
            Environment.Exit(1);
        }
    }
}