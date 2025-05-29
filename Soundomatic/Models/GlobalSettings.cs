using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Soundomatic.Models;

public class GlobalSettings
{
    /// <summary>
    /// Привязки клавиш
    /// </summary>
    public List<KeyBinding> KeyBindings { get; set; } = [];
    
    /// <summary>
    /// Флаг, указывающий, включены ли звуки
    /// </summary>
    public bool IsSoundEnabled { get; set; } = true;
    
    /// <summary>
    /// Громкость звуков (от 0 до 100)
    /// </summary>
    public int Volume { get; set; } = 100;

    /// <summary>
    /// Флаг, указывающий, должно ли приложение запускаться автоматически при старте системы
    /// </summary>
    public bool AutoRun { get; set; } = true;
    
    public void Save(string filePath)
    {
        #pragma warning disable CA1869
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public static GlobalSettings Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new GlobalSettings();
        }

        var json = File.ReadAllText(filePath);
        var settings = JsonSerializer.Deserialize<GlobalSettings>(json);

        if (settings == null)
        {
            // todo: Отобразить уведомление об ошибке (LoadFileSettingsError), но не выходить из метода
        }
        
        return new GlobalSettings();
    }
}