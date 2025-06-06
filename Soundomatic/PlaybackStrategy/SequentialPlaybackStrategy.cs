using System;
using System.Collections.Generic;
using Soundomatic.Models;

namespace Soundomatic.PlaybackStrategy;

/// <summary>
/// Стратегия последовательного выбора звука
/// </summary>
public class SequentialPlaybackStrategy : IPlaybackStrategy
{
    /// <inheritdoc />
    public Sound SelectSound(IList<Sound> sounds)
    {
        throw new NotImplementedException();
    }
} 