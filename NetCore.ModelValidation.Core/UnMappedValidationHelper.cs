using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NetCore.ModelValidation.Core
{

    public class UnMappedValidationHelper<TModel>
    {
        private readonly ModelValidator context;

        internal UnMappedValidationHelper(ModelValidator context)
        {
            this.context = context;
        }

        public IEnumerable<string> GetErrors<T>(Expression<Func<TModel, T>> lamda)
        {
            if (lamda.Body is MemberExpression mem) return context.GetErrors(mem.Member.Name);
            else throw new InvalidOperationException("Invalid lamda provided. Property is expected.");
        }

        public IEnumerable<string> AddError<T>(Expression<Func<TModel, T>> lamda, string error)
        {
            if (lamda.Body is MemberExpression mem) return context.AddError(mem.Member.Name, error);
            else throw new InvalidOperationException("Invalid lamda provided. Property is expected.");
        }

    }

}
