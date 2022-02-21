using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPI.Data;
using TestAPI.Models;
using TestAPI.Repositories;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private IPersonRepository personRepository;

        public PersonController(IPersonRepository repository)
        {
            personRepository = repository;
        }

        [Route("ValidateAge")]
        [HttpGet]
        public IActionResult ValidateAge(int age)
        {
            if (age < 0)
            {
                return BadRequest("age cannot be negative");
            }
            return Ok(true);
        }

        [Route("new")]
        [HttpGet]
        [Authorize]
        public IActionResult NewPerson()
        {
            return Ok(new Person());
        }

        [HttpGet]
        public ActionResult<List<Person>> GetAllPeople()
        {
            var people = personRepository.GetAllPeople();

            return Ok(people);
        }

        [HttpGet("{id}")]
        public ActionResult<Person> GetPerson(int id)
        {
            var person = personRepository.GetPersonByID(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }   

        [HttpPost]
        //[Authorize]
        public IActionResult Create([Bind("FirstName,LastName,DateOfBirth,PlaceOfBirth,Gender,Married")] Person person)
        {
            if (ModelState.IsValid)
            {
                personRepository.InsertPerson(person);
                personRepository.Save();
                return CreatedAtAction(nameof(GetPerson), new { id = person.ID }, person);
            }
            return BadRequest();
        }
    }
}
