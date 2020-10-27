using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public class PolicyData 
    {
        static readonly object defaultValue = new object();

        private void SetValues(IEnumerable<KeyValuePair<string, object>> values)
        {
            foreach (var value in values)
            {
                var prop = this.GetType().GetProperty(value.Key);
                if (prop != null)
                    try
                    {
                        ReflectionHelper.SetValue(prop, this, value.Value);
                    }
                    catch { }
            }
        }

        private IEnumerable<KeyValuePair<string, object>> GetValues()
        {
            return this.GetType().GetProperties()
                .Select(x =>
                   new KeyValuePair<string, object>(
                       x.Name, ReflectionHelper.GetValue(x, this)));
        }

        public virtual PolicyData Merge(PolicyData next)
        {
            if (this.GetType() != next.GetType())
                throw new InvalidOperationException("Cannot merge policy data of different types");

            if (next == null)
                return this;

            var currentValues = GetValues().ToList();
            var nextValues = next.GetValues().ToList();

            if (currentValues.Count != nextValues.Count)
                throw new InvalidOperationException($"Policy value count mismatch when trying to merge policy data of type {this.GetType().Name}");

            var effectiveValues = new List<KeyValuePair<string, object>>();
            foreach (var items in currentValues.Zip(nextValues, (x, y) => new { Current = x, Next = y }))
            {
                if (items.Current.Key != items.Next.Key)
                    throw new InvalidOperationException($"Policy value sequence mismatch for data of type {this.GetType().Name}");

                object v = null;
                if (items.Current.Value == null && items.Next.Value == null)
                    v = null;
                else if (items.Current.Value == null || items.Next.Value == null)
                    v = items.Current.Value ?? items.Next.Value;
                else
                {
                    if (items.Current.Value.GetType() != items.Next.Value.GetType())
                        throw new InvalidOperationException($"Policy value type mismatch for data of type {this.GetType().Name}");

                    if (typeof(IConfigurable).IsAssignableFrom(items.Current.Value.GetType()))
                    {
                        if (((IConfigurable)items.Next.Value).IsConfigured)
                            v = Configurable.Create(((IConfigurable)items.Next.Value).Value);
                        else if (((IConfigurable)items.Current.Value).IsConfigured)
                            v = Configurable.Create(((IConfigurable)items.Current.Value).Value);
                        else
                            v = defaultValue;
                    }
                    else
                        v = items.Next.Value;
                }

                if (v != defaultValue)
                    effectiveValues.Add(new KeyValuePair<string, object>(items.Current.Key, v));
            }

            var result = (PolicyData)Activator.CreateInstance(this.GetType());
            result.SetValues(effectiveValues);

            //if (result == null)
            //    throw new InvalidOperationException($"Null result returned from PolicyData CreateNew for data of type {this.GetType().Name}");
            //if (result.GetType() != this.GetType())
            //    throw new InvalidOperationException($"Different type returned from PolicyData CreateNew for data of type {this.GetType().Name}");

            return result;
        }
    }
}
