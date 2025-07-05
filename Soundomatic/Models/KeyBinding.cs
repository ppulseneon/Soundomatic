using System.ComponentModel.DataAnnotations;
using SharpHook.Data;
using Soundomatic.Models.Base;

namespace Soundomatic.Models;

/// <summary>
/// Биндинг файла к звуку
/// </summary>
public class KeyBinding: BaseEntity
{
    /// <summary>
    /// Код назначенной клавиши
    /// </summary>
    [Required]
    public KeyCode Key { get; set; }

    /// <summary>
    /// Связанная группа звуков
    /// </summary>
    public SoundPack? Pack { get; set; }
    
    /// <summary>
    /// ID связанной группы звуков
    /// </summary>
    public long? PackId { get; init; }
}