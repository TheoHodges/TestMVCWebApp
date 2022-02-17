using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TestApp.Models;
using TestApp.Services;

namespace IntegrationTests
{
    internal class PersonServiceTests
    {
        private string apiEndpoint = "https://localhost:44372/api/Person";
        private string validPersonJson = "{\"id\":1,\"firstName\":\"Theo\",\"lastName\":\"Hodges\",\"dateOfBirth\":\"2001-02-21T00:00:00\",\"placeOfBirth\":\"Sutton\",\"gender\":\"Male\",\"married\":false}";
        private Person validPerson = new Person { FirstName = "Theo", LastName = "Hodges", DateOfBirth = new System.DateTime(2001, 2, 21), PlaceOfBirth = "Sutton", Gender = "Male", Married = false };


        [Test]
        public void GetPeopleCallsApiGet()
        {
            IHttpClientHandler httpClientHandler = Substitute.For<IHttpClientHandler>();
            HttpResponseMessage res = new HttpResponseMessage();
            res.StatusCode = (System.Net.HttpStatusCode)200;
            res.Content = new StringContent(validPersonJson);
            httpClientHandler.GetAsync(apiEndpoint).Returns(res);
            PersonService service = new PersonService(httpClientHandler);

            var result = service.GetPeople();

            Assert.AreEqual(res, result.Result);
            httpClientHandler.Received().GetAsync(apiEndpoint);
        }

        [Test]
        public void PostPersonCallsApiPost()
        {
            IHttpClientHandler httpClientHandler = Substitute.For<IHttpClientHandler>();
            HttpResponseMessage res = new HttpResponseMessage();
            res.StatusCode = (System.Net.HttpStatusCode)201;
            res.Content = new StringContent(validPersonJson);
            httpClientHandler.PostAsync(apiEndpoint, Arg.Any<StringContent>()).Returns(res);
            PersonService service = new PersonService(httpClientHandler);

            var result = service.PostPerson(validPerson);

            httpClientHandler.Received().PostAsync(apiEndpoint, Arg.Any<StringContent>());
            Assert.AreEqual(res, result.Result);
        }
    }
}
