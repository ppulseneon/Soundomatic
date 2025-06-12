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

        _indexLastPlayed.TryGetValue(packName, out var lastPlayed);
        Sound playSound;

        do
        {
            var currentIndex = _random.Next(sounds.Count);
            playSound = sounds[currentIndex];

        } while (playSound == lastPlayed);

        _indexLastPlayed[packName] = playSound;
        return playSound;
    }
} 