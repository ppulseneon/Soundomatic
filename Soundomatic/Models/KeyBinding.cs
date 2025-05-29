using SharpHook.Data;

namespace Soundomatic.Models;

/// <summary>
/// Биндинг файла к звуку
/// </summary>
public class KeyBinding
{
    /// <summary>
    /// Код назначенной клавиши
    /// </summary>
    public required KeyCode Key { get; set; }

    /// <summary>
    /// Звук, проигрываемый при нажатии
    /// </summary>
    public required Sound Sound { get; set; }
}