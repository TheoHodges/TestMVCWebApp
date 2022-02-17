using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestAPI;

namespace IntegrationTests
{
    public class AuthenticationTests
    {
        private WebApplicationFactory<Startup> WebAppFactoryObj;

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
                    });
        }

        [Test]
        public async Task Returns_person_when_authorized()
        {
            using (var httpClient = WebAppFactoryObj.CreateClient())
            {
                var response = await httpClient.GetAsync("api/Person/new");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }
    }
}
