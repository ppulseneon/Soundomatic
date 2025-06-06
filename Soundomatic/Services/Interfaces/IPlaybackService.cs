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
    /// <param name="sounds"></param>
    /// <param name="strategiesType"></param>
    /// <returns></returns>
    Sound? GetNextSoundToPlay(ICollection<Sound> sounds, Enums.PlaybackStrategy strategiesType);
}