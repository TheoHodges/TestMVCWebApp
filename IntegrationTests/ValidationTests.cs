using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestAPI;
using TestAPI.Models;

namespace IntegrationTests
{
    public class ValidationTests
    {
        private WebApplicationFactory<Startup> WebAppFactoryObj;

        [SetUp]
        public void SetupFixture()
        {
            WebAppFactoryObj = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder.ConfigureTestServices(services => { });
                    }
                );
        }

        [Test]
        public async Task Returns_bad_request_if_negative()
        {
            using (var httpClient = WebAppFactoryObj.CreateClient())
            {
                var response = await httpClient.GetAsync("api/Person/ValidateAge?age=-10");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        [Test]
        public async Task Returns_ok_if_positive()
        {
            using (var httpClient = WebAppFactoryObj.CreateClient())
            {
                var response = await httpClient.GetAsync("api/Person/ValidateAge?age=100");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public async Task Returns_bad_request_if_invalid_model()
        {
            using (var httpClient = WebAppFactoryObj.CreateClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(new Person()));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var response = await httpClient.PostAsync("api/Person", content);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        [Test]
        public async Task Returns_created_if_valid_model()
        {
            using (var httpClient = WebAppFactoryObj.CreateClient())
            {
                var person = new Person { FirstName = "Theo", LastName = "Hodges", DateOfBirth = new System.DateTime(2001, 2, 21), PlaceOfBirth = "Sutton", Gender = "Male", Married = false };
                var content = new StringContent(JsonConvert.SerializeObject(person));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var response = await httpClient.PostAsync("api/Person", content);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            }
        }
    }
}