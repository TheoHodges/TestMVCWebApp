using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestAPI.Controllers;
using TestAPI.Data;
using TestAPI.Models;
using TestAPI.Repositories;

namespace IntegrationTests
{
    internal class PersonControllerTests
    {
        [Test]
        public void ValidateAgeNegativeReturnsBadRequest()
        {
            IPersonRepository mockPersonRepository = Substitute.For<IPersonRepository>();
            PersonController controller = new PersonController(mockPersonRepository);

            var result = controller.ValidateAge(-2);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            BadRequestObjectResult result2 = (BadRequestObjectResult)result;
            Assert.AreEqual("age cannot be negative", result2.Value);
        }

        [Test]
        public void ValidateAgeZeroReturnsOk()
        {
            IPersonRepository mockPersonRepository = Substitute.For<IPersonRepository>();
            PersonController controller = new PersonController(mockPersonRepository);

            var result = controller.ValidateAge(0);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void ValidateAgePositiveReturnsOk()
        {
            IPersonRepository mockPersonRepository = Substitute.For<IPersonRepository>();
            PersonController controller = new PersonController(mockPersonRepository);

            var result = controller.ValidateAge(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void NewPersonTest()
        {
            IPersonRepository mockPersonRepository = Substitute.For<IPersonRepository>();
            PersonController controller = new PersonController(mockPersonRepository);

            var result = controller.NewPerson();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetAllPeopleCallsPersonRepository()
        {
            IPersonRepository mockPersonRepository = Substitute.For<IPersonRepository>();
            var expected = Enumerable.Empty<Person>();
            mockPersonRepository.GetAllPeople().Returns(expected);
            PersonController controller = new PersonController(mockPersonRepository);

            var result = controller.GetAllPeople();

            mockPersonRepository.Received().GetAllPeople();
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            OkObjectResult result2 = (OkObjectResult)result.Result;
            Assert.AreEqual(expected, result2.Value);
        }

        [Test]
        public void GetPersonInvalidIdReturnsNotFound()
        {
            IPersonRepository mockPersonRepository = Substitute.For<IPersonRepository>();
            Person expected = null;
            mockPersonRepository.GetPersonByID(-2).Returns(expected);
            PersonController controller = new PersonController(mockPersonRepository);

            var result = controller.GetPerson(-2);

            mockPersonRepository.Received().GetPersonByID(-2);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void GetPersonValidIdReturnsPerson()
        {
            IPersonRepository mockPersonRepository = Substitute.For<IPersonRepository>();
            Person expected = new Person();
            mockPersonRepository.GetPersonByID(1).Returns(expected);
            PersonController controller = new PersonController(mockPersonRepository);

            var result = controller.GetPerson(1);

            mockPersonRepository.Received().GetPersonByID(1);
            Assert.AreEqual(expected, result.Value);
        }

        [Test]
        public void CreateWithValidModelReturnsCreated()
        {
            IPersonRepository mockPersonRepository = Substitute.For<IPersonRepository>();
            Person person = new Person();
            PersonController controller = new PersonController(mockPersonRepository);

            var result = controller.Create(person);

            mockPersonRepository.Received().InsertPerson(person);
            mockPersonRepository.Received().Save();
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }

        [Test]
        public void CreateWithInvalidModelReturnsBadRequest()
        {
            IPersonRepository mockPersonRepository = Substitute.For<IPersonRepository>();
            Person person = new Person();
            PersonController controller = new PersonController(mockPersonRepository);
            controller.ModelState.AddModelError("fakeError", "fakeError");

            var result = controller.Create(person);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}
