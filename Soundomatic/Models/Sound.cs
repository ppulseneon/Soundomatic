using Soundomatic.Models.Enums;

namespace Soundomatic.Models;

/// <summary>
/// Объект звука
/// </summary>
public class Sound
{
    // todo: переписать под группы и вынести в бд, работу с файлами музыки вынести в FileStorage методы
    
    /// <summary>
    /// Название звука
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Название звукового файла
    /// </summary>
    public required string FileName { get; init; }
    
    /// <summary>
    /// Путь к звуковому файлу
    /// </summary>
    public required string FilePath { get; init; }
    
    /// <summary>
    /// Формат звукового файла 
    /// </summary>
    public AudioFormat AudioFormat { get; init; }

    /// <summary>
    /// Путь к иконке, связанной с этим звуковым файлом
    /// </summary>
    public string? IconPath { get; set; }

    /// <summary>
    /// Флаг, указывающий, является ли этот звуковой файл стандартным
    /// </summary>
    public bool IsDefault { get; init; }

    /// <summary>
    /// Размер файла в байтах
    /// </summary>
    public long FileSizeBytes { get; init; }
}