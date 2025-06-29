using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    [Required]
    public SoundPack Pack { get; set; } = null!;
    
    /// <summary>
    /// ID связанной группы звуков
    /// </summary>
    public long PackId { get; set; }
    
    /// <summary>
    /// Список всех доступных паков для привязки
    /// </summary>
    [NotMapped]
    public ObservableCollection<SoundPack>? AllAvailablePacks { get; set; }
}