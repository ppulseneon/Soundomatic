using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;
using Soundomatic.Storage.Context;

namespace Soundomatic.Services;

/// <summary>
/// Сервис для работы с привязками клавиш
/// </summary>
public class KeyBindingService(ApplicationDbContext dbContext) : IKeyBindingService
{
    /// <inheritdoc />
    public async Task<IEnumerable<KeyBinding>> GetAllKeyBindingsAsync()
    {
        return await dbContext.KeyBindings.ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<KeyBinding?> GetKeyBindingByIdAsync(int id)
    {
        return await dbContext.KeyBindings.FindAsync(id);
    }
    
    /// <inheritdoc />
    public async Task<KeyBinding?> GetKeyBindingByKeyAsync(string key)
    {
        return await dbContext.KeyBindings.FirstOrDefaultAsync(k => k.Key.ToString() == key);
    }
    
    /// <inheritdoc />
    public async Task<KeyBinding> AddKeyBindingAsync(KeyBinding keyBinding)
    {
        dbContext.KeyBindings.Add(keyBinding);
        await dbContext.SaveChangesAsync();
        return keyBinding;
    }
    
    /// <inheritdoc />
    public async Task UpdateKeyBindingAsync(KeyBinding keyBinding)
    {
        dbContext.KeyBindings.Update(keyBinding);
        await dbContext.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task DeleteKeyBindingAsync(int id)
    {
        var keyBinding = await dbContext.KeyBindings.FindAsync(id);
        if (keyBinding != null)
        {
            dbContext.KeyBindings.Remove(keyBinding);
            await dbContext.SaveChangesAsync();
        }
    }
} 