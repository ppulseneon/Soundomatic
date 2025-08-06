using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Soundomatic.Enums;
using Soundomatic.Models.Base;
using Soundomatic.Playback;

namespace Soundomatic.Models;

/// <summary>
/// Группа звуков
/// </summary>
public class SoundPack: BaseEntity
{
    /// <summary>
    /// Название группы
    /// </summary>
    [Required, MaxLength(128)]
    public required string Name { get; set; }
    
    /// <summary>
    /// Звуки, входящие в группу
    /// </summary>
    public List<Sound> Sounds { get; set; } = [];
    
    /// <summary>
    /// Данные иконки в формате байтов
    /// </summary>
    public byte[]? IconData { get; set; }

    /// <summary>
    /// Стратегия воспроизведения звуков пака
    /// </summary>
    [Required]
    public PlaybackStrategyType PlaybackStrategyType { get; set; }
    
    /// <summary>
    /// MIME-тип иконки
    /// </summary>
    [MaxLength(128)]
    public string? IconMimeType { get; set; }
    
    /// <summary>
    /// Флаг, указывающий, является ли эта группа стандартной
    /// </summary>
    public bool IsDefault { get; init; }
    
    /// <summary>
    /// Создает и возвращает экземпляр стратегии воспроизведения
    /// </summary>
    public IPlaybackStrategy? CreateStrategy()
    {
        if (Sounds.Count == 0)
        {
            return null; 
        }

        return PlaybackStrategyType switch
        {
            PlaybackStrategyType.Sequential => new SequentialPlaybackStrategy(),
            PlaybackStrategyType.Random => new RandomPlaybackStrategy(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
} 