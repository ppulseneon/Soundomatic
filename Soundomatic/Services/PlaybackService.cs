using System.Collections.Generic;
using System.Linq;
using Soundomatic.Enums;
using Soundomatic.Models;
using Soundomatic.Playback;
using Soundomatic.Services.Interfaces;

namespace Soundomatic.Services;

/// <summary>
/// Сервис для воспроизведения звуков
/// </summary>
public class PlaybackService: IPlaybackService
{
    private readonly Dictionary<PlaybackStrategyType, IPlaybackStrategy> _strategies = new()
    {
        { PlaybackStrategyType.Random, new RandomPlaybackStrategy() },
        { PlaybackStrategyType.Sequential, new SequentialPlaybackStrategy() }
    };

    public Sound? GetNextSoundToPlay(string packName, ICollection<Sound> sounds, PlaybackStrategyType strategiesTypeType)
    {
        if (sounds.Count == 0) return null;

        var soundsList = sounds.ToList();
        if (!_strategies.TryGetValue(strategiesTypeType, out var strategy))
        {
            strategy = _strategies[PlaybackStrategyType.Sequential];
        }

        return strategy.SelectSound(packName, soundsList);
    }
}