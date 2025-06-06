using System.Collections.Generic;
using System.Linq;
using Soundomatic.Models;
using Soundomatic.PlaybackStrategy;
using Soundomatic.Services.Interfaces;

namespace Soundomatic.Services;

/// <summary>
/// Сервис для воспроизведения звуков
/// </summary>
public class PlaybackService: IPlaybackService
{
    private readonly Dictionary<Enums.PlaybackStrategy, IPlaybackStrategy> _strategies = new()
    {
        { Enums.PlaybackStrategy.Random, new RandomPlaybackStrategy() },
        { Enums.PlaybackStrategy.Sequential, new SequentialPlaybackStrategy() }
    };

    public Sound? GetNextSoundToPlay(ICollection<Sound> sounds, Enums.PlaybackStrategy strategiesType)
    {
        if (sounds.Count == 0) return null;

        var soundsList = sounds.ToList();
        if (!_strategies.TryGetValue(strategiesType, out var strategy))
        {
            strategy = _strategies[Enums.PlaybackStrategy.Sequential];
        }

        return strategy.SelectSound(soundsList);
    }
}