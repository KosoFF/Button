﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Button.Converters
{
    class BinaryImageConverter 
    {
       // object IValueConverter.Convert(object value,
       //Type targetType,
       //object parameter,
       //System.Globalization.CultureInfo culture)
       // {
       //     if (value != null && value is byte[])
       //     {
       //         byte[] bytes = value as byte[];

       //         MemoryStream stream = new MemoryStream(bytes);

       //         BitmapImage image = new BitmapImage();
       //         image.BeginInit();
       //         image.StreamSource = stream;
       //         image.EndInit();

       //         return image;
       //     }

       //     return null;
       // }

       // object IValueConverter.ConvertBack(object value,
       //     Type targetType,
       //     object parameter,
       //     System.Globalization.CultureInfo culture)
       // {
       //     throw new Exception("The method or operation is not implemented.");
       // }
    }
}
