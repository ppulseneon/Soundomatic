using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Soundomatic.ViewModels;

namespace Soundomatic;

/// <summary>
/// Класс для поиска и сопоставления представлений (View) с моделями представлений (ViewModel)
/// </summary>
public class ViewLocator : IDataTemplate
{
    /// <summary>
    /// Создает и возвращает View, соответствующий ViewModel
    /// </summary>
    /// <param name="param">Объект данных ViewModel, для которого нужно создать View</param>
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    /// <summary>
    /// Определяет, может ли этот шаблон данных обработать указанный объект
    /// </summary>
    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}