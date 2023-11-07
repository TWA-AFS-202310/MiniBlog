using MiniBlog.Model;
using MiniBlog.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MiniBlogTest.RepositoryTest
{
    public class UserRepositoryTest
    {
        [Fact]
        public void Should_return_created_user_When_createUser()
        {
            var mock = new Mock<IUserRepository>();
            var userName = "Tom";
            var originalEmail = "a@b.com";
            var user = new User(userName, originalEmail);

            mock.Setup(repository => repository.CreateUser(user)).Returns(Task.FromResult(user));

            IUserRepository userRepository = mock.Object;
            Task<User> userGot = userRepository.CreateUser(user);

            mock.Verify(repository => repository.CreateUser(user), Times.Once());
        }

        [Fact]
        public void Should_return_the_user_When_getArticle_Given_name()
        {
            var mock = new Mock<IUserRepository>();
            var userName = "Tom";
            var originalEmail = "a@b.com";
            var user = new User(userName, originalEmail);
            mock.Setup(repository => repository.GetUser(userName)).Returns(Task.FromResult(user));

            IUserRepository userRepository = mock.Object;
            Task<User> userGot = userRepository.GetUser(userName);

            mock.Verify(repository => repository.GetUser(userName), Times.Once());
        }
    }
}
