using Soundomatic.Exceptions.Base;

namespace Soundomatic.Exceptions;

/// <summary>
/// Ошибка запуска приложения
/// </summary>
public class StartupException(string message) : SystemNotificationTypeException(message)
{
    private const string DefaultMessage = "Startup exception";
    
    public StartupException() : this(DefaultMessage) { }
}