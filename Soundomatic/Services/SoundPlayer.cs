using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using Soundomatic.Models;
using Soundomatic.Playback;
using Soundomatic.Services.Interfaces;
using Soundomatic.Settings;

namespace Soundomatic.Services;

/// <summary>
/// Плеер для воспроизведения звуков
/// </summary>
public class SoundPlayer(AppSettings appSettings, ISoundFileService fileService, ILogger<SoundPlayer> logger)
    : ISoundPlayer
{
    private readonly Dictionary<string, IPlaybackStrategy> _strategyCache = new();
    private WaveOutEvent _waveOut = null!;

    public async Task PlayAsync(SoundPack pack)
    {
        var strategy = GetOrCreateStrategy(pack);
        var sound = strategy.SelectSound(pack.Name, pack.Sounds.ToList());
        await PlaySoundAsync(sound);
    }
    
    /// <summary>
    /// Воспроизвести указанный звук
    /// </summary>
    /// <param name="sound">Звук для воспроизведения</param>
    /// <exception cref="FileNotFoundException">Ошибка, если файл недоступен</exception>
    private async Task PlaySoundAsync(Sound sound)
    {
        var filePath = fileService.GetSoundFilePath(sound);
        
        if (!File.Exists(filePath))
        {
            // todo: переделать под box error message 
            throw new FileNotFoundException($"Файл звука не найден: {filePath}");
        }

        var volume = appSettings.Volume * sound.Volume / 100.0f;
        
        await Task.Run(() =>
        {
            try
            {
                _waveOut = new WaveOutEvent();
                using var audioFile = new AudioFileReader(filePath);
                audioFile.Volume = volume;
                
                _waveOut.Init(audioFile);
                _waveOut.Play();
                
                while (_waveOut.PlaybackState == PlaybackState.Playing)
                {
                    Task.Delay(50).Wait();
                }
                
                logger.LogInformation("Playing sound: {sound}", sound.Name);
            }
            catch (Exception ex)
            {
                logger.LogError("Audio playback error: {error}", ex.Message);
            }
        });
    }

    /// <summary>
    /// Метод для работы с кешем стратегий 
    /// </summary>
    private IPlaybackStrategy GetOrCreateStrategy(SoundPack pack)
    {
        var cacheKey = $"{pack.Name}_{pack.PlaybackStrategyType}";

        if (_strategyCache.TryGetValue(cacheKey, out var strategy)) return strategy;
        
        strategy = pack.CreateStrategy();
        _strategyCache[cacheKey] = strategy;

        return strategy;
    }
    
    public void Dispose()
    {
        _waveOut.Stop();
        _waveOut.Dispose();
    }
}