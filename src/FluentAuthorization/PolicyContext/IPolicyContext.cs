using System;

namespace FluentAuthorization
{
    public interface IPolicyContext<out T> where T : IPolicy
    {
        T Policy { get; }

        AssertionResult Assert(Func<T, IPermission> select);
        AssertionResult Assert<TState>(Func<T, IPermission<TState>> select, TState state);
    }
}
