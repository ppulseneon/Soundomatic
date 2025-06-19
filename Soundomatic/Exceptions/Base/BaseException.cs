using System;
using System.Threading.Tasks;

namespace Soundomatic.Exceptions.Base;

/// <summary>
/// Абстрактный класс базовой ошибки, наследуемый от <see cref="Exception"/> 
/// </summary>
public class BaseException: Exception
{
    /// <summary>
    /// Конструктор с сообщением об ошибке
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    protected BaseException(string message) : base(message) { }
    
    /// <summary>
    /// Метод обработки ошибки
    /// </summary>
    /// <param name="e">Ошибка</param>
    /// <returns>Результат задачи</returns>
    protected virtual Task HandleAsync(Exception e)
    {
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Фабричный метод для вызова ошибки и её обработчика с дефолтным сообщением 
    /// </summary>
    public static async void ThrowAsync<T>() where T : BaseException, new()
    {
        var exception = new T();
        await exception.HandleAsync(exception);
    }
    
    /// <summary>
    /// Фабричный метод для вызова ошибки и её обработчика 
    /// </summary>
    /// <param name="message"></param>
    public static async void ThrowAsync<T>(string message) where T : BaseException
    {
        var exception = (T)Activator.CreateInstance(typeof(T), message)!;
        await exception.HandleAsync(exception);
    }
}