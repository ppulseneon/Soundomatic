using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;
using Soundomatic.Storage.Context;

namespace Soundomatic.Services;

/// <summary>
/// Сервис для работы со звуками в базе данных
/// </summary>
public class SoundStorageService(ApplicationDbContext dbContext) : ISoundStorageService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Sound>> GetAllSoundsAsync() => await dbContext.Sounds
        .Include(s => s.SoundPack)
        .ToListAsync();
    
    /// <inheritdoc />
    public async Task<Sound?> GetSoundByIdAsync(int id) => await dbContext.Sounds
        .Include(s => s.SoundPack)
        .FirstOrDefaultAsync(s => s.Id == id);
    
    /// <inheritdoc />
    public async Task<Sound> AddSoundAsync(Sound sound)
    {
        var group = await dbContext.SoundPacks.FindAsync(sound.SoundPackId);
        if (group == null)
        {
            throw new InvalidOperationException($"Группа с ID {sound.SoundPackId} не найдена");
        }
        
        dbContext.Sounds.Add(sound);
        await dbContext.SaveChangesAsync();
        return sound;
    }
    
    /// <inheritdoc />
    public async Task UpdateSoundAsync(Sound sound)
    {
        dbContext.Sounds.Update(sound);
        await dbContext.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task DeleteSoundAsync(int id)
    {
        var sound = await dbContext.Sounds.FindAsync(id);
        
        if (sound != null)
        {
            dbContext.Sounds.Remove(sound);
            await dbContext.SaveChangesAsync();
        }
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<SoundPack>> GetAllSoundPacksAsync() => await dbContext.SoundPacks
        .Include(g => g.Sounds)
        .ToListAsync();
    
    /// <inheritdoc />
    public async Task<SoundPack?> GetSoundPackByIdAsync(int id)
    {
        return await dbContext.SoundPacks
            .Include(g => g.Sounds)
            .FirstOrDefaultAsync(g => g.Id == id);
    }
    
    /// <inheritdoc />
    public async Task<SoundPack> AddSoundPackAsync(SoundPack pack)
    {
        dbContext.SoundPacks.Add(pack);
        await dbContext.SaveChangesAsync();
        return pack;
    }
    
    /// <inheritdoc />
    public async Task UpdateSoundPackAsync(SoundPack pack)
    {
        dbContext.SoundPacks.Update(pack);
        await dbContext.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task DeleteSoundPackAsync(int id)
    {
        var group = await dbContext.SoundPacks.FindAsync(id);
        if (group != null)
        {
            dbContext.SoundPacks.Remove(group);
            await dbContext.SaveChangesAsync();
        }
    }
} 