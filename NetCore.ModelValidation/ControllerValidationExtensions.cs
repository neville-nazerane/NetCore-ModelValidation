using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using NetCore.ModelValidation.Core;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerValidationExtensions
    {


        public static bool UpdateValidations(this ControllerBase controller, object model = null, bool mapModel = false)
        {
            controller.ModelState.Clear();
            if (model != null) controller.TryValidateModel(model);
            var validator = controller.HttpContext.RequestServices.GetService<ModelValidator>();
            if (!mapModel) model = null;
            var errors = validator.GetErrors(model)?
                        .SelectMany(e => e.Value.Select(v => new KeyValuePair<string, string>(e.Key, v)));
            foreach (var error in errors)
                controller.ModelState.AddModelError(error.Key, error.Value);
            return controller.ModelState.IsValid && errors?.Count() < 1;
        }

        public static IActionResult ValidateAndView(this Controller controller, object model = null, bool mapModel = false)
        {
            controller.UpdateValidations(model, mapModel);
            return controller.View(model);
        }

        public static ActionResult ValidateAndBadRequest(this ControllerBase controller, object model = null, bool mapModel = false)
        {
            controller.UpdateValidations(model, mapModel);
            return controller.BadRequest(controller.ModelState);
        }

    }
}
