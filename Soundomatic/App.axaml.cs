using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Soundomatic.Hooks;
using Soundomatic.Models;
using Soundomatic.Services;
using Soundomatic.Services.Interfaces;
using Soundomatic.ViewModels;
using Soundomatic.Views;

namespace Soundomatic;

public class App : Application
{
    public IServiceProvider Services { get; private set; }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        var services = new ServiceCollection();
        var settings = GlobalSettings.Load("settings.json");
        
        ConfigureServices(services, settings);

        Services = services.BuildServiceProvider();
        
        var hookHandler = Services.GetService<OnKeyPressedHookHandler>() ?? throw new InvalidOperationException("Не удалось получить экземпляр OnKeyPressedHookHandler");;

        Task.Run(() => hookHandler.StartAsync());
    }

    private void ConfigureServices(IServiceCollection services, GlobalSettings settings)
    {
        services.AddSingleton(settings);

        services.AddSingleton<ISoundPlayer, SoundPlayer>();
        services.AddSingleton<OnKeyPressedHookHandler>();

        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ViewModelBase>();
    }
    
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}