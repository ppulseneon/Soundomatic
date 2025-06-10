using System.Collections.Generic;
using System.Threading.Tasks;
using Soundomatic.Models;

namespace Soundomatic.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы со звуками в базе данных
/// </summary>
public interface ISoundStorageService
{
    /// <summary>
    /// Получить все звуки
    /// </summary>
    Task<IEnumerable<Sound>> GetAllSoundsAsync();
    
    /// <summary>
    /// Получить звук по ID
    /// </summary>
    Task<Sound?> GetSoundByIdAsync(int id);
    
    /// <summary>
    /// Добавить новый звук
    /// </summary>
    Task<Sound> AddSoundAsync(Sound sound);
    
    /// <summary>
    /// Обновить существующий звук
    /// </summary>
    Task UpdateSoundAsync(Sound sound);
    
    /// <summary>
    /// Удалить звук
    /// </summary>
    Task DeleteSoundAsync(int id);
    
    /// <summary>
    /// Получить все группы звуков
    /// </summary>
    Task<IEnumerable<SoundPack>> GetAllSoundPacksAsync();
    
    /// <summary>
    /// Получить группу по ID
    /// </summary>
    Task<SoundPack?> GetSoundPackByIdAsync(int id);
    
    /// <summary>
    /// Добавить новую группу
    /// </summary>
    Task<SoundPack> AddSoundPackAsync(SoundPack pack);
    
    /// <summary>
    /// Обновить существующую группу
    /// </summary>
    Task UpdateSoundPackAsync(SoundPack pack);
    
    /// <summary>
    /// Удалить группу
    /// </summary>
    Task DeleteSoundPackAsync(int id);
} 