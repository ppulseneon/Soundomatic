using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Soundomatic.Enums;

namespace Soundomatic.Converters;

public class BindingStatusToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not BindingStatus status)
        {
            return "Статус неизвестен";
        }

        return status switch
        {
            BindingStatus.NotAssigned => "Клавиша не назначена",
            BindingStatus.SoundPackInstalled => "Установлен набор",
            BindingStatus.SoundInstalled => "Установлен звук",
            BindingStatus.NoSoundSelected => "Звук не выбран",
            _ => "Неизвестный статус"
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}