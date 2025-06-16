using Soundomatic.Exceptions.Base;

namespace Soundomatic.Exceptions;

/// <summary>
/// Ошибка запуска приложения
/// </summary>
/// <param name="message">Сообщение об ошибке</param>
public abstract class StartupException(string message) : SystemNotificationTypeException(message);