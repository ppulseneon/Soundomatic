using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;

namespace Soundomatic.ViewModels;

/// <summary>
/// ViewModel главного окна приложения
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ISoundStorageService _soundStorageService;
    
    [ObservableProperty]
    private ObservableCollection<SoundPack> _soundPacks = [];
    
    public MainWindowViewModel(ISoundStorageService soundStorageService)
    {
        _soundStorageService = soundStorageService;
        LoadSoundPacksAsync();
    }
    
    private async void LoadSoundPacksAsync()
    {
        var packs = await _soundStorageService.GetAllSoundPacksAsync();
        SoundPacks.Clear();
        foreach (var pack in packs)
        {
            SoundPacks.Add(pack);
        }
    }
}