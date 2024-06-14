using System.Diagnostics;
using System.Net;
using System.Text;
using System.Xml;
using Demo.Maui.Models;
using Demo.Maui.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Demo.Maui.Services.Base
{
    public abstract class BaseHttpClient
	{
        #region Fields

        private HttpClient _httpClient = new ();

        #endregion

        #region Constructor

        public BaseHttpClient(AppConfig config)
        {
            BaseAddress = $"{config.BaseUrl}{config.ApiVersion}";

            ApiKey = config.ApiKey;

            InitClient();
		}

        #endregion

        #region Properties

        protected string BaseAddress { get; }
        protected string ApiKey { get; }

        #endregion

        #region Http Methods

        protected async Task<RestResponse<T>> RequestAsync<T>(HttpRequestMessage message)
        {
            HttpResponseMessage httpResponse = null;
            string content = null;

            try
            {
                await PrintRequestToConsole(message);

                httpResponse = await _httpClient.SendAsync(message);
                content = await httpResponse.Content.ReadAsStringAsync();

                PrintResponseToConsole(httpResponse, content);

                httpResponse.EnsureSuccessStatusCode();

                var response = new RestResponse<T>
                {
                    StatusCode = httpResponse.StatusCode,
                    Message = httpResponse.ReasonPhrase,
                    ResponseHeaders = httpResponse.Headers,
                    Content = httpResponse.Content,
                    Data = await Task.Run(() => JsonConvert.DeserializeObject<T>(content))
                };

                return response;

            }
            catch (Exception ex)
            {
                //TODO: log exception

                return new RestResponse<T>
                {
                    StatusCode = httpResponse?.StatusCode != null ? httpResponse.StatusCode : HttpStatusCode.Gone,
                    Message = ex.Message,
                    ResponseHeaders = null,
                    Content = null,
                    Data = default
                };
            }
        }

        #endregion

        #region Helper Methods

        private void InitClient()
        {
            _httpClient.Dispose();

            _httpClient = new HttpClient();

            _httpClient.Timeout = TimeSpan.FromMilliseconds(350000);
        }

        private async Task PrintRequestToConsole(HttpRequestMessage request)
        {
#if DEBUG
            var str = await GetRequestLogAsync(request);
            if (!string.IsNullOrEmpty(str))
                Debug.WriteLine(str);
#endif
        }

        private void PrintResponseToConsole(HttpResponseMessage response, string content)
        {
#if DEBUG
            var str = GetResponseLog(response, content);
            if (!string.IsNullOrEmpty(str))
                Debug.WriteLine(str);
#endif
        }

        private async Task<string> GetRequestLogAsync(HttpRequestMessage request)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Request Start >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            sb.AppendLine($"Time: {DateTime.Now}");
            sb.AppendLine("## REQUEST");
            sb.AppendLine();
            sb.AppendLine($"URI: {request.RequestUri}");
            sb.AppendLine($"METHOD: {request.Method}");

#if DEBUG
            sb.AppendLine($"HEADERS: {request.Headers} {_httpClient.DefaultRequestHeaders}");
#endif

            if (request.Content == null) return sb.ToString();

            var requestContentString = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            try
            {
                //check if valid json to avoid exception
                requestContentString = requestContentString.Trim();

                if (string.IsNullOrEmpty(requestContentString))
                    return sb.ToString();

                if (requestContentString.StartsWith("{") && requestContentString.EndsWith("}") || //For object
                    (requestContentString.StartsWith("[") && requestContentString.EndsWith("]"))) //For array
                {
                    var formattedString = JToken.Parse(requestContentString).ToString(Newtonsoft.Json.Formatting.Indented);
                    sb.AppendLine($"CONTENT: {formattedString}");
                }
                else
                {
                    sb.AppendLine($"CONTENT: {requestContentString}");
                }

            }
            catch (Exception)
            {
                sb.AppendLine($"CONTENT: {requestContentString}");
            }

            return sb.ToString();
        }

        private string GetResponseLog(HttpResponseMessage response, string content)
        {
            var sb = new StringBuilder();
            sb.AppendLine("## RESPONSE:");
            sb.AppendLine($"Time: {DateTime.Now}");
            sb.AppendLine($"URL: {response?.RequestMessage?.RequestUri}");
            sb.AppendLine();
            sb.AppendLine($"StatusCode: {response?.StatusCode}");

#if DEBUG
            sb.AppendLine($"HEADERS: {response?.Headers}");
#endif

            if (string.IsNullOrEmpty(content)) return sb.ToString();

            try
            {
                var formattedString = JToken.Parse(content).ToString(Newtonsoft.Json.Formatting.Indented);
                sb.AppendLine($"CONTENT: {formattedString}");
            }
            catch (Exception)
            {
                sb.AppendLine($"CONTENT: {content}");
            }

            sb.AppendLine("Request End <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

            return sb.ToString();
        }

        #endregion
    }
}

