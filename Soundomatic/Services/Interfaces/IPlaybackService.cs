using System.Collections.Generic;
using Soundomatic.Models;

namespace Soundomatic.Services.Interfaces;

/// <summary>
/// Интерфейс для сервиса воспроизведения звуков
/// </summary>
public interface IPlaybackService
{
    /// <summary>
    /// Метод для воспроизведения звука из списка звуков пака
    /// </summary>
    /// <param name="packName">Название пака</param>
    /// <param name="sounds">Список доступных звуков</param>
    /// <param name="strategiesTypeType">Стратегия выбора звука</param>
    /// <returns>Объект звука</returns>
    Sound? GetNextSoundToPlay(string packName, ICollection<Sound> sounds, Enums.PlaybackStrategyType strategiesTypeType);
}