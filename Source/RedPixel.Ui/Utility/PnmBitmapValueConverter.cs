using System;
using System.Drawing;
using System.Globalization;
using Avalonia.Data.Converters;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace RedPixel.Ui.Utility;

public class PnmBitmapValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            null => null,
            Image image when targetType.IsAssignableFrom(typeof(Bitmap)) => image.ConvertToAvaloniaBitmap(),
            _ => throw new NotSupportedException()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}