using System;
using System.Collections.Generic;
using TestAPI.Models;

namespace TestAPI.Repositories
{
    public interface IPersonRepository 
    {
        IEnumerable<Person> GetAllPeople();
        Person GetPersonByID(int id);
        void InsertPerson(Person person);
        void Save();
    }
}