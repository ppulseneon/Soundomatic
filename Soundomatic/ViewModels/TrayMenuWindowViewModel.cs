using CommunityToolkit.Mvvm.ComponentModel;
using Soundomatic.Services;

namespace Soundomatic.ViewModels
{
    /// <summary>
    /// ViewModel ���������� ���� � ���� ��� ��������� ��������� ���������� ����� UI
    /// </summary>
    public partial class TrayMenuWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _systemVolume;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="_systemVolume">���������� ��������� ����������</param>
        public TrayMenuWindowViewModel(int _systemVolume)
        {
            this._systemVolume = _systemVolume;
        }

        // TODO: ����������� ��������� ��������� ���������� ����� slider � TrayMenuWindow
    }
} 