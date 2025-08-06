using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;

namespace Soundomatic.Converters
{
    public class SoundEnabledToIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var uriVolume = (value is bool volumeOffOnBool && volumeOffOnBool) ? "avares://Soundomatic/Assets/TrayIcon/Volume.png" :
                                                                                 "avares://Soundomatic/Assets/TrayIcon/OffVolume.png";

            return new Bitmap(AssetLoader.Open(new Uri(uriVolume)));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}