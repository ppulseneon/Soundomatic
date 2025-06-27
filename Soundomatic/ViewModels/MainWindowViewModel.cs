using System.Collections.ObjectModel;
using System.Linq;
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
    
    [ObservableProperty]
    private ObservableCollection<KeyBinding> _keyBindings = [];

    public MainWindowViewModel(IKeyBindingService keyBindingService)
    {
        _keyBindingService = keyBindingService;
        LoadSoundPacksAsync();
    }
    
    private async void LoadSoundPacksAsync()
    {
        var bindings = await _keyBindingService.GetAllKeyBindingsAsync();
        KeyBindings.Clear();
        foreach (var binding in bindings)
        {
            KeyBindings.Add(binding);
        }
    }
}