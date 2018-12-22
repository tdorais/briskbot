using System;
using System.Net.Http;
using System.Threading.Tasks;

public interface IApiClient
{
    Task<HttpResponseMessage> Get(string url);

    Task<HttpResponseMessage> Post(string url, HttpContent content);
}