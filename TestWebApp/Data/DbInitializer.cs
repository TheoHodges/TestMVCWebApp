using System.Linq;
using TestAPI.Models;

namespace TestAPI.Data
{
    public class DbInitializer
    {
        public static void Initialize(PersonContext context)
        {
            context.Database.EnsureCreated();

            // Look for any records
            if (context.Person.Any())
            {
                return; // DB has already been seeded
            }

            var people = new Person[]
            {
                new Person{FirstName="Theo",LastName="Hodges",DateOfBirth=new System.DateTime(1970, 1, 1),PlaceOfBirth="London",Gender="Male",Married=false},
                new Person{FirstName="Jane",LastName="Smith",DateOfBirth=new System.DateTime(2000, 1, 1),PlaceOfBirth="New York",Gender="Female",Married=false}
            };
            foreach (Person person in people)
            {
                context.Person.Add(person);
            }
            context.SaveChanges();
        }
    }
}
