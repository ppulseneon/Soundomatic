using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharpHook;
using Soundomatic.Models;
using Soundomatic.Playback;
using Soundomatic.Services.Interfaces;
using Soundomatic.Settings;
using Soundomatic.Storage.Context;

namespace Soundomatic.Hooks;

/// <summary>
/// Глобальный обработчик нажатия клавиши клавиатуры
/// </summary>
public class OnKeyPressedHookHandler : IDisposable
{
    public event EventHandler<KeyboardHookEventArgs> KeyPressed;
    private readonly Dictionary<string, IPlaybackStrategy> _strategyCache;
    private readonly ApplicationDbContext _context;
    private readonly ISoundPlayer _soundPlayer;
    private readonly AppSettings _settings;
    private readonly TaskPoolGlobalHook _hook;
    
    public OnKeyPressedHookHandler(ApplicationDbContext context, AppSettings settings, ISoundPlayer soundPlayer)
    {
        _context = context;
        _hook = new TaskPoolGlobalHook();
        _hook.KeyPressed += OnKeyPressed;
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _soundPlayer = soundPlayer ?? throw new ArgumentNullException(nameof(soundPlayer));
        _strategyCache = new Dictionary<string, IPlaybackStrategy>();
    }

    public async Task StartAsync()
    {
        await _hook.RunAsync();
    }

    private void Stop()
    {
        _hook.Stop();
    }

    /// <summary>
    /// Обработчик события нажатия клавиши клавиатуры
    /// </summary>
    private async void OnKeyPressed(object sender, KeyboardHookEventArgs e)
    {
        // Проверяем, включено ли воспроизведение звуков
        if (!_settings.IsSoundEnabled) return;

        var binding = _context.KeyBindings.Include(keyBinding => keyBinding.Pack)
            .ThenInclude(soundPack => soundPack.Sounds).FirstOrDefault(kb => kb.Key == e.Data.KeyCode);

        if (binding?.Pack == null) return;

        var strategy = GetOrCreateStrategy(binding.Pack);
        var sound = strategy.SelectSound(binding.Pack.Name, binding.Pack.Sounds.ToList());

        if (sound == null) return;

        await _soundPlayer.PlaySoundAsync(sound);
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
        Stop();
        _hook.KeyPressed -= OnKeyPressed;
        _hook.Dispose();
        _strategyCache.Clear();
    }
}