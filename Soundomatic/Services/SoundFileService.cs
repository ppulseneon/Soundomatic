using System;
using System.IO;
using System.Threading.Tasks;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;

namespace Soundomatic.Services;

/// <summary>
/// Сервис для работы с файлами звуков в файловой системе
/// </summary>
public class SoundFileService : ISoundFileService
{
    private readonly string _soundsDirectory;
    
    public SoundFileService()
    {
        var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        
        // todo: вынести в настройки?
        _soundsDirectory = Path.Combine(appDirectory, "Sounds");
        
        CreateDirectoryIfNotExists(_soundsDirectory);
    }
    
    /// <inheritdoc />
    public string GetSoundFilePath(Sound sound) => GetSoundFilePath(sound.SoundPack.Name, sound.FileName);
    
    /// <inheritdoc />
    public string GetSoundFilePath(string packName, string soundFileName)
    {
        var packDirectory = Path.Combine(_soundsDirectory, packName);
        CreateDirectoryIfNotExists(packDirectory);
        return Path.Combine(packDirectory, soundFileName);
    }
    
    /// <inheritdoc />
    public async Task SaveSoundFileAsync(Sound sound, byte[] fileData)
    {
        var filePath = GetSoundFilePath(sound);
        await File.WriteAllBytesAsync(filePath, fileData);
    }
    
    /// <inheritdoc />
    public async Task<byte[]> LoadSoundFileAsync(Sound sound)
    {
        var filePath = GetSoundFilePath(sound);
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл звука не найден: {filePath}");
        }
        
        return await File.ReadAllBytesAsync(filePath);
    }
    
    /// <inheritdoc />
    public void DeleteSoundFile(Sound sound)
    {
        var filePath = GetSoundFilePath(sound);
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
    
    #region Local
    private static void CreateDirectoryIfNotExists(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
    #endregion local
} 