using System;
using System.Threading.Tasks;
using DesktopNotifications;
using Soundomatic.Extensions.Factories;

namespace Soundomatic.Exceptions.Base;

/// <summary>
/// Класс типа ошибки для вывода в уведомления в систему
/// </summary>
public class SystemNotificationTypeException: BaseException
{
    /// <inheritdoc />
    protected SystemNotificationTypeException(string message) : base(message){}

    /// <inheritdoc />
    protected override Task HandleAsync(Exception e)
    {
        var notificationManager = NotificationManagerFactory.CreateNotificationManager();
            
        var nf = new Notification
        {
            Title = "Application Error",
            Body = "Application cant load",
        };

        notificationManager.ShowNotification(nf);
        return Task.CompletedTask;
    }
}