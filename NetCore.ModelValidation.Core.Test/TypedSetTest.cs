using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NetCore.ModelValidation.Core.Test
{
    public class TypedSetTest
    {

        [Fact]
        public void WithModel()
        {
            var model = new Person();
            var validator = new ModelValidator();
            var helper = validator.GetHelper(model);

            helper.AddError(p => p.Name, "Name is too long");
            helper.AddError(p => p.Age, "You are getting too old for this");
            validator.AddError(nameof(Person.Age), "this one is personal");

            Assert.Single(validator.GetErrors());
            Assert.Equal(2, validator.GetErrors(model).Count());
            Assert.Single(validator.GetErrors(model, nameof(Person.Age)));
            Assert.Single(helper.GetErrors(p => p.Age));
            Assert.Single(helper.GetErrors(p => p.Name));
            Assert.Null(helper.GetErrors(p => p.Hobby));
            Assert.Single(validator.GetErrors(model, nameof(Person.Name)));

        }

        [Fact]
        public void NoModel()
        {
            var validator = new ModelValidator();
            var helper = validator.GetHelper<Person>();

            helper.AddError(p => p.Name, "Name is too long");
            helper.AddError(p => p.Age, "You are getting too old for this");
            validator.AddError(nameof(Person.Age), "this one is personal");

            Assert.Equal(3, validator.GetErrors().SelectMany(e => e.Value).Count());
            Assert.Equal(2, validator.GetErrors().Count());
            Assert.Equal(2, validator.GetErrors(nameof(Person.Age)).Count());
            Assert.Single(validator.GetErrors(nameof(Person.Name)));
            Assert.Single(helper.GetErrors(p => p.Name));

        }

    }
}
