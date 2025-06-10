using System.Collections.Generic;
using Soundomatic.Models;

namespace Soundomatic.Settings;

public class AppSettings
{
    /// <summary>
    /// Флаг, указывающий, включено ли воспроизведение звуков
    /// </summary>
    public bool IsSoundEnabled { get; set; } = true;
    
    /// <summary>
    /// Общая громкость звуков (от 0 до 100)
    /// </summary>
    public int Volume { get; set; } = 100;

    /// <summary>
    /// Флаг, указывающий, должно ли приложение запускаться автоматически при старте системы
    /// </summary>
    public bool AutoRun { get; set; } = true;

    public List<KeyBinding> KeyBindings { get; set; } = [];
}