using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Soundomatic.Storage.Context;

namespace Soundomatic.Storage.DatabaseInitialization;

/// <summary>
/// Класс для инициализации начальных данных в базу данных
/// </summary>
public class ApplicationDatabaseInitializer
{
    /// <summary>
    /// Инициализация начальных данных в базу данных
    /// </summary>
    public static async Task InitAsync(IServiceProvider scopeServiceProvider)
    {
        var applicationDbContext = scopeServiceProvider.GetRequiredService<ApplicationDbContext>();
        await applicationDbContext.Database.EnsureCreatedAsync();
    }
}
