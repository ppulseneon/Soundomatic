using Microsoft.EntityFrameworkCore;

namespace Soundomatic.Storage.Context;

/// <summary> 
/// Класс контекста базы данных приложения. 
/// </summary> 
/// <param name="options">Параметры контекста базы данных.</param> 
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    
}
