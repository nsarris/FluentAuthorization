using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FluentAuthorization.Tests
{
    /// <summary>
    /// Comprehensive tests for async permission delegates
    /// </summary>
    public class AsyncAssertionTests
    {
        #region Test Models

        public record User(string Id, string Name, bool IsAdmin);
        public record Document(int Id, string OwnerId);

        private static User CreateUser(string id, string name, bool isAdmin = false) => new User(id, name, isAdmin);

        #endregion

        #region Basic Async Permission Tests

        public class AsyncPermissionPolicy : Policy<User, Document, AsyncPermissionPolicy.Data>
        {
            public class Data
            {
                public static readonly Data True = new() { Allow = true };
                public static readonly Data False = new() { Allow = false };
                public static readonly Data Null = new() { Allow = null };

                public bool? Allow { get; set; }
                public int DelayMs { get; set; } = 10;
            }

            public AsyncPermission Read { get; }
            public AsyncPermission Write { get; }

            public AsyncPermissionPolicy()
            {
                // Test Task<bool> return type
                Read = permissionBuilder.AssertWithAsync(async ctx =>
                {
                    await Task.Delay(ctx.Data.DelayMs);
                    return ctx.Data.Allow == true;
                })
                .WithName(nameof(Read))
                .WithMessage(ctx => $"{ctx.User.Name} cannot read document {ctx.Resource.Id}")
                .Build();

                // Test Task<bool?> return type (tri-state)
                Write = permissionBuilder.AssertWithAsync(async ctx =>
                {
                    await Task.Delay(ctx.Data.DelayMs);
                    return ctx.Data.Allow;
                })
                .WithName(nameof(Write))
                .Build();
            }
        }

        [Fact]
        public async Task AsyncPermission_Should_Allow_When_Delegate_Returns_True()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { AsyncPermissionPolicy.Data.True };

            var context = new PolicyContext<AsyncPermissionPolicy, User, Document, AsyncPermissionPolicy.Data>(
                new AsyncPermissionPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.Read);

            Assert.True(result);
        }

        [Fact]
        public async Task AsyncPermission_Should_Deny_When_Delegate_Returns_False()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { AsyncPermissionPolicy.Data.False };

            var context = new PolicyContext<AsyncPermissionPolicy, User, Document, AsyncPermissionPolicy.Data>(
                new AsyncPermissionPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.Read);

            Assert.False(result);
            Assert.NotEmpty(result.Failures);
        }

        [Fact]
        public async Task AsyncPermission_Should_Return_Undefined_When_Delegate_Returns_Null()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { AsyncPermissionPolicy.Data.Null };

            var context = new PolicyContext<AsyncPermissionPolicy, User, Document, AsyncPermissionPolicy.Data>(
                new AsyncPermissionPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.Write);

            // With default TreatUndefinedAsDeny = true, undefined becomes deny
            Assert.False(result);
        }

        #endregion

        #region Stateful Async Permission Tests

        public class StatefulAsyncPermissionPolicy : Policy<User, Document, StatefulAsyncPermissionPolicy.Data>
        {
            public class Data
            {
                public string[] AllowedTargets { get; set; } = Array.Empty<string>();
                public int DelayMs { get; set; } = 10;
            }

            public AsyncPermission<string> ShareWith { get; }

            public StatefulAsyncPermissionPolicy()
            {
                ShareWith = permissionBuilder.AssertWithAsync<string>(async ctx =>
                {
                    await Task.Delay(ctx.Data.DelayMs);

                    if (string.IsNullOrEmpty(ctx.State))
                        return (bool?)null; // Undefined

                    if (ctx.State == ctx.User.Id)
                        return false; // Cannot share with self

                    return ctx.Data.AllowedTargets.Contains(ctx.State);
                })
                .WithName(nameof(ShareWith))
                .WithMessage(ctx => $"Cannot share document {ctx.Resource.Id} with user {ctx.State}")
                .Build();
            }
        }

        [Fact]
        public async Task StatefulAsyncPermission_Should_Allow_When_State_Matches()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { new StatefulAsyncPermissionPolicy.Data { AllowedTargets = new[] { "user2", "user3" } } };

            var context = new PolicyContext<StatefulAsyncPermissionPolicy, User, Document, StatefulAsyncPermissionPolicy.Data>(
                new StatefulAsyncPermissionPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.ShareWith, "user2");

            Assert.True(result);
        }

        [Fact]
        public async Task StatefulAsyncPermission_Should_Deny_When_State_Does_Not_Match()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { new StatefulAsyncPermissionPolicy.Data { AllowedTargets = new[] { "user2" } } };

            var context = new PolicyContext<StatefulAsyncPermissionPolicy, User, Document, StatefulAsyncPermissionPolicy.Data>(
                new StatefulAsyncPermissionPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.ShareWith, "user3");

            Assert.False(result);
        }

        [Fact]
        public async Task StatefulAsyncPermission_Should_Deny_When_Sharing_With_Self()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { new StatefulAsyncPermissionPolicy.Data { AllowedTargets = new[] { "user1" } } };

            var context = new PolicyContext<StatefulAsyncPermissionPolicy, User, Document, StatefulAsyncPermissionPolicy.Data>(
                new StatefulAsyncPermissionPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.ShareWith, "user1");

            Assert.False(result);
        }

        #endregion

        #region Async Permission with Task<AssertionResult>

        public class FullControlAsyncPolicy : Policy<User, Document, FullControlAsyncPolicy.Data>
        {
            public class Data
            {
                public bool CheckPassed { get; set; }
                public string[] Reasons { get; set; } = Array.Empty<string>();
            }

            public FluentAuthorization.IAsyncPermission Access { get; }

            public FullControlAsyncPolicy()
            {
                Access = permissionBuilder.AssertWithAsync(async ctx =>
                {
                    await Task.Delay(10);

                    if (ctx.Data.CheckPassed)
                        return ctx.Allow();

                    if (ctx.Data.Reasons.Any())
                        return ctx.Deny(ctx.Data.Reasons);

                    return ctx.Deny("Access denied");
                })
                .WithName(nameof(Access))
                .Build();
            }
        }

        [Fact]
        public async Task AsyncPermission_With_AssertionResult_Should_Allow()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { new FullControlAsyncPolicy.Data { CheckPassed = true } };

            var context = new PolicyContext<FullControlAsyncPolicy, User, Document, FullControlAsyncPolicy.Data>(
                new FullControlAsyncPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.Access);

            Assert.True(result);
        }

        [Fact]
        public async Task AsyncPermission_With_AssertionResult_Should_Deny_With_Multiple_Reasons()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var reasons = new[] { "Insufficient privileges", "Account locked" };
            var data = new[] { new FullControlAsyncPolicy.Data { CheckPassed = false, Reasons = reasons } };

            var context = new PolicyContext<FullControlAsyncPolicy, User, Document, FullControlAsyncPolicy.Data>(
                new FullControlAsyncPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.Access);

            Assert.False(result);
            Assert.Equal(2, result.Failures.Count());
        }

        #endregion

        #region Async Permission Aggregation Tests

        public class AsyncAggregationPolicy : Policy<User, Document, AsyncAggregationPolicy.Data>
        {
            public class Data
            {
                public bool? Allow { get; set; }
            }

            public FluentAuthorization.IAsyncPermission MultiData { get; }

            public AsyncAggregationPolicy()
            {
                MultiData = permissionBuilder.AssertWithAsync(async ctx =>
                {
                    await Task.Delay(5);
                    return ctx.Data.Allow;
                })
                .WithName(nameof(MultiData))
                .Build();
            }
        }

        [Theory]
        [MemberData(nameof(GetAsyncAggregationTestData))]
        public async Task Should_Aggregate_Async_Assertions_Correctly(IEnumerable<bool?> allowValues, bool expected)
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = allowValues.Select(a => new AsyncAggregationPolicy.Data { Allow = a });

            var context = new PolicyContext<AsyncAggregationPolicy, User, Document, AsyncAggregationPolicy.Data>(
                new AsyncAggregationPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.MultiData);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetAsyncAggregationTestData()
            => new object[][] {
                new object[] { new bool?[] { true, false, null }, false },
                new object[] { new bool?[] { true, false }, false },
                new object[] { new bool?[] { true, null }, true },
                new object[] { new bool?[] { false, null }, false },
                new object[] { new bool?[] { true, true }, true },
                new object[] { new bool?[] { false, false }, false },
                new object[] { new bool?[] { true }, true },
                new object[] { new bool?[] { false }, false },
                new object[] { new bool?[] { null }, false }, // Undefined treated as deny by default
            };

        #endregion

        #region Override Tests for Async Permissions

        public class AsyncOverridePolicy : Policy<User, Document, AsyncOverridePolicy.Data>
        {
            public class Data
            {
                public bool? Allow { get; set; }
            }

            public FluentAuthorization.IAsyncPermission Check { get; }

            public AsyncOverridePolicy()
            {
                Check = permissionBuilder.AssertWithAsync(async ctx =>
                {
                    await Task.Delay(10);
                    return ctx.Data.Allow == true;
                })
                .WithName(nameof(Check))
                .Build();
            }

            protected override AssertionResult OverrideAssertion(User user, Document resource, string permissionName, IEnumerable<Data> data)
            {
                return user.IsAdmin ? AssertionResult.Success : null;
            }
        }

        [Fact]
        public async Task AsyncPermission_Should_Respect_Override_For_Admin()
        {
            var adminUser = CreateUser("admin1", "Admin", isAdmin: true);
            var document = new Document(42, "user1");
            var data = new[] { new AsyncOverridePolicy.Data { Allow = false } }; // Would normally deny

            var context = new PolicyContext<AsyncOverridePolicy, User, Document, AsyncOverridePolicy.Data>(
                new AsyncOverridePolicy(), adminUser, document, data);

            var result = await context.AssertAsync(p => p.Check);

            Assert.True(result); // Override allows admin
        }

        [Fact]
        public async Task AsyncPermission_Should_Execute_Delegate_When_Override_Returns_Null()
        {
            var regularUser = CreateUser("user1", "Alice", isAdmin: false);
            var document = new Document(42, "user1");
            var data = new[] { new AsyncOverridePolicy.Data { Allow = true } };

            var context = new PolicyContext<AsyncOverridePolicy, User, Document, AsyncOverridePolicy.Data>(
                new AsyncOverridePolicy(), regularUser, document, data);

            var result = await context.AssertAsync(p => p.Check);

            Assert.True(result); // Delegate executes normally
        }

        #endregion

        #region String-based (Reflection) Async Permission Tests

        [Fact]
        public async Task Should_Assert_Async_Permission_By_Name()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { AsyncPermissionPolicy.Data.True };

            var context = new PolicyContext<AsyncPermissionPolicy, User, Document, AsyncPermissionPolicy.Data>(
                new AsyncPermissionPolicy(), user, document, data);

            var result = await context.AssertAsync(nameof(AsyncPermissionPolicy.Read));

            Assert.True(result);
        }

        [Fact]
        public async Task Should_Assert_Stateful_Async_Permission_By_Name()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { new StatefulAsyncPermissionPolicy.Data { AllowedTargets = new[] { "user2" } } };

            var context = new PolicyContext<StatefulAsyncPermissionPolicy, User, Document, StatefulAsyncPermissionPolicy.Data>(
                new StatefulAsyncPermissionPolicy(), user, document, data);

            var result = await context.AssertAsync<string>(nameof(StatefulAsyncPermissionPolicy.ShareWith), "user2");

            Assert.True(result);
        }

        #endregion

        #region Extension Method Tests

        [Fact]
        public async Task ThrowOnDenyAsync_Should_Not_Throw_When_Allowed()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { AsyncPermissionPolicy.Data.True };

            var context = new PolicyContext<AsyncPermissionPolicy, User, Document, AsyncPermissionPolicy.Data>(
                new AsyncPermissionPolicy(), user, document, data);

            await context.ThrowOnDenyAsync(p => p.Read);

            // No exception = test passes
        }

        [Fact]
        public async Task ThrowOnDenyAsync_Should_Throw_When_Denied()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { AsyncPermissionPolicy.Data.False };

            var context = new PolicyContext<AsyncPermissionPolicy, User, Document, AsyncPermissionPolicy.Data>(
                new AsyncPermissionPolicy(), user, document, data);

            await Assert.ThrowsAsync<PolicyAssertionException>(async () =>
                await context.ThrowOnDenyAsync(p => p.Read));
        }

        [Fact]
        public async Task ThrowOnDenyAsync_Stateful_Should_Throw_When_Denied()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { new StatefulAsyncPermissionPolicy.Data { AllowedTargets = new[] { "user2" } } };

            var context = new PolicyContext<StatefulAsyncPermissionPolicy, User, Document, StatefulAsyncPermissionPolicy.Data>(
                new StatefulAsyncPermissionPolicy(), user, document, data);

            await Assert.ThrowsAsync<PolicyAssertionException>(async () =>
                await context.ThrowOnDenyAsync(p => p.ShareWith, "user3"));
        }

        #endregion

        #region Custom Async Permission Class Tests

        public class CustomAsyncPermissionPolicy : Policy<User, Document, CustomAsyncPermissionPolicy.Data>
        {
            public class Data
            {
                public bool CanAccess { get; set; }
            }

            public class CustomPermission : AsyncPermission
            {
                public override string Name => "CustomAsync";

                protected override async Task<AssertionResult> AssertAsync(AssertionContext context)
                {
                    await Task.Delay(10);

                    if (context.Data.CanAccess)
                        return context.Allow();

                    return context.Deny("Custom async permission denied");
                }

                protected override string BuildMessage(AssertionContext context)
                {
                    return $"Custom async permission for {context.User.Name} on document {context.Resource.Id}";
                }
            }

            public FluentAuthorization.IAsyncPermission Custom { get; }

            public CustomAsyncPermissionPolicy()
            {
                Custom = new CustomPermission();
            }
        }

        [Fact]
        public async Task Custom_Async_Permission_Class_Should_Work()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { new CustomAsyncPermissionPolicy.Data { CanAccess = true } };

            var context = new PolicyContext<CustomAsyncPermissionPolicy, User, Document, CustomAsyncPermissionPolicy.Data>(
                new CustomAsyncPermissionPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.Custom);

            Assert.True(result);
        }

        [Fact]
        public async Task Custom_Async_Permission_Class_Should_Deny_With_Custom_Message()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { new CustomAsyncPermissionPolicy.Data { CanAccess = false } };

            var context = new PolicyContext<CustomAsyncPermissionPolicy, User, Document, CustomAsyncPermissionPolicy.Data>(
                new CustomAsyncPermissionPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.Custom);

            Assert.False(result);
            var failure = result.Failures.First();
            Assert.Equal("Custom async permission denied", failure.Reason);
            Assert.Contains("Alice", failure.Message);
            Assert.Contains("42", failure.Message);
        }

        #endregion

        #region Undefined Behavior Tests

        public class UndefinedAsAllowedAsyncPolicy : Policy<User, Document, UndefinedAsAllowedAsyncPolicy.Data>
        {
            public class Data
            {
                public bool? Allow { get; set; }
            }

            public FluentAuthorization.IAsyncPermission Check { get; }

            public UndefinedAsAllowedAsyncPolicy()
            {
                TreatUndefinedAsDeny = false;

                Check = permissionBuilder.AssertWithAsync(async ctx =>
                {
                    await Task.Delay(10);
                    return ctx.Data.Allow;
                })
                .WithName(nameof(Check))
                .Build();
            }
        }

        [Fact]
        public async Task AsyncPermission_Should_Allow_Undefined_When_TreatUndefinedAsDeny_Is_False()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { new UndefinedAsAllowedAsyncPolicy.Data { Allow = null } };

            var context = new PolicyContext<UndefinedAsAllowedAsyncPolicy, User, Document, UndefinedAsAllowedAsyncPolicy.Data>(
                new UndefinedAsAllowedAsyncPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.Check);

            Assert.True(result); // Undefined treated as allow
        }

        #endregion

        #region Data Aggregation Before Assertion Tests

        public class AggregateDataBeforeAssertionAsyncPolicy : Policy<User, Document, AggregateDataBeforeAssertionAsyncPolicy.Data>
        {
            public class Data
            {
                public bool? Allow { get; set; }
            }

            public FluentAuthorization.IAsyncPermission Check { get; }

            public AggregateDataBeforeAssertionAsyncPolicy()
            {
                AggregateDataBeforeAssertion = true;

                Check = permissionBuilder.AssertWithAsync(async ctx =>
                {
                    await Task.Delay(10);
                    return ctx.Data.Allow == true;
                })
                .WithName(nameof(Check))
                .Build();
            }

            public override Data Aggregate(IEnumerable<Data> data)
            {
                // Return first true, else first false, else first null
                return data.FirstOrDefault(x => x.Allow == true)
                    ?? data.FirstOrDefault(x => x.Allow == false)
                    ?? data.FirstOrDefault(x => x.Allow == null)
                    ?? new();
            }
        }

        [Fact]
        public async Task AsyncPermission_Should_Aggregate_Data_Before_Assertion()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            // Mixed data - aggregation should pick first true
            var data = new[] {
                new AggregateDataBeforeAssertionAsyncPolicy.Data { Allow = false },
                new AggregateDataBeforeAssertionAsyncPolicy.Data { Allow = true },
                new AggregateDataBeforeAssertionAsyncPolicy.Data { Allow = null }
            };

            var context = new PolicyContext<AggregateDataBeforeAssertionAsyncPolicy, User, Document, AggregateDataBeforeAssertionAsyncPolicy.Data>(
                new AggregateDataBeforeAssertionAsyncPolicy(), user, document, data);

            var result = await context.AssertAsync(p => p.Check);

            Assert.True(result);
        }

        #endregion

        #region Performance and Concurrency Tests

        [Fact]
        public async Task Multiple_Async_Assertions_Should_Execute_Sequentially()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] {
                new AsyncPermissionPolicy.Data { Allow = true, DelayMs = 50 },
                new AsyncPermissionPolicy.Data { Allow = true, DelayMs = 50 }
            };

            var context = new PolicyContext<AsyncPermissionPolicy, User, Document, AsyncPermissionPolicy.Data>(
                new AsyncPermissionPolicy(), user, document, data);

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var result = await context.AssertAsync(p => p.Read);
            sw.Stop();

            Assert.True(result);
            // Should take at least 100ms (2 x 50ms) if executed sequentially
            Assert.True(sw.ElapsedMilliseconds >= 90, $"Expected >= 90ms, got {sw.ElapsedMilliseconds}ms");
        }

        [Fact]
        public async Task Multiple_Async_Assertions_Can_Run_In_Parallel()
        {
            var user = CreateUser("user1", "Alice");
            var document = new Document(42, "user1");
            var data = new[] { AsyncPermissionPolicy.Data.True };

            var context = new PolicyContext<AsyncPermissionPolicy, User, Document, AsyncPermissionPolicy.Data>(
                new AsyncPermissionPolicy(), user, document, data);

            // Run multiple assertions in parallel
            var tasks = new[]
            {
                context.AssertAsync(p => p.Read),
                context.AssertAsync(p => p.Write),
                context.AssertAsync(p => p.Read)
            };

            var results = await Task.WhenAll(tasks);

            Assert.All(results, r => Assert.True(r));
        }

        #endregion
    }
}
