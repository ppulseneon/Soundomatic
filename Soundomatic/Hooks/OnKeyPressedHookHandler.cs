using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharpHook;
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

        await _soundPlayer.PlayAsync(binding.Pack);
    }
    
    public void Dispose()
    {
        Stop();
        _hook.KeyPressed -= OnKeyPressed;
        _hook.Dispose();
    }
}