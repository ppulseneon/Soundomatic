namespace Soundomatic.Models.Settings;

/// <summary>
/// Настройки базы данных
/// </summary>
public class DbSettings
{
    private const string DatabaseName = "soundomatic";
    
    public string ConnectionString { get; } = $"Data Source={DatabaseName}.db";
}