# FluentAuthorization

A technology-agnostic, code-first, fluent authorization framework for defining and asserting security policies in any layer (domain model, application services, background workers, UI gateways, APIs). It is intentionally decoupled from ASP.NET Core to keep authorization logic close to the domain while remaining host independent.

---
## Table of Contents
1. Motivation & Goals
2. Core Concepts
3. Installation
4. Quick Start (Direct Usage)
5. Defining Policies & Permissions
6. Stateless vs Stateful Permissions
7. Data Providers (Supplying `TData`)
8. Building and Using a Policy Context
9. Dependency Injection Integration
10. Working With Results & Failures
11. Tri-State (Undefined) Semantics
12. Dynamic / Name-Based Assertions
13. Testing Strategies
14. Best Practices
15. Extensibility Points
16. FAQ
17. Roadmap Ideas
18. License

---
## 1. Motivation & Goals
Traditional authorization often lives at the outer web layer, making reuse in domain logic awkward. This library:
- Centralizes authorization logic in strongly typed policy classes.
- Enables rich failure diagnostics (not just boolean).
- Allows injecting additional domain data to drive permission checks.
- Supports both stateless and stateful (parameterized) permissions.
- Remains framework agnostic (can be used in console apps, services, tests, ASP.NET Core, etc.).

---
## 2. Core Concepts
| Concept | Description |
|---------|-------------|
| Policy | A class grouping related permissions concerning a (User, Resource, Data) triple. |
| Permission | A single assertion that can Allow, Deny, or be Undefined. May be stateless or stateful (parameterized). |
| Policy Context | Runtime container holding a policy instance, the current user, target resource, and its loaded `IEnumerable<TData>`. |
| Data Provider (`IPolicyDataProvider<TUser>`) | Supplies policy specific data rows used during permission evaluation. |
| User Context Provider (`IUserContextProvider<TUser>`) | Supplies the current user/principal. |
| Assertion Result (`AssertionResult`) | Outcome of a permission evaluation (Allowed / Denied / Undefined + failures). |
| Assertion Failure (`AssertionFailure`) | Describes a reason for denial (policy, permission, message, reason code/text). |

---
## 3. Installation
Add the project or (future) NuGet package to your solution.
- Target frameworks supported: .NET Standard 2.0+, .NET 6+.
- Add a project reference to `FluentAuthorization` (and optionally `FluentAuthorization.DependencyInjection` for DI helpers).

---
## 4. Quick Start (Direct Usage)
Minimal direct (non DI) usage showing a stateless policy.
```csharp
// User representation
a public record Principal(string Id, string Name);

// Resource representation
public record CustomerRecordResource(int Id);

// Policy data row consumed by the policy
public record RecordPolicyData(bool CanView, bool CanUpdate, bool CanDelete, bool IsArchived);

// Concrete policy
a public sealed class CustomerRecordPolicy : Policy<Principal, CustomerRecordResource, RecordPolicyData>
{
    public IPermission View { get; }
    public IPermission Update { get; }
    public IPermission Delete { get; }

    public CustomerRecordPolicy()
    {
        var b = new PermissionBuilder();

        View = b.AssertWith(ctx => ctx.Data.CanView && !ctx.Data.IsArchived)
            .WithName(nameof(View))
            .WithMessage(ctx => $"User cannot view record {ctx.Resource.Id}.")
            .Build();

        Update = b.AssertWith(ctx => ctx.Data.CanUpdate)
            .WithName(nameof(Update))
            .Build();

        Delete = b.AssertWith(ctx => ctx.Data.CanDelete)
            .WithName(nameof(Delete))
            .Build();
    }
}

// Build a context manually (simplified helper for example only)
public IPolicyContext<CustomerRecordPolicy> BuildContext(
    Principal user,
    CustomerRecordPolicy policy,
    CustomerRecordResource resource,
    IEnumerable<RecordPolicyData> dataRows)
{
    // Internally the library constructs policy contexts; expose your own factories as needed.
    return new PolicyContext<CustomerRecordPolicy, Principal, CustomerRecordResource, RecordPolicyData>(
        policy, user, resource, dataRows);
}

var user = new Principal("u1", "Alice");
var policy = new CustomerRecordPolicy();
var resource = new CustomerRecordResource(42);
var data = new [] { new RecordPolicyData(CanView:true, CanUpdate:false, CanDelete:false, IsArchived:false) };
var ctx = BuildContext(user, policy, resource, data);

var viewResult = ctx.Assert(p => p.View);
if (!viewResult.IsAllowed)
{
    // Inspect failures
}
```

---
## 5. Defining Policies & Permissions
A policy subclasses `Policy<TUser, TResource, TData>` and exposes public `IPermission` (stateless) or `IPermission<TState>` (stateful) properties initialized via the `PermissionBuilder`.

Pattern:
```csharp
public sealed class ExamplePolicy : Policy<MyUser, MyResource, ExamplePolicy.Data>
{
    public IPermission Create { get; }
    public IPermission<int> ViewPage { get; }

    public record Data(bool Create, IReadOnlySet<int> PagesAllowed);

    public ExamplePolicy()
    {
        var b = new PermissionBuilder();

        Create = b.AssertWith(ctx => ctx.Data.Create)
            .WithName(nameof(Create))
            .Build();

        ViewPage = b.AssertWith<int>(ctx => ctx.Data.PagesAllowed.Contains(ctx.State))
            .WithName(nameof(ViewPage))
            .WithMessage(ctx => $"Page {ctx.State} not allowed.")
            .Build();
    }
}
```

### Permission Builder Overloads
| Overload | Input Delegate | Semantics |
|----------|----------------|-----------|
| `AssertWith(Func<AssertionContext, AssertionResult>)` | Full control over outcome | Return any result form directly. |
| `AssertWith(Func<AssertionContext, bool>)` | Boolean -> Allow/Deny | Simple yes/no checks. |
| `AssertWith(Func<AssertionContext, bool?>)` | Nullable bool -> Allow/Deny/Undefined | Enables tri-state decisions. |
| `AssertWith<TState>(Func<AssertionContext<TState>, AssertionResult>)` | Stateful full control | Parameterized assertion. |
| `AssertWith<TState>(Func<AssertionContext<TState>, bool>)` | Stateful boolean | Allow/Deny with extra state. |
| `AssertWith<TState>(Func<AssertionContext<TState>, bool?>)` | Stateful tri-state | Parameterized tri-state logic. |

Add optional:
- `.WithName(string)` (recommended; used in failure reporting).
- `.WithMessage(Func<AssertionContext(<>), string>)` for custom failure text.

---
## 6. Stateless vs Stateful Permissions
Stateless permission depends only on user/resource/data. Stateful permission additionally requires an external state value at assertion time.
```csharp
// Stateless
var canCreate = ctx.Assert(p => p.Create);

// Stateful (e.g., page id = 7)
var canViewPage7 = ctx.Assert(p => p.ViewPage, 7);
```

---
## 7. Data Providers (Supplying `TData`)
Implement `IPolicyDataProvider<TUser>` to supply policy specific rows. The library will call:
```csharp
Task<IEnumerable<TData>> GetDataAsync<TPolicy, TResource, TData>(
    TUser user,
    TPolicy policy,
    TResource resource)
    where TPolicy : IPolicy<TUser, TResource, TData>;
```
Example skeleton:
```csharp
public class PolicyDataProvider : IPolicyDataProvider<Principal>
{
    public Task<IEnumerable<TData>> GetDataAsync<TPolicy, TResource, TData>(
        Principal user, TPolicy policy, TResource resource)
        where TPolicy : IPolicy<Principal, TResource, TData>
    {
        if (policy is CustomerRecordPolicy)
        {
            // Query DB / cache / compose domain data
            var row = new RecordPolicyData(CanView:true, CanUpdate:false, CanDelete:false, IsArchived:false);
            return Task.FromResult<IEnumerable<TData>>(new [] { (TData)(object)row });
        }
        throw new InvalidOperationException("Unsupported policy");
    }
}
```
Return as many data rows as needed; policy assertions can aggregate them (e.g., via LINQ) if modeling multiple sources.

---
## 8. Building and Using a Policy Context
A policy context coordinates data retrieval and assertion evaluation.
Typical flow:
1. Resolve current user (via `IUserContextProvider<TUser>` or manual).
2. Instantiate (or resolve) a policy.
3. Provide a resource instance.
4. Build context (sync or async depending on data loading).
5. Call `Assert`.

If using your own construction utilities you may directly instantiate internal context types; ordinarily you rely on `PolicyContextProvider` via DI (see next section).

---
## 9. Dependency Injection Integration
Provided by `FluentAuthorization.DependencyInjection`.
Register services:
```csharp
services.AddFluentAuthorization<Principal>(c => c
    .AddUserContextProvider(sp => new HttpUserContextProvider(), ServiceLifetime.Scoped)
    .AddDataProvider<PolicyDataProvider>(ServiceLifetime.Scoped)
    // Optional custom context provider interface mapping
    .AddCustomPolicyContextProvider<IPolicyContextProvider, PolicyContextProvider>()
);
```
Then inject `IPolicyContextProvider` anywhere:
```csharp
public class CustomerService
{
    private readonly IPolicyContextProvider policies;
    public CustomerService(IPolicyContextProvider policies) => this.policies = policies;

    public async Task<CustomerDto> GetAsync(int id)
    {
        var policy = new CustomerRecordPolicy();
        var resource = new CustomerRecordResource(id);
        var ctx = await policies.ForPolicy(policy).ForResource(resource).BuildAsync();

        var view = ctx.Assert(p => p.View);
        if (!view.IsAllowed) throw new AuthorizationException(view.Failures);

        // continue loading + mapping
        return new CustomerDto(id);
    }
}
```
*(The `ForPolicy(...)`, `ForResource(...)`, and `BuildAsync()` fluent members are inferred from sample usage patterns. Adjust to actual API names if they differ.)*

---
## 10. Working With Results & Failures
`AssertionResult` exposes at minimum a success indicator and a collection of failures when denied.
```csharp
var update = ctx.Assert(p => p.Update);
if (!update.IsAllowed)
{
    foreach (var failure in update.Failures)
    {
        Console.WriteLine($"{failure.PolicyName}.{failure.PermissionName}: {failure.Message} (Reason: {failure.Reason})");
    }
}
```
You can map failures to error contracts, logs, audits, etc.

---
## 11. Tri-State (Undefined) Semantics
Using a nullable boolean assertion (`Func<..., bool?>`) returns `Undefined` when the delegate returns `null`.
```csharp
Escalate = b.AssertWith(ctx =>
{
    if (!ctx.Data.BaseCheck) return false;      // Deny
    if (!ctx.Data.SecondaryLoaded) return (bool?)null; // Undefined
    return ctx.Data.SecondaryApproved;          // Allow/Deny
}).WithName("Escalate").Build();

var res = ctx.Assert(p => p.Escalate);
if (res.IsUndefined)
{
    // Fallback: escalate to another policy or queue review
}
```

---
## 12. Dynamic / Name-Based Assertions
When enumerating permissions dynamically (e.g., UI permission matrix), you can assert by name.
```csharp
string[] permissionNames = { nameof(CustomerRecordPolicy.View), nameof(CustomerRecordPolicy.Update) };
var evaluations = permissionNames
    .Select(n => (Name: n, Result: ctx.Assert(n)))
    .ToDictionary(t => t.Name, t => t.Result.IsAllowed);
```
Use strongly typed lambda form for compile-time safety in most business code.

---
## 13. Testing Strategies
Unit test policies independent of infrastructure:
```csharp
[Fact]
public void View_Denies_When_Archived()
{
    var user = new Principal("u1", "Alice");
    var policy = new CustomerRecordPolicy();
    var resource = new CustomerRecordResource(5);
    var data = new [] { new RecordPolicyData(CanView:true, CanUpdate:false, CanDelete:false, IsArchived:true) };

    var ctx = new PolicyContext<CustomerRecordPolicy, Principal, CustomerRecordResource, RecordPolicyData>(
        policy, user, resource, data);

    var result = ctx.Assert(p => p.View);
    Assert.False(result.IsAllowed);
}
```
Integration test with DI (sample pattern from repository tests):
```csharp
var services = new ServiceCollection();
services.AddFluentAuthorization<Principal>(c => c
    .AddUserContextProvider(sp => new TestUserContextProvider(user), ServiceLifetime.Singleton)
    .AddDataProvider<TestDataProvider>(ServiceLifetime.Transient));
var provider = services.BuildServiceProvider();
var contexts = provider.GetRequiredService<IPolicyContextProvider>();
```

---
## 14. Best Practices
- Keep policy classes cohesive (one aggregate or resource type each).
- Ensure policy instances are stateless (no mutable per request fields) -> treat them as singletons when possible.
- Always set `.WithName(...)` for clarity and stable diagnostics.
- Derive failure messages from contextual data for operator troubleshooting.
- Avoid excessive reflection by preferring lambda assertions in hot paths.
- Batch or cache heavy data provider queries when multiple permissions evaluated sequentially.
- Log denied and undefined results centrally for auditing.

---
## 15. Extensibility Points
| Extension | Approach |
|-----------|----------|
| Additional logging | Decorate `IPolicyContextProvider` or wrap `Assert` calls. |
| Caching data | Implement caching layer inside `IPolicyDataProvider`. |
| Custom result translation | Map `AssertionResult` to domain exceptions or notification objects. |
| Composition (AND/OR) | Add helper methods combining multiple `AssertionResult`s (e.g., `AggregateAll`). |
| Adapters (ASP.NET Core) | Middleware filters calling policy assertions before controllers. |

---
## 16. FAQ
**Q: Why not use ASP.NET Core policies?**  
A: Those are HTTP pipeline oriented; this library decouples authorization for domain and service use without web dependencies.

**Q: How do I localize messages?**  
A: Provide `.WithMessage(ctx => localizer["..."])` using your localization abstraction.

**Q: Can I short circuit multiple permission checks?**  
A: Evaluate sequentially; you can implement a composite helper that stops on first Deny if desired.

**Q: What is `Undefined` useful for?**  
A: It represents `indeterminate` requiring escalation, secondary policy, or deferred workflow.

**Q: Are policies thread safe?**  
A: Keep them immutable; the framework treats them as reusable without per thread state.

---
## 17. Roadmap Ideas
(community contributions welcome.)
- Permission reflection caching & metadata.
- Async permission delegates.
- Built-in composite (AND/OR) evaluators.
- Observer / logging hooks.
- ASP.NET Core adapter package.
- Source generator for strongly typed permission name constants.

---
## 18. License
Refer to the repository root (e.g., `LICENSE` file) for license terms.

---
## Appendix A: Full Example Bringing It Together
```csharp
// 1. Domain models
a public record Principal(string Id, string DisplayName);
public record CustomerEntityResource(int Id);

// 2. Policy data
public record CustomerEntityPolicyData(
    bool Create, bool View, bool Delete, bool ViewVip, bool IsVip);

// 3. Policy definition
public sealed class CustomerEntityPolicy : Policy<Principal, CustomerEntityResource, CustomerEntityPolicyData>
{
    public IPermission Create { get; }
    public IPermission View { get; }
    public IPermission Delete { get; }

    public CustomerEntityPolicy()
    {
        var b = new PermissionBuilder();
        Create = b.AssertWith(ctx => ctx.Data.Create).WithName(nameof(Create)).Build();
        View = b.AssertWith(ctx => ctx.Data.View || (ctx.Data.ViewVip && ctx.Data.IsVip)).WithName(nameof(View)).Build();
        Delete = b.AssertWith(ctx => ctx.Data.Delete && !ctx.Data.IsVip)
            .WithName(nameof(Delete))
            .WithMessage(_ => "Cannot delete VIP customer.")
            .Build();
    }
}

// 4. Data provider
public class CustomerDataProvider : IPolicyDataProvider<Principal>
{
    public Task<IEnumerable<TData>> GetDataAsync<TPolicy, TResource, TData>(
        Principal user, TPolicy policy, TResource resource)
        where TPolicy : IPolicy<Principal, TResource, TData>
    {
        if (policy is CustomerEntityPolicy && resource is CustomerEntityResource r)
        {
            var row = new CustomerEntityPolicyData(
                Create: true,
                View: true,
                Delete: r.Id != 1,
                ViewVip: true,
                IsVip: r.Id == 99);
            return Task.FromResult(new [] { (TData)(object)row }.AsEnumerable());
        }
        throw new InvalidOperationException("Unsupported policy/resource pair.");
    }
}

// 5. Registration
services.AddFluentAuthorization<Principal>(c => c
    .AddUserContextProvider(sp => new HttpUserContextProvider(), ServiceLifetime.Scoped)
    .AddDataProvider<CustomerDataProvider>(ServiceLifetime.Scoped));

// 6. Usage inside an application service
public class CustomerApplicationService
{
    private readonly IPolicyContextProvider policies;
    public CustomerApplicationService(IPolicyContextProvider policies) => this.policies = policies;

    public async Task DoSomethingAsync(int id)
    {
        var policy = new CustomerEntityPolicy();
        var resource = new CustomerEntityResource(id);
        var ctx = await policies.ForPolicy(policy).ForResource(resource).BuildAsync();

        var canDelete = ctx.Assert(p => p.Delete);
        if (!canDelete.IsAllowed)
        {
            // fail fast
            throw new AuthorizationException(canDelete.Failures);
        }
        // proceed with delete logic
    }
}
```

---
