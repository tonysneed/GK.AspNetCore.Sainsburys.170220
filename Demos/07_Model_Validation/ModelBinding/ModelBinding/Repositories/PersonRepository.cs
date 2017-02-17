using System.Collections.Generic;
using ModelBinding.Models;

namespace ModelBinding.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IDictionary<int, Person> _persons = new Dictionary<int, Person>
        {
            { 1, new Person { Id = 1, Name = "Peter", Age = 20 } },
            { 2, new Person { Id = 2, Name = "Paul", Age = 21 } },
            { 3, new Person { Id = 3, Name = "Mary", Age = 22 } },
        };

        public IEnumerable<Person> GetPersons()
        {
            return _persons.Values;
        }

        public Person GetPerson(int id)
        {
            return _persons[id];
        }

        public void UpdatePerson(int id, Person person)
        {
            _persons[id] = person;
        }
    }
}
