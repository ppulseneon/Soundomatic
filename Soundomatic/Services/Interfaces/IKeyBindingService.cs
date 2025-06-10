using System.Collections.Generic;
using System.Threading.Tasks;
using Soundomatic.Models;

namespace Soundomatic.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с привязками клавиш
/// </summary>
public interface IKeyBindingService
{
    /// <summary>
    /// Получить все привязки клавиш
    /// </summary>
    Task<IEnumerable<KeyBinding>> GetAllKeyBindingsAsync();
    
    /// <summary>
    /// Получить привязку клавиши по ID
    /// </summary>
    Task<KeyBinding?> GetKeyBindingByIdAsync(int id);
    
    /// <summary>
    /// Получить привязку клавиши по названию клавиши
    /// </summary>
    Task<KeyBinding?> GetKeyBindingByKeyAsync(string key);
    
    /// <summary>
    /// Добавить новую привязку клавиши
    /// </summary>
    Task<KeyBinding> AddKeyBindingAsync(KeyBinding keyBinding);
    
    /// <summary>
    /// Обновить привязку клавиши
    /// </summary>
    Task UpdateKeyBindingAsync(KeyBinding keyBinding);
    
    /// <summary>
    /// Удалить привязку клавиши
    /// </summary>
    Task DeleteKeyBindingAsync(int id);
} 