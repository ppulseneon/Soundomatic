using CommunityToolkit.Mvvm.ComponentModel;
using Soundomatic.Services;

namespace Soundomatic.ViewModels
{
    /// <summary>
    /// ViewModel кастомного меню в трее для изменения громкости приложения через UI
    /// </summary>
    public partial class TrayMenuWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _systemVolume;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_systemVolume">ГНачальная громкость приложения</param>
        public TrayMenuWindowViewModel(int _systemVolume)
        {
            this._systemVolume = _systemVolume;
        }

        // TODO: Реализовать изменение громкости приложения через slider в TrayMenuWindow
    }
} 