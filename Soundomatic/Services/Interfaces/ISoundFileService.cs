using System.Threading.Tasks;
using Soundomatic.Models;

namespace Soundomatic.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с файлами звуков в файловой системе
/// </summary>
public interface ISoundFileService
{
    /// <summary>
    /// Получить путь к файлу звука
    /// </summary>
    string GetSoundFilePath(Sound sound);
    
    /// <summary>
    /// Получить путь к файлу звука по названию пака 
    /// </summary>
    string GetSoundFilePath(string packName, string soundFileName);
    
    /// <summary>
    /// Сохранить файл звука
    /// </summary>
    Task SaveSoundFileAsync(Sound sound, byte[] fileData);
    
    /// <summary>
    /// Загрузить файл звука
    /// </summary>
    Task<byte[]> LoadSoundFileAsync(Sound sound);
    
    /// <summary>
    /// Удалить файл звука
    /// </summary>
    void DeleteSoundFile(Sound sound);
} 