namespace Soundomatic.Models.Enums;

/// <summary>
/// Форматы аудио-файлов
/// </summary>
public enum AudioFormat
{
    /// <summary>
    /// Waveform Audio File Format (.wav)
    /// </summary>
    Wav,

    /// <summary>
    /// MPEG-1 Audio Layer III (.mp3)
    /// </summary>
    Mp3,

    /// <summary>
    /// Audio Interchange File Format (.aiff, .aif)
    /// </summary>
    Aiff,

    /// <summary>
    /// Free Lossless Audio Codec (.flac)
    /// </summary>
    Flac,

    /// <summary>
    /// Ogg Vorbis (.ogg)
    /// </summary>
    Ogg,

    /// <summary>
    /// Advanced Audio Codec (.aac)
    /// </summary>
    Aac,

    /// <summary>
    /// Windows Media Audio (.wma)
    /// </summary>
    Wma,

    /// <summary>
    /// Apple Lossless Audio Codec (.m4a, .alac)
    /// </summary>
    Alac
}