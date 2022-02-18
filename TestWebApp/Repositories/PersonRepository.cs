using System.Collections.Generic;
using System.Linq;
using TestAPI.Data;
using TestAPI.Models;

namespace TestAPI.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private PersonContext context;

        public PersonRepository(PersonContext context)
        {
            this.context = context;
        }

        public IEnumerable<Person> GetAllPeople()
        {
            return context.Person.ToList();
        }

        public Person GetPersonByID(int id)
        {
            return context.Person.Find(id);
        }

        public void InsertPerson(Person person)
        {
            context.Person.Add(person);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
