using System.Text;
using Newtonsoft.Json;

namespace Demo.Maui.Extensions;

public static class HttpExtensions
{
    #region Constants

    private const char _PATH_DELIMITER = '/';

    private const string _APPLICATION_JSON = "application/json";

    #endregion

    #region Methods

    public static UriBuilder AppendUrlSegment(this UriBuilder builder, string path)
    {
        builder.Path = Combine(builder.Path, path);
        return builder;
    }

    public static void AddJsonContent(this HttpRequestMessage message, object data)
    {
        if (data == null)
            return;

        var json = JsonConvert.SerializeObject(data);
        message.Content = new StringContent(json, Encoding.UTF8, _APPLICATION_JSON);
    }

    public static T DeserializeJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);

    private static string Combine(string path, string relative)
    {
        relative ??= string.Empty;

        path ??= string.Empty;

        switch (relative.Length)
        {
            case 0 when path.Length == 0:
                return string.Empty;
            case 0:
                return path;
        }

        if (path.Length == 0)
            return relative;

        path = path.Replace('\\', _PATH_DELIMITER);
        relative = relative.Replace('\\', _PATH_DELIMITER);

        return path.TrimEnd(_PATH_DELIMITER) + _PATH_DELIMITER + relative.TrimStart(_PATH_DELIMITER);
    }
    #endregion
}

