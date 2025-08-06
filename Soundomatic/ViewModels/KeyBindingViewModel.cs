using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Soundomatic.Enums;
using Soundomatic.Helpers;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;

namespace Soundomatic.ViewModels;

/// <summary>
/// ViewModel биндингов клавиш
/// </summary>
public partial class KeyBindingViewModel : ObservableObject
{
    private readonly IKeyBindingService _keyBindingService;
    private readonly ISoundPlayer _soundPlayer;
    private readonly ILogger _logger;
    
    /// <summary>
    /// Биндинг клавиши
    /// </summary>
    private KeyBinding KeyBinding { get; }
    
    /// <summary>
    /// Пак звуков биндинга
    /// </summary>
    [ObservableProperty]
    private SoundPack? _pack;
    
    /// <summary>
    /// Все доступные наборы для этого биндинга
    /// </summary>
    public ObservableCollection<SoundPack> AllAvailablePacks { get; }
    
    /// <summary>
    /// Статус биндинга клавиши
    /// </summary>
    public BindingStatus Status => GetStatus();
    
    /// <summary>
    /// Метод для получения user-friendly названия для кода клавиши
    /// </summary>
    public string GetKeyCodeName => KeyCodeHelper.GetFriendlyName(KeyBinding.Key);
    
    /// <summary>
    /// Конструктор
    /// </summary>
    public KeyBindingViewModel(KeyBinding keyBinding,
        ObservableCollection<SoundPack> allAvailablePacks, 
        IKeyBindingService keyBindingService,
        ISoundPlayer soundPlayer,
        ILogger logger)
    {
        _soundPlayer = soundPlayer;
        AllAvailablePacks = allAvailablePacks;
        _keyBindingService = keyBindingService;
        KeyBinding = keyBinding;
        Pack = keyBinding.Pack;
        _logger = logger;
    }

    /// <summary>
    /// Метод для получения статуса биндинга
    /// </summary>
    private BindingStatus GetStatus()
    {
       if (!KeyCodeHelper.IsKeyAssigned(KeyBinding.Key)) 
       {
           return BindingStatus.NotAssigned;
       }

       return Pack switch
       {
           { Sounds.Count: > 1 } => BindingStatus.SoundPackInstalled,
           { Sounds.Count: 1 } => BindingStatus.SoundInstalled,
           _ => BindingStatus.NoSoundSelected
       };
    }
    
    /// <summary>
    /// Обработчик изменения пака
    /// </summary>
    async partial void OnPackChanged(SoundPack? oldValue, SoundPack? newValue)
    {
        if (newValue == null || oldValue == null || oldValue == newValue) return;
        
        KeyBinding.Pack = newValue;
        await _keyBindingService.UpdateKeyBindingAsync(KeyBinding);
        
        _logger.LogInformation($"Pack changed from {oldValue?.Name} to {newValue.Name}");
        
        OnPropertyChanged(nameof(Status));
    }
    
    /// <summary>
    /// Команда для прослушивания пака звуков
    /// </summary>
    [RelayCommand]
    public async Task PlayPackSoundCommand()
    {
        if (Pack != null)
        {
            await _soundPlayer.PlayAsync(Pack);
        }
    }
}