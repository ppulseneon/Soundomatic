using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Soundomatic.Settings;
using Soundomatic.Storage;

namespace Soundomatic.ViewModels
{
    /// <summary>
    /// ViewModel для окна системных настроек приложения
    /// </summary>
    public partial class SystemSettingsViewModel : INotifyPropertyChanged
    {
        private readonly FileSettingsStorage _settingsStorage;
        private readonly AppSettings _applicationSettings;
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _systemVolume;
        private bool _systemAutoStart;
        private bool _startSystemVolumeOffOn;

        protected void OnPropertyChanged(string propertyName)
              => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Конструктор
        /// </summary>
        public SystemSettingsViewModel()
        {
            _settingsStorage = new FileSettingsStorage();
            _applicationSettings = _settingsStorage.Load();

            _startSystemVolumeOffOn = _applicationSettings.IsSoundEnabled;
            _systemVolume = _applicationSettings.Volume;
            _systemAutoStart = _applicationSettings.AutoRun;
        }

        /// <summary>
        /// Чтение и изменение значения включенного/выключенного звука приложения
        /// </summary>
        public bool StartSystemVolumeOffOn
        {
            get => _startSystemVolumeOffOn;
            set => _startSystemVolumeOffOn = value;
        }

        /// <summary>
        /// Чтение и изменение значения громкости приложения
        /// </summary>
        public int AppSystemVolume
        {
            get => _systemVolume;
            set
            {
                if (_systemVolume != value)
                {
                    _systemVolume = value;
                    OnPropertyChanged(nameof(AppSystemVolume));

                    _applicationSettings.Volume = value;
                    _settingsStorage.Save(_applicationSettings);
                }
            }
        }

        /// <summary>
        /// Чтение и изменение значения автозапуска приложения
        /// </summary>
        public bool StartSystemAutoStart
        {
            get => _systemAutoStart;
            set => _systemAutoStart = value;
        }
    }

    /// <summary>
    /// Привязка изменения громкости приложения
    /// </summary>
    public static class ViewModelLocator
    {
        public static SystemSettingsViewModel SystemSettingsVM { get; } = new SystemSettingsViewModel();
    }
}
