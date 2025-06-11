using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Soundomatic.Models;

namespace Soundomatic.PlaybackStrategy;

/// <summary>
/// Стратегия последовательного выбора звука
/// </summary>
public class SequentialPlaybackStrategy : IPlaybackStrategy
{
    /// <summary>
    /// Коллекция для хранения индексов последних воспроизводимых звуков по названиям наборов
    /// </summary>
    private ConcurrentDictionary<string, int> indexLastPlayed = new ConcurrentDictionary<string, int>();

    /// <inheritdoc />
    public Sound SelectSound(string packName, IList<Sound> sounds)
    {
        if (sounds == null || sounds.Count == 0)
        {
            throw new InvalidOperationException("Набор звуков не может быть пустым");
        }

        int index = indexLastPlayed.GetOrAdd(packName, 0);
        Sound playSound = sounds[index];

        index++;
        if (index >= sounds.Count)
        {
            index = 0;
        }

        indexLastPlayed[packName] = index;
        return playSound;
    }
} 