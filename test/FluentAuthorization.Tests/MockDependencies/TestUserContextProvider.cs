using SampleApplication.Authorization;
using System.Threading.Tasks;

namespace FluentAuthorization.Tests
{
    class TestUserContextProvider : IUserContextProvider<Principal>
    {
        private readonly Principal user;

        public TestUserContextProvider(Principal user)
        {
            this.user = user;
        }

        public Task<Principal> GetAsync()
        {
            return Task.FromResult(user);
        }
    }
}