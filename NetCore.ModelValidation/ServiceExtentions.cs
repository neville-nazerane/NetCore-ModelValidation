using NetCore.ModelValidation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtentions
    {

        public static IServiceCollection AddNetCoreValidations(this IServiceCollection services) 
            => services.AddScoped<ModelValidator>();

    }
}
