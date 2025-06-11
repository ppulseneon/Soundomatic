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
    private Random random = new Random();

    /// <summary>
    /// Коллекция для хранения индексов последних воспроизводимых звуков по названиям наборов
    /// </summary>
    private ConcurrentDictionary<string, Sound> indexLastPlayed = new ConcurrentDictionary<string, Sound>();

    /// <inheritdoc />
    public Sound SelectSound(string packName, IList<Sound> sounds)
    {
        if (sounds == null || sounds.Count == 0)
        {
            throw new InvalidOperationException("Набор звуков не может быть пустым");
        }

        if (sounds.Count == 1)
        {
            indexLastPlayed[packName] = sounds[0];
            return sounds[0];
        }

        indexLastPlayed.TryGetValue(packName, out Sound? lastPlayed);
        Sound playSound;

        do
        {
            int currentIndex = random.Next(sounds.Count);
            playSound = sounds[currentIndex];

        } while (playSound == lastPlayed);

        indexLastPlayed[packName] = playSound;
        return playSound;
    }
} 