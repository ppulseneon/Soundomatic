namespace Soundomatic.Enums;

/// <summary>
/// Статусы привязки клавиш
/// </summary>
public enum BindingStatus
{
    /// <summary>
    /// Не выбрана клавиша
    /// </summary>
    NotAssigned,
    
    /// <summary>
    /// Установлен звук
    /// </summary>
    SoundInstalled,
    
    /// <summary>
    /// Установлен набор звуков
    /// </summary>
    SoundPackInstalled,
    
    /// <summary>
    /// Звук не выбран
    /// </summary>
    NoSoundSelected,
}