using System;

namespace Soundomatic.Exceptions;

public class LoadFileSettingsError: Exception
{
    /// <summary>
    /// Путь к файлу, который вызвал ошибку
    /// </summary>
    public string FilePath { get; }
    
    public LoadFileSettingsError(string filePath, string message)
        : base(message)
    {
        FilePath = filePath;
    }
}