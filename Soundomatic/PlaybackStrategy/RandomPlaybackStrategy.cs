using System;
using System.Collections.Generic;
using Soundomatic.Models;

namespace Soundomatic.PlaybackStrategy;

/// <summary>
/// Стратегия случайного выбора звука
/// </summary>
public class RandomPlaybackStrategy : IPlaybackStrategy
{
    /// <inheritdoc />
    public Sound SelectSound(IList<Sound> sounds)
    {
        throw new NotImplementedException();
    }
} 