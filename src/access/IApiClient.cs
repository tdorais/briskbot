using System;
using System.Net.Http;
using System.Threading.Tasks;

public interface IApiClient
{
    // HttpResponseMessage Get(string url);

    // HttpResponseMessage Post(string url, HttpContent content);

    Task<HttpResponseMessage> Get(string url);

    Task<HttpResponseMessage> Post(string url, HttpContent content);
}