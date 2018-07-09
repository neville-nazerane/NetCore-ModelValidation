using NetCore.ModelValidation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebsite.Models;

namespace TestWebsite.Services
{
    public class PersonRepository
    {
        private readonly ModelValidator modelValidator;

        public PersonRepository(ModelValidator modelValidator)
        {
            this.modelValidator = modelValidator;
        }

        public void Validate(Person person)
        {
            var helper = modelValidator.GetHelper<Person>();
            if (person.FirstName == "nameless")
                helper.AddError(p => p.FirstName, "You need a first name.");
            if (person.LastName == "nameless")
                helper.AddError(p => p.LastName, "Man needs a last name.");
            if (person.Age > 35)
                helper.AddError(p => p.Age, "You are getting too old for this s...erver");
        }

    }
}
