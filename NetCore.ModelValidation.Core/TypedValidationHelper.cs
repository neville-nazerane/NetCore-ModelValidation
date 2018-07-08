using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NetCore.ModelValidation.Core
{
    public class TypedValidationHelper<TModel>
    {

        private readonly ModelValidator context;
        private readonly TModel model;

        internal TypedValidationHelper(ModelValidator context, TModel model)
        {
            this.context = context;
            this.model = model;
        }

        public IEnumerable<string> GetErrors<T>(Expression<Func<TModel, T>> lamda)
            => context.GetErrors(model, lamda);

        public IEnumerable<string> AddError<T>(Expression<Func<TModel, T>> lamda, string error)
            => context.AddError(model, lamda, error);
    }
}
