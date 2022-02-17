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

namespace IntegrationTests
{
    internal class HomeControllerTests
    {
        private string validPersonJson = "{\"id\":1,\"firstName\":\"Theo\",\"lastName\":\"Hodges\",\"dateOfBirth\":\"2001-02-21T00:00:00\",\"placeOfBirth\":\"Sutton\",\"gender\":\"Male\",\"married\":false}";
        private Person validPerson = new Person { FirstName = "Theo", LastName = "Hodges", DateOfBirth = new System.DateTime(2001, 2, 21), PlaceOfBirth = "Sutton", Gender = "Male", Married = false };
        private Person invalidPerson = new Person { FirstName = "Theo", DateOfBirth = new System.DateTime(2001, 2, 21), PlaceOfBirth = "Sutt99on", Gender = "Male", Married = false };

        [Test]
        public void IndexActionCallsServiceGet()
        {
            IPersonService service = Substitute.For<IPersonService>();
            HttpResponseMessage res = new HttpResponseMessage();
            res.StatusCode = (System.Net.HttpStatusCode)200;
            res.Content = new StringContent(validPersonJson);
            service.GetPeople().Returns(res);
            HomeController controller = new HomeController(service);

            _ = controller.Index();

            service.Received().GetPeople();
        }
        
        [Test]
        public void CreateActionCallsServicePostWhenModelIsValid()
        {
            IPersonService service = Substitute.For<IPersonService>();
            HttpResponseMessage res = new HttpResponseMessage();
            res.StatusCode = (System.Net.HttpStatusCode)201;
            service.PostPerson(validPerson).Returns(res);
            HomeController controller = new HomeController(service);

            _ = controller.Create(validPerson);
            
            service.Received().PostPerson(validPerson);
        }

        [Test]
        public void CreateActionDoesNotCallServiceWhenModelIsInvalid()
        {
            IPersonService service = Substitute.For<IPersonService>();
            HttpResponseMessage res = new HttpResponseMessage();
            res.StatusCode = (System.Net.HttpStatusCode)201;
            service.PostPerson(invalidPerson).Returns(res);
            HomeController controller = new HomeController(service);
            controller.ModelState.AddModelError("fakeError", "fakeError");

            _ = controller.Create(invalidPerson);

            service.DidNotReceive().PostPerson(invalidPerson);
        }
    }
}
