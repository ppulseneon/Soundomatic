using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Soundomatic.Models;

namespace Soundomatic.Playback;

/// <summary>
/// Стратегия последовательного выбора звука
/// </summary>
public class SequentialPlaybackStrategy : IPlaybackStrategy
{
    /// <summary>
    /// Коллекция для хранения индексов последних воспроизводимых звуков по названиям наборов
    /// </summary>
    private readonly ConcurrentDictionary<string, int> _indexLastPlayed = new();

    /// <inheritdoc />
    public Sound SelectSound(string packName, IList<Sound> sounds)
    {
        if (sounds == null || sounds.Count == 0)
        {
            throw new InvalidOperationException("Набор звуков не может быть пустым");
        }

        var index = _indexLastPlayed.GetOrAdd(packName, 0);
        var playSound = sounds[index];

        index++;
        if (index >= sounds.Count)
        {
            index = 0;
        }

        _indexLastPlayed[packName] = index;
        return playSound;
    }
} 