using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public interface IConfigurable
    {
        bool HasValue { get; }
        bool IsConfigured { get; }
        object Value { get; }
        Type UnderlyingType { get; }
        Configurable<Tout> ConvertTo<Tout>();
        IConfigurable ConvertTo(Type type);
    }

    [System.Diagnostics.DebuggerDisplay("{IsConfigured ? this : (object)Value}")]
    public struct Configurable<T> : IConfigurable
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly T value;

        public bool HasValue { get; private set; }
        public bool IsConfigured => !HasValue;
        public T Value => value;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        object IConfigurable.Value => value;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        Type IConfigurable.UnderlyingType => typeof(T);

        public Configurable(T v)
        {
            value = v;
            HasValue = true;
        }

        public static implicit operator Configurable<T>(T value)
        {
            return new Configurable<T>(value);
        }



        public static explicit operator T(Configurable<T> value)
        {
            return value.Value;
        }

        public T GetValueOrDefault()
        {
            return value;
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return HasValue ? value : defaultValue;
        }

        public override bool Equals(object other)
        {
            if (!HasValue) return other == null;
            if (other == null) return false;
            return value.Equals(other);
        }

        public override int GetHashCode()
        {
            return HasValue ? value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return HasValue ? value.ToString() : "";
        }

        public Configurable<Tout> ConvertTo<Tout>()
        {
            Configurable<Tout> e = new Configurable<Tout>();
            if (!IsConfigured)
            {
                if (typeof(T) == typeof(Tout))
                    e = new Configurable<Tout>(((Tout)(object)value));
                else if (typeof(T) != typeof(Tout))
                    e = new Configurable<Tout>((Tout)Convert.ChangeType(value, typeof(T)));
            }

            return e;
        }

        public IConfigurable ConvertTo(Type type)
        {
            if (!IsConfigured)
            {
                if (typeof(T) == type)
                    return Configurable.Create(value);
                else
                    return Configurable.Create(Convert.ChangeType(value, type));
            }
            else
                return Configurable.Create(type);
        }
    }

    public static class Configurable
    {
        public static readonly IConfigurable Empty = new Configurable<object>();
        public static IConfigurable Create(object value)
        {
            if (value == null)
                return new Configurable<object>(null);
            else
                return (IConfigurable)Activator.CreateInstance(typeof(Configurable<>).MakeGenericType(new[] { value.GetType() }), new object[] { value });
        }

        public static IConfigurable Create(Type type)
        {
            return (IConfigurable)Activator.CreateInstance(typeof(Configurable<>).MakeGenericType(new[] { type }), new object[] { });
        }

        public static Type GetUnderlyingType(Type ConfigurableTyle)
        {
            if (!ConfigurableTyle.IsGenericType || ConfigurableTyle.GetGenericTypeDefinition() != typeof(Configurable<>))
                throw new ArgumentException("Type " + ConfigurableTyle.Name + " is not Configurable<>");

            return ConfigurableTyle.GetGenericArguments().First();
        }
    }
}