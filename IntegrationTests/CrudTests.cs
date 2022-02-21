using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestAPI;
using TestAPI.Models;

namespace IntegrationTests
{
    internal class CrudTests
    {
        private WebApplicationFactory<Startup> WebAppFactoryObj;
        private string validPersonJson = "{\"id\":1,\"firstName\":\"Theo\",\"lastName\":\"Hodges\",\"dateOfBirth\":\"1970-01-01T00:00:00\",\"placeOfBirth\":\"London\",\"gender\":\"Male\",\"married\":false}";
        private Person validPerson = new Person { FirstName = "Theo", LastName = "Hodges", DateOfBirth = new System.DateTime(1970, 1, 1), PlaceOfBirth = "London", Gender = "Male", Married = false };
        private Person invalidPerson = new Person { FirstName = "Theo", DateOfBirth = new System.DateTime(1970, 1, 1), PlaceOfBirth = "London", Gender = "Male", Married = false };

        [SetUp]
        public void SetupFixture()
        {
            WebAppFactoryObj = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder.ConfigureTestServices(services => 
                        {
                            MockAuthentication.Enable(services);
                        });
                    }
                );

        }

        [Test]
        public async Task Returns_all_people_for_get()
        {
            using (var httpClient = WebAppFactoryObj.CreateClient())
            {
                var response = await httpClient.GetAsync("api/Person");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                var responseText = await response.Content.ReadAsStringAsync();
                Assert.That(responseText, Contains.Substring(validPersonJson));
            }
        }

        [Test]
        public async Task Returns_me_for_get_with_id_1()
        {
            using (var httpClient = WebAppFactoryObj.CreateClient())
            {
                var response = await httpClient.GetAsync("api/Person/1");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                var responseText = await response.Content.ReadAsStringAsync();
                Assert.That(responseText, Is.EqualTo(validPersonJson));
            }
        }

        [Test]
        public async Task Returns_bad_request_for_get_with_id_0()
        {
            using (var httpClient = WebAppFactoryObj.CreateClient())
            {
                var response = await httpClient.GetAsync("api/Person/0");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }

        [Test]
        public async Task Returns_created_for_valid_post()
        {
            using (var httpClient = WebAppFactoryObj.CreateClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(validPerson));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var response = await httpClient.PostAsync("api/Person", content);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            }
        }

        [Test]
        public async Task Returns_bad_request_for_invalid_post()
        {
            using (var httpClient = WebAppFactoryObj.CreateClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(invalidPerson));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var response = await httpClient.PostAsync("api/Person", content);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }
    }
}
