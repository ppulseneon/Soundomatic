using System;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;

namespace Soundomatic.Services;

public class SoundPlayer(GlobalSettings settings) : ISoundPlayer
{
    private WaveOutEvent _waveOut;
    
    public async Task PlaySoundAsync(Sound sound)
    {
        if (string.IsNullOrEmpty(sound.FilePath))
        {
            throw new ArgumentException("Путь к файлу звука не может быть пустым", nameof(sound.FilePath));
        }

        if (!File.Exists(sound.FilePath))
        {
            Console.WriteLine($"Файл звука не найден: {sound.FilePath}");
            return;
        }

        var volume = settings.Volume / 100.0f;
        
        await Task.Run(() =>
        {
            try
            {
                _waveOut = new WaveOutEvent();
                using var audioFile = new AudioFileReader(sound.FilePath);
                
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