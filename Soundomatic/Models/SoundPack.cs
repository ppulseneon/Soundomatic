using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Soundomatic.Enums;
using Soundomatic.Models.Base;
using Soundomatic.PlaybackStrategy;

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
    public ICollection<Sound> Sounds { get; set; } = [];
    
    /// <summary>
    /// Данные иконки в формате байтов
    /// </summary>
    public byte[]? IconData { get; set; }
    
    /// <summary>
    /// MIME-тип иконки
    /// </summary>
    public string? IconMimeType { get; set; }
    
    /// <summary>
    /// Флаг, указывающий, является ли эта группа стандартной
    /// </summary>
    public bool IsDefault { get; init; }
} 