using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Platform;
using RedPixel.Core;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace RedPixel.Ui.Utility;

public class PnmBitmapValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return null;

        if (value is Image image && targetType.IsAssignableFrom(typeof(Bitmap)))
        {
            return image.ConvertToAvaloniaBitmap();
        }

        throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}