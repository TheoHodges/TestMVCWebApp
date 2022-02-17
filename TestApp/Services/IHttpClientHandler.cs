using System.Net.Http;
using System.Threading.Tasks;

namespace TestApp.Services
{
    public interface IHttpClientHandler
    {
        HttpResponseMessage Get(string url);
        Task<HttpResponseMessage> GetAsync(string url);
        HttpResponseMessage Post(string url, HttpContent content);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    }
}