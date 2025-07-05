using System.Threading.Tasks;
using DesktopNotifications;
using Soundomatic.Services.Interfaces;

namespace Soundomatic.Services;

/// <summary>
/// Сервис системных уведомлений
/// </summary>
public class SystemNotificationService(INotificationManager notificationManager): ISystemNotificationService
{
    public Task SendNotification(string title, string body)
    {
        var nf = new Notification
        {
            Title = title,
            Body = body,
        };

        notificationManager.ShowNotification(nf);
        return Task.CompletedTask;
    }
}