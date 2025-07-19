using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Soundomatic.Services;
using Soundomatic.Settings;
using Soundomatic.Storage;
using System;
using System.ComponentModel;

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
        public new event PropertyChangedEventHandler? PropertyChanged;

        [ObservableProperty] private bool _isSoundEnabled;

        [ObservableProperty] private int _systemVolume;

        protected new void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string textSoundEnabled => IsSoundEnabled ? "Çâóêè âêëþ÷åíû" : "Çâóêè âûêëþ÷åíû";

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
            get => (int)SystemVolume;
            set
            {
                _logger.LogInformation("A AppSystemVolume change in the window TrayMenuWindowViewModel has been started");

                if (SystemVolume != value)
                {
                    SystemVolume = value;
                    OnPropertyChanged(nameof(AppSystemVolume));

                    _appSettings.Volume = value;
                    _settingsStorage.Save(_appSettings);
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

            OnPropertyChanged(nameof(textSoundEnabled));
        }
    }
} 
