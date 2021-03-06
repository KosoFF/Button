﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Button.Core.Helpers
{
    public static class ImageHelper
    {
        public static async Task<ImageSource> CreateBitmapImage(byte[] imageBytes, int? decodePixelWidth = null, int? decodePixelHeight = null)
        {
            if (imageBytes == null) return null;

            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(imageBytes.AsBuffer());
                stream.Seek(0);

                return CreateBitmapImage(stream, decodePixelWidth, decodePixelHeight);
            }
        }

        public static ImageSource CreateBitmapImage(IRandomAccessStream imageBytesStream, int? decodePixelWidth = null, int? decodePixelHeight = null)
        {
            if (imageBytesStream == null) return null;

            try
            {
                var bitmapImage = new BitmapImage();
                if (decodePixelWidth.HasValue) bitmapImage.DecodePixelWidth = decodePixelWidth.Value;
                if (decodePixelHeight.HasValue) bitmapImage.DecodePixelHeight = decodePixelHeight.Value;
                bitmapImage.SetSource(imageBytesStream);
                return bitmapImage;
            }
            catch (Exception ex)
            {
               
               Debug.Write("Helpers class. Failed to create BitmapImage. " + ex);
                return null;
            }
        }
    }
}
