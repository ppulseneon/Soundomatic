using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;

namespace Soundomatic.ViewModels;

/// <summary>
/// ViewModel главного окна приложения
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IKeyBindingService _keyBindingService;
    private readonly ISoundStorageService _soundStorageService;

    [ObservableProperty] private ObservableCollection<SoundPack> _soundPacks = [];
    [ObservableProperty] private ObservableCollection<KeyBindingViewModel> _keyBindings = [];

    public MainWindowViewModel(IKeyBindingService keyBindingService, ISoundStorageService soundStorageService)
    {
        _keyBindingService = keyBindingService;
        _soundStorageService = soundStorageService;
        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        await LoadSoundPacksAsync();
        await LoadKeyBindingsAsync();
    }

    private async Task LoadKeyBindingsAsync()
    {
        var bindings = await _keyBindingService.GetAllKeyBindingsAsync();
        KeyBindings.Clear();

        foreach (var binding in bindings)
        {
            var keyBindingViewModel = new KeyBindingViewModel(SoundPacks, binding);
            KeyBindings.Add(keyBindingViewModel);
        }
    }

    private async Task LoadSoundPacksAsync()
    {
        var sounds = await _soundStorageService.GetAllSoundPacksAsync();
        SoundPacks.Clear();

        foreach (var sound in sounds)
        {
            SoundPacks.Add(sound);
        }
    }
}