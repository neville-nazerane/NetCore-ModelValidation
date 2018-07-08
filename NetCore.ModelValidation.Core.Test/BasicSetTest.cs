using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NetCore.ModelValidation.Core.Test
{

    public class BasicSetTest
    {

        [Fact]
        public void NoModel()
        {
            var validator = new ModelValidator();
            validator.AddError("name", "what the hell?");
            validator.AddError("place", "when?");
            validator.AddError("name", "How?");

            Assert.Equal(2, validator["name"].Count());
            Assert.Single(validator["place"]);

        }

        [Fact]
        public void Modeled()
        {
            var model = new List<int>();
            var validator = new ModelValidator();
            validator.AddError("name", "what the hell?");
            validator.AddError("place", "when?");
            validator.AddError(model, "name", "How?");

            Assert.Single(validator[model]);
            Assert.Single(validator[model, "name"]);
            Assert.Single(validator["name"]);

        }

    }
}
