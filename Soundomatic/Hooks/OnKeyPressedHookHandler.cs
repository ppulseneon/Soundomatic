using System;
using System.Linq;
using System.Threading.Tasks;
using SharpHook;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;
using Soundomatic.Settings;

namespace Soundomatic.Hooks;

/// <summary>
/// Глобальный обработчик нажатия клавиши клавиатуры
/// </summary>
public class OnKeyPressedHookHandler : IDisposable
{
    public event EventHandler<KeyboardHookEventArgs> KeyPressed;
    private readonly ISoundPlayer _soundPlayer;
    private readonly AppSettings _settings;
    private readonly TaskPoolGlobalHook _hook;
    
    public OnKeyPressedHookHandler(AppSettings settings, ISoundPlayer soundPlayer)
    {
        _hook = new TaskPoolGlobalHook();
        _hook.KeyPressed += OnKeyPressed;
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _soundPlayer = soundPlayer ?? throw new ArgumentNullException(nameof(soundPlayer));
    }

    public async Task StartAsync()
    {
        await _hook.RunAsync();
    }

    public void Stop()
    {
        _hook.Stop();
    }

    /// <summary>
    /// Обработчик события нажатия клавиши клавиатуры
    /// </summary>
    private async void OnKeyPressed(object sender, KeyboardHookEventArgs e)
    {
        if (!_settings.IsSoundEnabled) return;
        
        // var binding = _settings.KeyBindings.FirstOrDefault(kb => kb.Key == e.Data.KeyCode);
        //
        // var soundToPlay = binding?.Pack.GetNextSoundToPlay();
        //
        // if (soundToPlay == null) return;
        //
        // try
        // {
        //     await _soundPlayer.PlaySoundAsync(soundToPlay);
        //     Console.WriteLine($"Key Pressed: {e.Data.KeyCode}, Playing sound: {soundToPlay.Name} from pack: {binding.Pack.Name}. Strategy: {binding.Pack.StrategyType}");
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"Error playing sound {soundToPlay.Name}: {ex.Message}");
        // }
    }

    public void Dispose()
    {
        Stop();
        _hook.KeyPressed -= OnKeyPressed;
        _hook.Dispose();
    }
}