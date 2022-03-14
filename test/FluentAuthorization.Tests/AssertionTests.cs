using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentAuthorization.Tests
{
    public class AssertionTests
    {
        private AssertionResult GetSuccess() => AssertionResult.Success;
        private AssertionResult GetFailure() => new(new AssertionFailure("", "", "", "", ""));

        [Fact]
        public void Should_Assert_Succcess_Is_Default()
        {
            var result1 = new AssertionResult();

            Assert.True(result1.Allow);
        }

        [Fact]
        public void Should_Assert_Failure_Is_Default()
        {
            var result1 = new AssertionResult(new AssertionFailure("", "", "", "", ""));

            Assert.False(result1.Allow);
        }

        [Fact]
        public void Should_Assert_Succcess_With_Logical_Comparison()
        {
            Assert.True(GetSuccess() && GetSuccess());
            Assert.True(GetSuccess() && null);
            Assert.True(null && GetSuccess());

            Assert.True(GetSuccess() || GetFailure());
            Assert.True(GetFailure() || GetSuccess());
            Assert.True(GetSuccess() || null);
            Assert.True(null || GetSuccess());
        }

        [Fact]
        public void Should_Assert_Succcess_With_Binary_Comparison()
        {
            Assert.True(GetSuccess() & GetSuccess());
            Assert.True(GetSuccess() & null);
            Assert.True(null & GetSuccess());

            Assert.True(GetSuccess() | GetFailure());
            Assert.True(GetFailure() | GetSuccess());
            Assert.True(GetSuccess() | null);
            Assert.True(null | GetSuccess());
        }

        [Fact]
        public void Should_Assert_Failure_With_Logical_Comparison()
        {
            Assert.False(GetFailure() && GetFailure());
            Assert.False(GetSuccess() && GetFailure());
            Assert.False(GetFailure() && GetSuccess());
            Assert.False(GetFailure() && null);
            Assert.False(null && GetFailure());

            Assert.False(GetFailure() || GetFailure());
            Assert.False(GetFailure() || null);
            Assert.False(null || GetFailure());
        }

        [Fact]
        public void Should_Assert_Failure_With_Binary_Comparison()
        {
            Assert.False(GetFailure() & GetFailure());
            Assert.False(GetSuccess() & GetFailure());
            Assert.False(GetFailure() & GetSuccess());
            Assert.False(GetFailure() & null);
            Assert.False(null & GetFailure());

            Assert.False(GetFailure() | GetFailure());
            Assert.False(GetFailure() | null);
            Assert.False(null | GetFailure());
        }


        [Fact]
        public void Should_Combine_Failures_Logical_Comparison()
        {
            var result1 = GetFailure() && GetFailure();
            var result2 = GetFailure() || GetFailure();
            
            Assert.True(result1.Failures.Count() == 2);
            Assert.True(result2.Failures.Count() == 2);
        }

        [Fact]
        public void Should_Combine_Failures_Binary_Comparison()
        {
            var result1 = GetFailure() & GetFailure();
            var result2 = GetFailure() | GetFailure();

            Assert.True(result1.Failures.Count() == 2);
            Assert.True(result2.Failures.Count() == 2);
        }
    }
}
