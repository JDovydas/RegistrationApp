using RegistrationApp.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.BusinessLogic.Helpers
{
    public class Helpers
    {
        //public async Task EnsureUserOwnsPerson(Guid userId, Guid personId) /// should it be added to Helpers? Is it OK that Task does not contain a  model in itself?
        //{
        //    var person = await _personRepository.GetPersonById(personId);
        //    if (person == null)
        //    {
        //        throw new InvalidOperationException("Person not found.");
        //    }

        //    if (person.UserId != userId)
        //    {
        //        throw new UnauthorizedAccessException("You are not authorized to update this person's information.");
        //    }
        //}
    }
}
