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

namespace UnitTests
{
    internal class PersonControllerTests
    {
        private PersonController SystemUnderTest;
        private IPersonRepository mockPersonRepository;

        [SetUp]
        public void SetUp()
        {
            mockPersonRepository = Substitute.For<IPersonRepository>();
            SystemUnderTest = new PersonController(mockPersonRepository);
        }

        [Test]
        public void ValidateAgeNegativeReturnsBadRequest()
        {
            var result = SystemUnderTest.ValidateAge(-2);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            BadRequestObjectResult result2 = (BadRequestObjectResult)result;
            Assert.AreEqual("age cannot be negative", result2.Value);
        }

        [Test]
        public void ValidateAgeZeroReturnsOk()
        {
            var result = SystemUnderTest.ValidateAge(0);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void ValidateAgePositiveReturnsOk()
        {
            var result = SystemUnderTest.ValidateAge(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void NewPersonTest()
        {
            var result = SystemUnderTest.NewPerson();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetAllPeopleCallsPersonRepository()
        {
            var expected = Enumerable.Empty<Person>();
            mockPersonRepository.GetAllPeople().Returns(expected);

            var result = SystemUnderTest.GetAllPeople();

            mockPersonRepository.Received().GetAllPeople();
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            OkObjectResult result2 = (OkObjectResult)result.Result;
            Assert.AreEqual(expected, result2.Value);
        }

        [Test]
        public void GetPersonInvalidIdReturnsNotFound()
        {
            Person expected = null;
            mockPersonRepository.GetPersonByID(-2).Returns(expected);

            var result = SystemUnderTest.GetPerson(-2);

            mockPersonRepository.Received().GetPersonByID(-2);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void GetPersonValidIdReturnsPerson()
        {
            Person expected = new Person();
            mockPersonRepository.GetPersonByID(1).Returns(expected);

            var result = SystemUnderTest.GetPerson(1);

            mockPersonRepository.Received().GetPersonByID(1);
            Assert.AreEqual(expected, result.Value);
        }

        [Test]
        public void CreateWithValidModelReturnsCreated()
        {
            Person person = new Person();

            var result = SystemUnderTest.Create(person);

            mockPersonRepository.Received().InsertPerson(person);
            mockPersonRepository.Received().Save();
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }

        [Test]
        public void CreateWithInvalidModelReturnsBadRequest()
        {
            Person person = new Person();
            SystemUnderTest.ModelState.AddModelError("fakeError", "fakeError");

            var result = SystemUnderTest.Create(person);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}
