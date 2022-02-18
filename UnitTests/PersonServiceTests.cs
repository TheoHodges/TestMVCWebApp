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

namespace UnitTests
{
    internal class PersonServiceTests
    {
        private string apiEndpoint = "https://localhost:44372/api/Person";
        private string validPersonJson = "{\"id\":1,\"firstName\":\"Theo\",\"lastName\":\"Hodges\",\"dateOfBirth\":\"1970-01-01T00:00:00\",\"placeOfBirth\":\"London\",\"gender\":\"Male\",\"married\":false}";
        private Person validPerson = new Person { FirstName = "Theo", LastName = "Hodges", DateOfBirth = new System.DateTime(1970, 1, 1), PlaceOfBirth = "London", Gender = "Male", Married = false };

        private PersonService SystemUnderTest;
        private IHttpClientHandler httpClientHandler;
        private HttpResponseMessage res;

        [SetUp]
        public void SetUp()
        {
            httpClientHandler = Substitute.For<IHttpClientHandler>();
            res = new HttpResponseMessage();
            SystemUnderTest = new PersonService(httpClientHandler);
        }

        [Test]
        public void GetPeopleCallsApiGet()
        {
            res.StatusCode = (System.Net.HttpStatusCode)200;
            res.Content = new StringContent(validPersonJson);
            httpClientHandler.GetAsync(apiEndpoint).Returns(res);

            var result = SystemUnderTest.GetPeople();

            Assert.AreEqual(res, result.Result);
            httpClientHandler.Received().GetAsync(apiEndpoint);
        }

        [Test]
        public void PostPersonCallsApiPost()
        {
            res.StatusCode = (System.Net.HttpStatusCode)201;
            res.Content = new StringContent(validPersonJson);
            httpClientHandler.PostAsync(apiEndpoint, Arg.Any<StringContent>()).Returns(res);

            var result = SystemUnderTest.PostPerson(validPerson);

            httpClientHandler.Received().PostAsync(apiEndpoint, Arg.Any<StringContent>());
            Assert.AreEqual(res, result.Result);
        }
    }
}
