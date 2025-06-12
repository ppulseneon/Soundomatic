using System.ComponentModel.DataAnnotations;
using Soundomatic.Enums;
using Soundomatic.Models.Base;

namespace Soundomatic.Models;

/// <summary>
/// Объект звука
/// </summary>
public class Sound: BaseEntity
{
    /// <summary>
    /// Название звука
    /// </summary>
    [Required, MaxLength(128)]
    public required string Name { get; set; }

    /// <summary>
    /// Название звукового файла
    /// </summary>
    [Required, MaxLength(128)]
    public required string FileName { get; init; }
    
    /// <summary>
    /// Формат звукового файла 
    /// </summary>
    public AudioFormat AudioFormat { get; init; }

    /// <summary>
    /// Размер файла в байтах
    /// </summary>
    public required long FileSizeBytes { get; set; }
    
    /// <summary>
    /// Громкость звука (от 0 до 100)
    /// </summary>
    public int Volume { get; set; }

    /// <summary>
    /// Группа, к которой принадлежит звук
    /// </summary>
    public SoundPack SoundPack { get; set; } = null!;
    
    /// <summary>
    /// ID группы, к которой принадлежит звук
    /// </summary>
    public long SoundPackId { get; set; }

    /// <summary>
    /// Путь к звуковому файлу
    /// </summary>
    public required string FilePath { get; set; }
}