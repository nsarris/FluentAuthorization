using SampleApplication.Authorization;
using System.Threading.Tasks;

namespace FluentAuthorization.Tests
{
    class TestUserContextProvider : IUserContextProvider<MyUser>
    {
        private readonly MyUser user;

        public TestUserContextProvider(MyUser user)
        {
            this.user = user;
        }

        public Task<MyUser> GetAsync()
        {
            return Task.FromResult(user);
        }
    }
}