using SampleApplication.Authorization;
using System.Threading.Tasks;

namespace FluentAuthorization.Tests
{
    class TestUserContextProvider : IUserContextProvider<MyUserSecurityContext>
    {
        private readonly MyUserSecurityContext user;

        public TestUserContextProvider(MyUserSecurityContext user)
        {
            this.user = user;
        }

        public Task<MyUserSecurityContext> GetAsync()
        {
            return Task.FromResult(user);
        }
    }
}