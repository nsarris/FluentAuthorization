using Moq;
using SampleApplication.Authorization;
using SampleApplication.Authorization.Policies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FluentAuthorization.Tests
{
    public class AssertionTests
    {
        private IPolicyContextProvider<TUser> GetPolicyContextProvider<TUser>(TUser user)
        {
            var dataProviderMock = new Mock<IPolicyDataProvider<TUser>>();
            var userProviderMock = new Mock<IUserContextProvider<TUser>>();

            userProviderMock
                .Setup(x => x.GetAsync())
                .Returns(Task.FromResult(user));

            return new PolicyContextProvider<TUser>(
                userProviderMock.Object,
                dataProviderMock.Object);
        }

        public class User { }

        public class DefaultPolicy : Policy<User, Resource, DefaultPolicy.Data>
        {
            public class Data
            {
                public static readonly Data True = new () { Allow = true };
                public static readonly Data False = new () { Allow = false };
                public static readonly Data Null = new () { Allow = null };

                public bool? Allow { get; set; }
            }

            public Permission Allows { get; }

            public DefaultPolicy()
            {
                Allows = permissionBuilder.AssertWith(x => x.Data.Allow).Build();
            }
        }

        public class UndefinedAsAllowedPolicy : DefaultPolicy
        {
            public UndefinedAsAllowedPolicy()
            {
                this.TreatUndefinedAsDeny = false;
            }
        }

        public class AggregateDataBeforeAssertionPolicy : DefaultPolicy
        {
            public AggregateDataBeforeAssertionPolicy()
            {
                this.AggregateDataBeforeAssertion = true;
            }

            public override Data Aggregate(IEnumerable<Data> data)
            {
                return data.FirstOrDefault(x => x.Allow == true)
                    ?? data.FirstOrDefault(x => x.Allow == false)
                    ?? data.FirstOrDefault(x => x.Allow == null)
                    ?? new();
            }
        }

        public class Resource { }

        [Theory]
        [MemberData(nameof(GetPolicyDataForAggregationTests_With_Default_Options))]
        public async Task Should_Aggegrate_Assertions_With_Default_Options(IEnumerable<DefaultPolicy.Data> data, bool? expected)
        {
            var policyContext = await GetPolicyContextProvider(new User())
                .ForResource(new Resource())
                .ForPolicy<DefaultPolicy>()
                .BuildContextAsync(data);

            var result = policyContext.Assert(x => x.Allows);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetPolicyDataForAggregationTests_With_Default_Options()
            => new object[][] {
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.False, DefaultPolicy.Data.Null }, false },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.False }, false },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.Null }, true },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.False, DefaultPolicy.Data.Null }, false },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.True}, true},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.False, DefaultPolicy.Data.False}, false},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.False }, false},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True }, true},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.Null }, false},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.Null, DefaultPolicy.Data.Null }, false},
            };

        [Theory]
        [MemberData(nameof(GetPolicyDataForAggregationTests_With_Undefined_As_Allowed))]
        public async Task Should_Aggegrate_Assertions_With_Undefined_As_Allowed(IEnumerable<DefaultPolicy.Data> data, bool? expected)
        {
            var policyContext = await GetPolicyContextProvider(new User())
                .ForResource(new Resource())
                .ForPolicy<UndefinedAsAllowedPolicy>()
                .BuildContextAsync(data);

            var result = policyContext.Assert(x => x.Allows);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetPolicyDataForAggregationTests_With_Undefined_As_Allowed()
            => new object[][] {
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.False, DefaultPolicy.Data.Null }, false },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.False }, false },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.Null }, true },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.False, DefaultPolicy.Data.Null }, false },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.True}, true},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.False, DefaultPolicy.Data.False}, false},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.False }, false},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True }, true},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.Null }, true},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.Null, DefaultPolicy.Data.Null }, true},
            };

        [Theory]
        [MemberData(nameof(GetPolicyDataForAggregationTests_With_Aggregate_Data_Before_Assertion))]
        public async Task Should_Aggegrate_Assertions_With_Aggregate_Data_Before_Assertion(IEnumerable<DefaultPolicy.Data> data, bool? expected)
        {
            var policyContext = await GetPolicyContextProvider(new User())
                .ForResource(new Resource())
                .ForPolicy<AggregateDataBeforeAssertionPolicy>()
                .BuildContextAsync(data);

            var result = policyContext.Assert(x => x.Allows);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetPolicyDataForAggregationTests_With_Aggregate_Data_Before_Assertion()
            => new object[][] {
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.False, DefaultPolicy.Data.Null }, true },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.False }, true },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.Null }, true },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.False, DefaultPolicy.Data.Null }, false },
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True, DefaultPolicy.Data.True}, true},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.False, DefaultPolicy.Data.False}, false},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.False }, false},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.True }, true},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.Null }, false},
                new object[] { new DefaultPolicy.Data[] { DefaultPolicy.Data.Null, DefaultPolicy.Data.Null }, false},
            };
    }
}
