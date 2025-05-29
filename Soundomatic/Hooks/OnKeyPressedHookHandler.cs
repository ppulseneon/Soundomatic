using System;
using System.Linq;
using System.Threading.Tasks;
using SharpHook;
using Soundomatic.Models;
using Soundomatic.Services.Interfaces;

namespace Soundomatic.Hooks;

public class OnKeyPressedHookHandler
{
    public event EventHandler<KeyboardHookEventArgs> KeyPressed;
    private readonly ISoundPlayer _soundPlayer;
    private readonly GlobalSettings _settings;
    private readonly TaskPoolGlobalHook _hook;
    
    public OnKeyPressedHookHandler(GlobalSettings settings, ISoundPlayer soundPlayer)
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

    private async void OnKeyPressed(object sender, KeyboardHookEventArgs e)
    {
        KeyPressed?.Invoke(this, e);

        if (!_settings.IsSoundEnabled) return;
        
        Console.WriteLine($"Нажата клавиша: {e.Data.KeyCode}");
        
        var binding = _settings.KeyBindings.FirstOrDefault(kb => kb.Key == e.Data.KeyCode);
        if (binding == null) return;
        
        await _soundPlayer.PlaySoundAsync(binding.Sound);
        Console.WriteLine($"Нажата клавиша: {e.Data.KeyCode}, воспроизводится звук: {binding.Sound.Name}");
    }

    public void Dispose()
    {
        Stop();
        _hook.KeyPressed -= OnKeyPressed;
    }
}