﻿using System;
using System.Globalization;
using System.Net;

namespace Demo.Maui.Controls.Converters;

public class UriToImageSourceConverter : IValueConverter
{
    private static WebClient _client = new WebClient();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return null;

        var byteArray = _client.DownloadData(value.ToString());

        return ImageSource.FromStream(() => new MemoryStream(byteArray));

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

