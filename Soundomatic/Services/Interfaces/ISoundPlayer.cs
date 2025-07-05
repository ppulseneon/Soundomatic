using System;
using System.Threading.Tasks;
using Soundomatic.Models;

namespace Soundomatic.Services.Interfaces;

public interface ISoundPlayer : IDisposable
{
    Task PlayAsync(SoundPack pack);
}