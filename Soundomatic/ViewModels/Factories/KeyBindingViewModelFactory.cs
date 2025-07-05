using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;
using Soundomatic.ViewModels.Factories.Interfaces;

namespace Soundomatic.ViewModels.Factories;

/// <summary>
/// Фабрика для создания KeyBindingViewModel
/// </summary>
/// <param name="keyBindingService"></param>
/// <param name="logger"></param>
public class KeyBindingViewModelFactory(IKeyBindingService keyBindingService, ISoundPlayer soundPlayer,
    ILogger<KeyBindingViewModel> logger): IKeyBindingViewModelFactory
{
    /// <inheritdoc />
    public KeyBindingViewModel Create(KeyBinding keyBinding, ObservableCollection<SoundPack> allAvailablePacks)
    {
        return new KeyBindingViewModel(keyBinding, allAvailablePacks, keyBindingService, soundPlayer, logger);
    }
}