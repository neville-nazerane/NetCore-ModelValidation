using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NetCore.ModelValidation.Core
{

    public class ModelValidator
    {
        readonly Dictionary<object, Dictionary<string, IEnumerable<string>>> modelErrors;
        readonly Dictionary<string, IEnumerable<string>> errors;

        public ModelValidator()
        {
            modelErrors = new Dictionary<object, Dictionary<string, IEnumerable<string>>>();
            errors = new Dictionary<string, IEnumerable<string>>();
        }

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> GetErrors(object model = null)
            => _GetErrors(model);

        Dictionary<string, IEnumerable<string>> _GetErrors(object model = null, bool createIfNotExist = false)
        {
            if (model == null) return errors;
            else if (modelErrors.ContainsKey(model)) return modelErrors[model];
            else
            {
                if (createIfNotExist)
                {
                    var output = new Dictionary<string, IEnumerable<string>>();
                    modelErrors.Add(model, output);
                    return output;
                }
                else return null;
            }
        }

        public TypedValidationHelper<TModel> GetHelper<TModel>(TModel model)
            => new TypedValidationHelper<TModel>(this, model);

        public IEnumerable<string> GetErrors(string key) => GetErrorsFrom(errors, key);

        public IEnumerable<string> GetErrors(object model, string key)
            => GetErrorsFrom(_GetErrors(model), key);

        public IEnumerable<string> GetErrors<TModel, T>(TModel model, Expression<Func<TModel, T>> lamda)
        {
            if (lamda.Body is MemberExpression mem) return GetErrors(model, mem.Member.Name);
            else throw new InvalidOperationException("Invalid lamda provided. Property is expected.");
        }

        IEnumerable<string> GetErrorsFrom(Dictionary<string, IEnumerable<string>> errors, string key)
        {
            if (errors.ContainsKey(key)) return errors[key];
            else return null;
        }
        
        public IEnumerable<string> AddError(string error) => AddError(errors, "", error);
        public IEnumerable<string> AddError(string key, string error) => AddError(errors, key, error);
        public IEnumerable<string> AddError(object model, string key, string error) 
                                            => AddError(_GetErrors(model, true), key, error);

        public IEnumerable<string> AddError<TModel, T>(TModel model, Expression<Func<TModel, T>> lamda, string error)
        {
            if (lamda.Body is MemberExpression mem) return AddError(model, mem.Member.Name, error);
            else throw new InvalidOperationException("Invalid lamda provided. Property is expected.");
        }

        IEnumerable<string> AddError(Dictionary<string, IEnumerable<string>> errors, string key, string error)
        {
            List<string> err;
            if (!errors.ContainsKey(key))
            {
                err = new List<string>();
                errors.Add(key, err);
            }
            else err = (List<string>)errors[key];
            err.Add(error);
            return err;
        }

        public IEnumerable<string> this[string key] => GetErrors(key);

        public IEnumerable<string> this[object model, string key] => GetErrors(model, key);

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> this[object model] => GetErrors(model);   

        public class TypedValidationHelper<TModel>
        {

            private readonly Dictionary<string, PropertyInfo> properties;
            private readonly ModelValidator context;
            private readonly TModel model;

            internal TypedValidationHelper(ModelValidator context, TModel model)
            {
                properties = typeof(TModel).GetProperties().ToDictionary(p => p.Name);
                this.context = context;
                this.model = model;
            }

            public IEnumerable<string> GetErrors<T>(Expression<Func<TModel, T>> lamda)
                => context.GetErrors(model, lamda);

            public IEnumerable<string> AddError<T>(Expression<Func<TModel, T>> lamda, string error)
                => context.AddError(model, lamda, error);
        }

    }
}
