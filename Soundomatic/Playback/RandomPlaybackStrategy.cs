using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Soundomatic.Models;

namespace Soundomatic.Playback;

/// <summary>
/// Стратегия случайного выбора звука
/// </summary>
public class RandomPlaybackStrategy : IPlaybackStrategy
{
    private readonly Random _random = new();

    /// <summary>
    /// Коллекция для хранения индексов последних воспроизводимых звуков по названиям наборов
    /// </summary>
    private readonly ConcurrentDictionary<string, Sound> _indexLastPlayed = new();

    /// <inheritdoc />
    public Sound SelectSound(string packName, IList<Sound> sounds)
    {
        if (sounds == null || sounds.Count == 0)
        {
            throw new InvalidOperationException("Набор звуков не может быть пустым");
        }

        if (sounds.Count == 1)
        {
            _indexLastPlayed[packName] = sounds[0];
            return sounds[0];
        }

        _indexLastPlayed.TryGetValue(packName, out Sound? lastPlayed);
        Sound playSound;

        var currentIndex = _random.Next(sounds.Count);
        playSound = sounds[currentIndex];

        if (lastPlayed is not null && sounds.Contains(lastPlayed) && playSound == lastPlayed)
        {
            if (_random.Next(0, 2) == 0)
            {
                var flagNewSound = false;

                for (int i = 0; i < sounds.Count; i++)
                {
                    var newIndex = _random.Next(sounds.Count);
                    var newSound = sounds[newIndex];

                    if (newSound != lastPlayed)
                    {
                        playSound = newSound;
                        flagNewSound = true;
                        break;
                    }
                }

                if (!flagNewSound)
                {
                    playSound = lastPlayed;
                }
            }
        }

        _indexLastPlayed[packName] = playSound;
        return playSound;
    }
} 