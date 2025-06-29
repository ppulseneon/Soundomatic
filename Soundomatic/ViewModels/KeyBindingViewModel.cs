using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Soundomatic.Models;

namespace Soundomatic.ViewModels;

public partial class KeyBindingViewModel : ObservableObject
{
    public ObservableCollection<SoundPack> AllAvailablePacks { get; }

    [ObservableProperty]
    private SoundPack? _pack;

    public KeyBinding KeyBinding { get; }

    public KeyBindingViewModel(ObservableCollection<SoundPack> allAvailablePacks, KeyBinding keyBinding)
    {
        AllAvailablePacks = allAvailablePacks;
        KeyBinding = keyBinding;
        Pack = keyBinding.Pack;
    }

    partial void OnPackChanged(SoundPack? oldValue, SoundPack? newValue)
    {
        if (newValue == null) return;
        
        // todo: update database
        KeyBinding.Pack = newValue;
        Console.WriteLine($"Pack changed from {oldValue?.Name} to {newValue.Name}");
    }
}