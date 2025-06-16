using System;
using DesktopNotifications;
using DesktopNotifications.FreeDesktop;
using DesktopNotifications.Windows;

namespace Soundomatic.Extensions.Factories;

/// <summary>
/// Фабрика для создания менеджера системных уведомлений.
/// </summary>
public static class NotificationManagerFactory
{
    /// <summary>
    /// Создает экземпляр менеджера уведомлений в зависимости от операционной системы.
    /// </summary>
    /// <returns>Экземпляр менеджера уведомлений.</returns>
    public static INotificationManager CreateNotificationManager()
    {
        INotificationManager manager;

        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Win32NT:
                var win32Context = WindowsApplicationContext.FromCurrentProcess();
                manager = new WindowsNotificationManager(win32Context);
                break;
            case PlatformID.Unix:
                var unixContext = FreeDesktopApplicationContext.FromCurrentProcess();
                manager = new FreeDesktopNotificationManager(unixContext);
                break;
            default:
                throw new PlatformNotSupportedException();
        }

        return manager;
    }
}