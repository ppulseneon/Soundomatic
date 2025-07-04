using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SharpHook.Data;
using Soundomatic.Messages;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;
using Soundomatic.ViewModels.Factories.Interfaces;

namespace Soundomatic.ViewModels;

/// <summary>
/// ViewModel главного окна приложения
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;
    private readonly IKeyBindingService _keyBindingService;
    private readonly ISoundStorageService _soundStorageService;
    private readonly IKeyBindingViewModelFactory _keyBindingViewModelFactory;
    private readonly ISystemNotificationService _systemNotificationService;
    private bool _closeNotificationShown;

    [ObservableProperty] private ObservableCollection<SoundPack> _soundPacks = [];
    [ObservableProperty] private ObservableCollection<KeyBindingViewModel> _keyBindings = [];
    
    
    public MainWindowViewModel(IKeyBindingViewModelFactory keyBindingViewModelFactory,
        IKeyBindingService keyBindingService,
        ISoundStorageService soundStorageService,
        ISystemNotificationService systemNotificationService, IMessenger messenger)
    {
        _keyBindingService = keyBindingService;
        _soundStorageService = soundStorageService;
        _keyBindingViewModelFactory = keyBindingViewModelFactory;
        _systemNotificationService = systemNotificationService;
        _messenger = messenger;
        LoadDataAsync();
    }

    /// <summary>
    /// Метод загрузки данных для страницы
    /// </summary>
    private async void LoadDataAsync()
    {
        await LoadSoundPacksAsync();
        await LoadKeyBindingsAsync();
    }

    /// <summary>
    /// Загрузка биндингов клавиш
    /// </summary>
    private async Task LoadKeyBindingsAsync()
    {
        var bindings = await _keyBindingService.GetAllKeyBindingsAsync();
        KeyBindings.Clear();

        foreach (var binding in bindings)
        {
            var keyBindingViewModel = _keyBindingViewModelFactory.Create(binding, SoundPacks);
            KeyBindings.Add(keyBindingViewModel);
        }
    }

    /// <summary>
    /// Загрузка наборов звуков
    /// </summary>
    private async Task LoadSoundPacksAsync()
    {
        var sounds = await _soundStorageService.GetAllSoundPacksAsync();
        SoundPacks.Clear();

        foreach (var sound in sounds)
        {
            SoundPacks.Add(sound);
        }
    }
    
    /// <summary>
    /// Команда для добавления нового биндинга
    /// </summary>
    [RelayCommand]
    private async Task AddKeyBinding()
    {
        var newBindingModel = new KeyBinding
        {
            Key = KeyCode.VcUndefined,
        };
        
        await _keyBindingService.AddKeyBindingAsync(newBindingModel);
        await LoadKeyBindingsAsync();
    }
    
    /// <summary>
    /// Команда для открытия меню управления звуками
    /// </summary>
    [RelayCommand]
    private void ShowManageSoundView()
    {
        throw new System.NotImplementedException();
    }
    
    /// <summary>
    /// Команда для сворачивания приложения
    /// </summary>
    [RelayCommand]
    private void HideApplication()
    {
        if (!_closeNotificationShown)
        {
            _systemNotificationService.SendNotification("Soundomatic был свернут", "Значок Soundomatic можно найти в области уведомлений для открытия приложения");
            _closeNotificationShown = true;
        }
        
        _messenger.Send(new HideWindowMessage());
    }
}