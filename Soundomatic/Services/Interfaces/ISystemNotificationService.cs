using System.Threading.Tasks;

namespace Soundomatic.Services.Interfaces;

/// <summary>
/// Интерфейс для сервиса системных уведомлений
/// </summary>
public interface ISystemNotificationService
{
    /// <summary>
    /// Метод для создания системного уведомления
    /// </summary>
    /// <param name="title">Заголовок</param>
    /// <param name="body">Текст уведомления</param>
    public Task SendNotification(string title, string body);
}