using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal static class ReflectionHelper
    {
        static readonly Dictionary<PropertyInfo, Action<object,object>> setterCache = new Dictionary<PropertyInfo, Action<object, object>>();
        static readonly Dictionary<PropertyInfo, Func<object, object>> getterCache = new Dictionary<PropertyInfo, Func<object, object>>();
        public static PropertyInfo GetProperty<T, TMember>(Expression<Func<T, TMember>> expression)
        {
            if (expression.Body is MemberExpression member)
            {
                if (member.Expression.Type != typeof(T))
                    throw new InvalidOperationException("Property " + member.Member.Name + " is not a member of type " + typeof(T).Name);

                return member.Member as PropertyInfo;
            }

            throw new ArgumentException("Expression is not a member accessor", "expression");
        }

        public static void SetValue(PropertyInfo prop, object target, object value)
        {
            if (!setterCache.TryGetValue(prop, out var setter))
            {
                var p1 = Expression.Parameter(typeof(object));
                var p2 = Expression.Parameter(typeof(object));

                var exp = Expression.Assign(
                                Expression.Property(
                                    Expression.Convert(p1, prop.DeclaringType), prop),
                                Expression.Convert(p2, prop.PropertyType));

                var l = Expression.Lambda<Action<object,object>>(exp, p1, p2);

                setter = l.Compile();
                setterCache.Add(prop, setter);
            }
            
            setter(target, value);
        }

        public static object GetValue(PropertyInfo prop, object target)
        {
            if (!getterCache.TryGetValue(prop, out var getter))
            {
                var p1 = Expression.Parameter(typeof(object));

                var exp = Expression.Convert(
                        Expression.Property(Expression.Convert(p1, prop.DeclaringType), prop),
                        typeof(object));
                
                var l = Expression.Lambda<Func<object, object>>(exp, p1);

                getter = l.Compile();
                getterCache.Add(prop, getter);
            }

            return getter(target);
        }
    }
}
