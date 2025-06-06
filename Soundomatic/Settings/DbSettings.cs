namespace Soundomatic.Settings;

/// <summary>
/// Настройки базы данных
/// </summary>
public class DbSettings
{
    private const string DatabaseName = "Soundomatic";
    
    public string ConnectionString { get; } = $"Data Source={DatabaseName}.db";
}