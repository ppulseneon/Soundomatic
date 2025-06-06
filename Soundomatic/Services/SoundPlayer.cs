using System;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;
using Soundomatic.Settings;

namespace Soundomatic.Services;

/// <summary>
/// Плеер для воспроизведения звуков
/// </summary>
public class SoundPlayer : ISoundPlayer
{
    private readonly AppSettings _settings;
    private readonly ISoundFileService _fileService;
    private WaveOutEvent _waveOut;
    
    public SoundPlayer(AppSettings settings, ISoundFileService fileService)
    {
        _settings = settings;
        _fileService = fileService;
    }
    
    /// <summary>
    /// Воспроизвести указанный звук
    /// </summary>
    /// <param name="sound">Звук для воспроизведения</param>
    /// <exception cref="FileNotFoundException">Ошибка, если файл недоступен</exception>
    public async Task PlaySoundAsync(Sound sound)
    {
        var filePath = _fileService.GetSoundFilePath(sound);
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл звука не найден: {filePath}");
        }

        var volume = _settings.Volume * sound.Volume / 100.0f;
        
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка воспроизведения звука: {ex.Message}");
            }
        });
    }

    public void Dispose()
    {
        _waveOut.Stop();
        _waveOut.Dispose();
    }
}