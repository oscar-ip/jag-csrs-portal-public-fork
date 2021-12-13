using Csrs.Api.Models;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Csrs.Api.Repositories
{
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Adds a string property if it is not null.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <param name="model"></param>
        /// <param name="propertyAccessor"></param>
        public static void Add<TModel>(this Dictionary<string, object?> data, string field, TModel model, Expression<Func<TModel, string>> propertyAccessor)
        {
            string value = propertyAccessor.Compile().Invoke(model);
            if (value is not null)
            {
                data.Add(field, value.Trim());
            }
        }

        /// <summary>
        /// Adds a string property if it has changed from the previous value.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <param name="model"></param>
        /// <param name="modelOropertyAccessor"></param>
        /// <param name="entity"></param>
        /// <param name="entityPropertyAccessor"></param>
        public static void Add<TModel, TEntity>(
            this Dictionary<string, object?> data,
            string field,
            TModel model,
            Expression<Func<TModel, string>> modelOropertyAccessor,
            TEntity entity,
            Expression<Func<TEntity, string>> entityPropertyAccessor)
        {
            string newValue = modelOropertyAccessor.Compile().Invoke(model);
            string oldValue = entityPropertyAccessor.Compile().Invoke(entity);

            // old value      new value   action
            // ---------      ---------   ------
            // null           null        nothing
            // null            B          set B
            //  A             null        set null
            //  A              B          set B

            if (newValue is null && oldValue is null)
            {
                return;
            }

            if (oldValue is null && newValue is not null)
            {
                data.Add(field, newValue.Trim());
                return;
            }

            if (oldValue is not null && newValue is null)
            {
                data.Add(field, null);
                return;
            }

            Debug.Assert(oldValue is not null);
            Debug.Assert(newValue is not null);

            // oldValue is not null and newValue is not null
            // if the value has changed, update it
            oldValue = oldValue.Trim();
            newValue = newValue.Trim();

            if (oldValue != newValue)
            {
                data.Add(field, newValue);
            }
        }

        /// <summary>
        /// Adds a boolean property if it has changed from the previous value.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <param name="model"></param>
        /// <param name="modelOropertyAccessor"></param>
        /// <param name="entity"></param>
        /// <param name="entityPropertyAccessor"></param>
        public static void Add<TModel, TEntity>(
            this Dictionary<string, object?> data,
            string field,
            TModel model,
            Expression<Func<TModel, bool>> modelOropertyAccessor,
            TEntity entity,
            Expression<Func<TEntity, bool>> entityPropertyAccessor)
        {
            bool newValue = modelOropertyAccessor.Compile().Invoke(model);
            bool oldValue = entityPropertyAccessor.Compile().Invoke(entity);

            if (oldValue != newValue)
            {
                data.Add(field, newValue);
            }
        }


        /// <summary>
        /// Adds the lookup value if it has a value.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <param name="model"></param>
        /// <param name="propertyAccessor"></param>
        public static void Add<TModel>(this Dictionary<string, object?> data, string field, TModel model, Expression<Func<TModel, LookupValue>> propertyAccessor)
        {
            LookupValue value = propertyAccessor.Compile().Invoke(model);
            if (value is not null)
            {
                data.Add(field, value.Id);
            }
        }
    }
}
