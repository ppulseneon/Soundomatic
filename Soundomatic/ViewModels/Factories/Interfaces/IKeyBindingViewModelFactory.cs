

using System.Collections.ObjectModel;
using Soundomatic.Models;

namespace Soundomatic.ViewModels.Factories.Interfaces;

/// <summary>
/// Интерфейс для фабрики KeyBindingViewModel
/// </summary>
public interface IKeyBindingViewModelFactory
{
    /// <summary>
    /// Формируем KeyBindingViewModel с параметрами из DI
    /// </summary>
    /// <param name="keyBinding">Привязанная клавиша</param>
    /// <param name="allAvailablePacks">Список доступных паков для привязки</param>
    /// <returns>Объект KeyBindingViewModel</returns>
    KeyBindingViewModel Create(KeyBinding keyBinding, ObservableCollection<SoundPack> allAvailablePacks);
}