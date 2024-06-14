using System.Net;
using System.Net.Http.Headers;

namespace Demo.Maui.Models.Responses;

public class RestResponse
{
    public HttpStatusCode StatusCode { get; set; }

    public string Message { get; set; }

    public HttpResponseHeaders ResponseHeaders { get; set; }

    public HttpContent Content { get; set; }
}

public class RestResponse<T> : RestResponse
{
    public T Data { get; set; }
}

