using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Soundomatic.Settings;
using Soundomatic.Storage;
using Soundomatic.Views;
using System;
using System.ComponentModel;
using Soundomatic.ViewModels;

namespace Soundomatic.ViewModels
{
    /// <summary>
    /// ViewModel кастомного меню в трее для изменения громкости приложения через UI
    /// </summary>
    public partial class TrayMenuWindowViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<App> _logger;

        private readonly FileSettingsStorage _settingsStorage;
        private readonly AppSettings _appSettings;
        private readonly SystemSettingsViewModel _systemSettingsVM = ViewModelLocator.SystemSettingsVM;
        public new event PropertyChangedEventHandler? PropertyChanged;

        [ObservableProperty] private bool _isSoundEnabled;
        [ObservableProperty] private int _systemVolume;

        protected new void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string TextSoundEnabled => IsSoundEnabled ? "Звуки включены" : "Звуки выключены";

        /// <summary>
        /// Конструктор
        /// </summary>
        public TrayMenuWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<App>>();

            try
            {
                _logger.LogInformation("The vm:TrayMenuWindowViewModel constructor is running. System settings are being loaded");
                _settingsStorage = new FileSettingsStorage();
                _appSettings = _settingsStorage.Load();

                _systemVolume = _appSettings.Volume;
                _isSoundEnabled = _appSettings.IsSoundEnabled;

                _systemSettingsVM.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(AppSystemVolume))
                    {
                        OnPropertyChanged(nameof(AppSystemVolume));
                    }
                };
            }
            catch(Exception? ex)
            {
                _logger.LogError("TrayMenuWindowViewModel error: {error}", ex.Message);
            }
        }

        /// <summary>
        /// Чтение статуса включен ли звук
        /// </summary>
        public bool isSoundEnabled
        {
            get => IsSoundEnabled;
        }

        /// <summary>
        /// Чтение и изменение громкости приложения
        /// </summary>
        public int AppSystemVolume
        {
            get => _systemSettingsVM.AppSystemVolume;
            set
            {
                if (_systemSettingsVM.AppSystemVolume != value)
                {
                    _systemSettingsVM.AppSystemVolume = value;
                    OnPropertyChanged(nameof(_systemSettingsVM.AppSystemVolume));
                }
            }
        }

        /// <summary>
        /// Изменение состояния включенного/выключенного звука приложения
        /// </summary>
        public void ToggleSound()
        {
            _logger.LogInformation("A IsSoundEnabled change in the window TrayMenuWindowViewModel has been started");
            IsSoundEnabled = !IsSoundEnabled;
            OnPropertyChanged(nameof(isSoundEnabled));
        }

        /// <summary>
        /// Обновление данных в настройках приложения
        /// </summary>
        /// <param name="value">Включен ли звук приложения</param>
        partial void OnIsSoundEnabledChanged(bool value)
        {
            _appSettings.IsSoundEnabled = value;
            _settingsStorage.Save(_appSettings);

            OnPropertyChanged(nameof(TextSoundEnabled));
        }
    }
} 