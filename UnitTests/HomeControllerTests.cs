using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestApp.Controllers;
using TestApp.Models;
using TestApp.Services;

namespace UnitTests
{
    internal class HomeControllerTests
    {
        private string validPersonJson = "{\"id\":1,\"firstName\":\"Theo\",\"lastName\":\"Hodges\",\"dateOfBirth\":\"1970-01-01T00:00:00\",\"placeOfBirth\":\"London\",\"gender\":\"Male\",\"married\":false}";
        private Person validPerson = new Person { FirstName = "Theo", LastName = "Hodges", DateOfBirth = new System.DateTime(1970, 1, 1), PlaceOfBirth = "London", Gender = "Male", Married = false };
        private Person invalidPerson = new Person { FirstName = "Theo", DateOfBirth = new System.DateTime(1970, 1, 1), PlaceOfBirth = "Lond99on", Gender = "Male", Married = false };

        private HomeController SystemUnderTest;
        private IPersonService service;
        private HttpResponseMessage res;

        [SetUp]
        public void SetUp()
        {
            service = Substitute.For<IPersonService>();
            res = new HttpResponseMessage();
            SystemUnderTest = new HomeController(service);
        }

        [Test]
        public void IndexActionCallsServiceGet()
        {
            res.StatusCode = (System.Net.HttpStatusCode)200;
            res.Content = new StringContent(validPersonJson);
            service.GetPeople().Returns(res);

            _ = SystemUnderTest.Index();

            service.Received().GetPeople();
        }
        
        [Test]
        public void CreateActionCallsServicePostWhenModelIsValid()
        {
            res.StatusCode = (System.Net.HttpStatusCode)201;
            service.PostPerson(validPerson).Returns(res);

            _ = SystemUnderTest.Create(validPerson);
            
            service.Received().PostPerson(validPerson);
        }

        [Test]
        public void CreateActionDoesNotCallServiceWhenModelIsInvalid()
        {
            res.StatusCode = (System.Net.HttpStatusCode)201;
            service.PostPerson(invalidPerson).Returns(res);
            SystemUnderTest.ModelState.AddModelError("fakeError", "fakeError");

            _ = SystemUnderTest.Create(invalidPerson);

            service.DidNotReceive().PostPerson(invalidPerson);
        }
    }
}
