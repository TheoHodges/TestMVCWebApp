using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Services
{
    public class PersonService : IPersonService
    {
        // Web API base url
        string baseURL = "https://localhost:44372/";
        string personEndpoint = "api/Person";
        private IHttpClientHandler _httpClient;
        public PersonService(IHttpClientHandler httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetPeople()
        {
            return await _httpClient.GetAsync(baseURL + personEndpoint);
        }

        public async Task<HttpResponseMessage> PostPerson(Person person)
        {
            var content = new StringContent(JsonConvert.SerializeObject(person));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            return await _httpClient.PostAsync(baseURL + personEndpoint, content);
        }
    }
}
