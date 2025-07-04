using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Controls.ApplicationLifetimes;
using Soundomatic.ViewModels;
using Soundomatic.Services;
using NAudio.CoreAudioApi;

namespace Soundomatic.Views
{
    /// <summary>
    /// Кастомное меню в трее
    /// </summary>
    public partial class TrayMenuWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<App> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceProvider">Сервис провайдер</param>
        /// <param name="_systemVolume">Начальная громкость приложения</param>
        public TrayMenuWindow(IServiceProvider serviceProvider, int _systemVolume)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<App>>();

            // TODO: Изменение громкости приложения
            var _trayMenuViewModel = new TrayMenuWindowViewModel(_systemVolume);
            this.DataContext = _trayMenuViewModel;

            this.Deactivated += (_, __) => this.Close();
        }

        /// <summary>
        /// Открыть главное окно приложения из трея
        /// </summary>
        private void TrayIconOpenClick(object? sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

            _logger.LogInformation("Выбрано пункт меню в трее 'Открыть приложение'. Открывается главное окно приложения.");
            var mainWindow = desktop.MainWindow;
            if (mainWindow is null || mainWindow.IsVisible) return;

            mainWindow.Show();
            mainWindow.Activate();

            this.Close();
        }

        /// <summary>
        /// Открыть окно системный настроек приложения из трея
        /// </summary>
        private void TrayMenuSettingsClick(object? sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

            _logger.LogInformation("Выбрано пункт меню в трее 'Открыть системные настройки'. Открывается окно системных настроек.");
            SystemSettingsWindow? _settingsWindow = new SystemSettingsWindow();
            var mainWindow = desktop.MainWindow;


            if (mainWindow is not null && mainWindow.IsVisible)
            {
                mainWindow.Hide();
            }

            if (_settingsWindow is null || !_settingsWindow.IsVisible)
            {
                _settingsWindow = new SystemSettingsWindow();
                _settingsWindow.Closed += (_, __) => _settingsWindow = null;
                _settingsWindow.Show();
            }

            else
            {
                _settingsWindow.Activate();
            }

            this.Close();
        }

        /// <summary>
        /// Закрыть приложение и снять задачу с процесса из трея
        /// </summary>
        private void TrayMenuExitCick(object? sender, RoutedEventArgs e)
        {
            _logger.LogInformation("Выбран пункт меню в трее 'Закрыть приложение'. Выполняется закрытие приложения.");
            Environment.Exit(0);
        }
    }
} 