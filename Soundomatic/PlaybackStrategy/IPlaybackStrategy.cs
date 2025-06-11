using System.Collections.Generic;
using Soundomatic.Models;

namespace Soundomatic.PlaybackStrategy;

/// <summary>
/// Интерфейс для стратегий выбора звука
/// </summary>
public interface IPlaybackStrategy
{

    /// <summary>
    /// Выбирает звук из пака для воспроизведения
    /// </summary>
    /// <param name="packName">Название пака</param>
    /// <param name="sounds">Список доступных звуков</param>
    /// <returns>Объект звука</returns>
    Sound SelectSound(string packName, IList<Sound> sounds);
} 