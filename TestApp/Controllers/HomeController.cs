using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Controllers
{
    public class HomeController : Controller
    {
        private IPersonService _service;

        public HomeController(IPersonService personService)
        {
            _service = personService;
        }

        public async Task<ActionResult> Index()
        {
            List<Person> people = new List<Person>();
            var res = await _service.GetPeople();
            if (res.IsSuccessStatusCode)
            {
                var response = res.Content.ReadAsStringAsync().Result;
                people = JsonConvert.DeserializeObject<List<Person>>(response);
            }
            return View(people);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("FirstName,LastName,DateOfBirth,PlaceOfBirth,Gender,Married")] Person person)
        {
            if (ModelState.IsValid)
            {
                var res = await _service.PostPerson(person);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(person);
        }
    }
}
