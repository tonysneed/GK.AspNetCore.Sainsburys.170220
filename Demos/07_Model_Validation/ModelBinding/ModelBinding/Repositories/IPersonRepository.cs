using System.Collections;
using System.Collections.Generic;
using ModelBinding.Models;

namespace ModelBinding.Repositories
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetPersons();
        Person GetPerson(int id);
        void UpdatePerson(int id, Person person);
    }
}