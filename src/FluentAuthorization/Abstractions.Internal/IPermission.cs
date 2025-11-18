namespace FluentAuthorization
{
    /// <summary>
    /// Abtraction of a permission. Internal marker interface, not intended for implementation.
    /// </summary>
    public interface IPermission
    {
        
    }

    /// <summary>
    /// Abtraction of a permission with state. Internal marker interface, not intended for implementation.
    /// </summary>
    public interface IPermission<TState>
    {
        
    }

    /// <summary>
    /// Abstraction of an async permission. Internal marker interface, not intended for implementation.
    /// </summary>
    public interface IAsyncPermission
    {
        
    }

    /// <summary>
    /// Abstraction of an async permission with state. Internal marker interface, not intended for implementation.
    /// </summary>
    public interface IAsyncPermission<TState>
    {
        
    }
}
