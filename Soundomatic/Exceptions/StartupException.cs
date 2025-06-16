using Soundomatic.Exceptions.Base;

namespace Soundomatic.Exceptions;

/// <summary>
/// Ошибка запуска приложения
/// </summary>
/// <param name="message">Сообщение об ошибке</param>
public class StartupException(string message) : SystemNotificationTypeException(message);