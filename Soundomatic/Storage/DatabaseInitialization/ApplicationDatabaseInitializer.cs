﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Platform;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharpHook.Data;
using SharpHook.Native;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;
using Soundomatic.Storage.Context;
using Soundomatic.Enums;

namespace Soundomatic.Storage.DatabaseInitialization;

/// <summary>
/// Класс для инициализации начальных данных в базу данных
/// </summary>
public static class ApplicationDatabaseInitializer
{
    private const string DefaultSoundPacksDirectory = "Assets/DefaultSoundPacks";

    /// <summary>
    /// Инициализация начальных данных в базу данных
    /// </summary>
    public static void Init(IServiceProvider scopeServiceProvider)
    {
        var applicationDbContext = scopeServiceProvider.GetRequiredService<ApplicationDbContext>();
        var soundStorageService = scopeServiceProvider.GetRequiredService<ISoundStorageService>();
        var soundFileService = scopeServiceProvider.GetRequiredService<ISoundFileService>();
        var keyBindingService = scopeServiceProvider.GetRequiredService<IKeyBindingService>();

        applicationDbContext.Database.Migrate();

        CreateDefaultPacksIfNeeded(soundStorageService, soundFileService);
        InitializeKeyBindings(keyBindingService, soundStorageService);
    }

    /// <summary>
    /// Инициализация биндов клавиш для стандартных паков
    /// </summary>
    private static void InitializeKeyBindings(IKeyBindingService keyBindingService,
        ISoundStorageService soundStorageService)
    {
        var existingBindings = keyBindingService.GetAllKeyBindingsAsync().GetAwaiter().GetResult();

        if (existingBindings.Any()) return;
        
        var packs = soundStorageService.GetAllSoundPacksAsync().GetAwaiter().GetResult();
        var packsList = packs.ToList();

        var maxIterations = Math.Min(packsList.Count, 12);

        for (var i = 1; i <= maxIterations; i++)
        {
            var keyCode = (KeyCode)((int)KeyCode.VcF1 + (i - 1));

            var pack = i <= packsList.Count ? packsList[i - 1] : null;

            var keyBinding = new KeyBinding
            {
                Key = keyCode,
                PackId = pack.Id,
                CreatedAt = DateTime.UtcNow
            };

            keyBindingService.AddKeyBindingAsync(keyBinding).GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// Запись в базу стандартных паков, если это нужно
    /// </summary>
    private static void CreateDefaultPacksIfNeeded(ISoundStorageService soundStorageService,
        ISoundFileService soundFileService)
    {
        var packs = soundStorageService.GetAllSoundPacksAsync().GetAwaiter().GetResult();

        if (!packs.Any())
        {
            CreateDefaultPacks(soundStorageService, soundFileService);
        }
    }

    /// <summary>
    /// Создание дефолтных паков
    /// </summary>
    private static void CreateDefaultPacks(ISoundStorageService soundStorageService, ISoundFileService soundFileService)
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        var assetsDirectory = $"avares://{assemblyName}/{DefaultSoundPacksDirectory}";

        var assetsList = AssetLoader.GetAssets(new Uri(assetsDirectory), null);

        var packGroups = assetsList
            .Select(assetPath => assetPath.ToString())
            .Where(path => path.StartsWith(assetsDirectory))
            .GroupBy(path => path.Split('/')[5])
            .ToList();

        foreach (var packGroup in packGroups)
        {
            var packName = packGroup.Key;

            var packs = soundStorageService.GetAllSoundPacksAsync().GetAwaiter().GetResult();
            var pack = packs.FirstOrDefault(g => g.Name == packName);

            if (pack == null)
            {
                pack = new SoundPack
                {
                    Name = packName,
                    IsDefault = true,
                };

                pack = soundStorageService.AddSoundPackAsync(pack).GetAwaiter().GetResult();
            }

            var logoPath = packGroup.FirstOrDefault(path =>
                path.EndsWith($"{packName}_icon.png", StringComparison.OrdinalIgnoreCase) ||
                path.EndsWith($"{packName}_icon.jpg", StringComparison.OrdinalIgnoreCase));

            if (logoPath != null)
            {
                try
                {
                    var uri = new Uri(logoPath);
                    using var logoStream = AssetLoader.Open(uri);

                    var buffer = new byte[4096];
                    using var memoryStream = new MemoryStream();

                    int bytesRead;
                    while ((bytesRead = logoStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, bytesRead);
                    }

                    if (memoryStream.Length > 0)
                    {
                        pack.IconData = memoryStream.ToArray();
                        pack.IconMimeType = GetMimeType(Path.GetExtension(logoPath));
                        soundStorageService.UpdateSoundPackAsync(pack).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке логотипа {logoPath}: {ex}");
                }
            }

            var soundFiles = packGroup
                .Where(path => path.Contains($"{packName.ToLower()}_sound_"))
                .OrderBy(path =>
                {
                    var fileName = Path.GetFileNameWithoutExtension(path);
                    var match = System.Text.RegularExpressions.Regex.Match(fileName, @"_(\d+)$");
                    return match.Success && int.TryParse(match.Groups[1].Value, out var index) ? index : 0;
                })
                .ToList();

            foreach (var soundPath in soundFiles)
            {
                var fileName = Path.GetFileName(soundPath);
                var fileExtension = Path.GetExtension(soundPath).ToLowerInvariant();

                var existingSounds = soundStorageService.GetAllSoundsAsync().GetAwaiter().GetResult();
                if (existingSounds.Any(s => s.FileName == fileName))
                {
                    continue;
                }

                using var soundStream = AssetLoader.Open(new Uri(soundPath));
                using var soundMemoryStream = new MemoryStream();
                soundStream.CopyTo(soundMemoryStream);
                
                var sound = new Sound
                {
                    Name = $"{packName} {Path.GetFileNameWithoutExtension(soundPath).Split('_').Last()}",
                    FilePath = soundFileService.GetSoundFilePath(packName, fileName),
                    FileName = fileName,
                    AudioFormat = GetAudioFormat(fileExtension),
                    SoundPackId = pack.Id,
                    FileSizeBytes = soundMemoryStream.Length
                };

                sound = soundStorageService.AddSoundAsync(sound).GetAwaiter().GetResult();
                
                var fileData = soundMemoryStream.ToArray();

                Task.Run(async () =>
                {
                    await soundFileService.SaveSoundFileAsync(sound, fileData);
                });
            }
        }
    }

    private static string GetMimeType(string extension)
    {
        return extension.ToLower() switch
        {
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            _ => "application/octet-stream"
        };
    }

    private static AudioFormat GetAudioFormat(string extension)
    {
        return extension.ToLower() switch
        {
            ".mp3" => AudioFormat.Mp3,
            ".wav" => AudioFormat.Wav,
            ".ogg" => AudioFormat.Ogg,
            ".flac" => AudioFormat.Flac,
            ".aac" => AudioFormat.Aac,
            ".wma" => AudioFormat.Wma,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}