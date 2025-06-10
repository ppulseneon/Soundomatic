using System;
using System.IO;
using System.Xml.Serialization;
using Soundomatic.Settings;

namespace Soundomatic.Storage;

/// <summary>
/// Класс для работы с настройками приложения в XML файле
/// </summary>
public class FileSettingsStorage
{
    private const string DefaultSettingsFileName = "AppSettings.xml";
    private readonly string _settingsPath;

    /// <summary>
    /// Инициализирует новый экземпляр класса FileSettingsStorage
    /// </summary>
    public FileSettingsStorage()
    {
        var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _settingsPath = Path.Combine(appDirectory, DefaultSettingsFileName);
    }

    /// <summary>
    /// Сохраняет настройки в XML файл
    /// </summary>
    /// <param name="settings">Настройки приложения для сохранения</param>
    public void Save(AppSettings settings)
    {
        var serializer = new XmlSerializer(typeof(AppSettings));
        using var writer = new StreamWriter(_settingsPath);
        serializer.Serialize(writer, settings);
    }

    /// <summary>
    /// Загружает настройки из XML файла. Если файл не существует, создает новый с настройками по умолчанию
    /// </summary>
    public AppSettings Load()
    {
        if (!File.Exists(_settingsPath))
        {
            var defaultSettings = new AppSettings();
            Save(defaultSettings);
            return defaultSettings;
        }

        try
        {
            var serializer = new XmlSerializer(typeof(AppSettings));
            using var reader = new StreamReader(_settingsPath);
            var settings = (AppSettings)serializer.Deserialize(reader);
            return settings ?? new AppSettings();
        }
        catch (Exception ex)
        {
            return new AppSettings();
        }
    }
}