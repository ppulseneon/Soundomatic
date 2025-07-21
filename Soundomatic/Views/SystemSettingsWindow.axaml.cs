using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Soundomatic.Settings;
using Soundomatic.Storage;
using Soundomatic.ViewModels;
using System;

namespace Soundomatic.Views
{
    public partial class SystemSettingsWindow : Window
    {
        /// <summary>
        /// Единственный экземпляр окна настроек
        /// </summary>
        private static SystemSettingsWindow? _instanceSystemSettings;

        private readonly FileSettingsStorage _settingsStorage;
        private readonly AppSettings _applicationSettings;


        /// <summary>
        /// Конструктор
        /// </summary>
        public SystemSettingsWindow()
        {
            DataContext = ViewModelLocator.SystemSettingsVM;
            InitializeComponent();

            _settingsStorage = new FileSettingsStorage();
            _applicationSettings = _settingsStorage.Load();
        }

        /// <summary>
        /// Получает или создает единственный экземпляр окна настроек
        /// </summary>
        public static SystemSettingsWindow GetInstance()
        {
            if (_instanceSystemSettings == null)
            {
                _instanceSystemSettings = new SystemSettingsWindow();
                _instanceSystemSettings.Closed += (_, __) => _instanceSystemSettings = null;
            }

            return _instanceSystemSettings;
        }

        /// <summary>
        /// Метод для обработки зажатия содержимого окна
        /// </summary>
        private void ContentPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            // Принимаем событие и ничего не делаем
            e.Handled = true;
        }

        /// <summary>
        ///  Метод для обработки зажатия рамки окна
        /// </summary>
        private void MainBorderPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }

        /// <summary>
        /// Метод для обработки зажатия TitleBar окна
        /// </summary>
        private void TitleBarPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }

        /// <summary>
        /// Метод для обработки нажатие на кнопку сворачивания окна
        /// </summary>
        private void MinimizeButtonClick(object? sender, RoutedEventArgs e)
        {
            if (VisualRoot is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        /// <summary>
        /// Метод для обработки нажатия на кнопку закрытия окна
        /// </summary>
        public void HideWindow(object? sender, RoutedEventArgs e)
        {
            _instanceSystemSettings = null;
            Close();
        }

        /// <summary>
        /// Метод для сохранения настроек включенного/выключенного звука всего приложения
        /// </summary>
        public void SystemVolumeOffOnChecked(object? sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch systemVolumeOffOn && systemVolumeOffOn.IsChecked is not null)
            {
                _applicationSettings.IsSoundEnabled = (bool)systemVolumeOffOn.IsChecked;
                _settingsStorage.Save(_applicationSettings);
            }
        }

        /// <summary>
        /// Метод для сохранения настроек автоматического включения приложения при запуске компьютера
        /// </summary>
        public void SystemAutoStartChecked(object? sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch sustemAutoRun && sustemAutoRun.IsChecked is not null)
            {
                _applicationSettings.AutoRun = (bool)sustemAutoRun.IsChecked;
                _settingsStorage.Save(_applicationSettings);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки сохранения
        /// </summary>
        public void SaveClick(object? sender, RoutedEventArgs e)
        {
            // TODO: нужна ли кнопка сохранения?
            Close();
        }
    }
}