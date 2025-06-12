using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Soundomatic.Models;

namespace Soundomatic.PlaybackStrategy;

/// <summary>
/// Стратегия случайного выбора звука
/// </summary>
public class RandomPlaybackStrategy : IPlaybackStrategy
{
    private readonly Random random = new Random();

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
        Sound? playSound = null;

        if (lastPlayed is not null && random.Next(0, 2) == 0)
        {
            if (sounds.Contains(lastPlayed))
            {
                playSound = lastPlayed;
                _indexLastPlayed[packName] = playSound;
                return playSound;
            }
        }

        var maxAttempt = sounds.Count * 2;

        for (int i = 0; i < maxAttempt; i++)
        {
            var currentIndexSound = random.Next(sounds.Count);
            Sound currentSound = sounds[currentIndexSound];

            if (currentSound != lastPlayed)
            {
                playSound = currentSound;
                break;
            }
        }

        if (playSound is null)
        {
            playSound = sounds[random.Next(sounds.Count)];
        }

        _indexLastPlayed[packName] = playSound;
        return playSound;
    }
} 