using Microsoft.AspNetCore.Identity;
using MiniBlog.Model;
using MiniBlog.Repositories;
using MiniBlog.Services;
using MiniBlog.Stores;
using Moq;
using Snappier;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MiniBlogTest.ServiceTest;

public class ArticleServiceTest_v1
{
    [Fact]
    public void Should_return_articles_when_invoke_GetById_given_Id()
    {
        // given
        string id = "123";
        var mockedArticleRepository = new Mock<IArticleRepository>();
        var mockUser = new Mock<IUserRepository>();
        mockedArticleRepository.Setup(repository => repository.GetArticleById(id)).Returns(Task.FromResult(new Article("Jerry", "Happy new year", "Happy 2021 new year")));

        var articleService = new ArticleService(mockedArticleRepository.Object, mockUser.Object);

        // when
        var article = articleService.GetById(id);

        // then
        Assert.Equal("Happy new year", article.Result.Title);
    }

    [Fact]
    public async void Should_create_article_and_not_create_user_when_invoke_createArticle_given_user_name_existed()
    {
        // given
        var mockedArticleRepository = new Mock<IArticleRepository>();
        var mockedUserRepository = new Mock<IUserRepository>();
        mockedUserRepository.Setup(repository => repository.GetUserByName("Jerry")).Returns(Task.FromResult(new User("Jerry")));
        mockedUserRepository.Setup(repository => repository.CreateUser(It.IsAny<User>())).Returns(Task.CompletedTask);
        mockedArticleRepository.Setup(repository => repository.CreateArticle(It.IsAny<Article>())).Returns(Task.CompletedTask);

        var newArticle = new Article("Jerry", "Let's code", "c#")
        { Id = string.Empty };

        var articleService = new ArticleService(mockedArticleRepository.Object, mockedUserRepository.Object);

        // when
        await articleService.CreateArticle(newArticle);

        // then
        mockedArticleRepository.Verify(repository => repository.CreateArticle(It.IsAny<Article>()), Times.Once);
        mockedUserRepository.Verify(repository => repository.CreateUser(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async void Should_create_article_and_create_user_when_invoke_createArticle_given_user_name_not_existed()
    {
        // given
        var mockedArticleRepository = new Mock<IArticleRepository>();
        var mockedUserRepository = new Mock<IUserRepository>();
        mockedUserRepository.Setup(repository => repository.GetUserByName("Jerry")).Returns(Task.FromResult<User>(null));
        mockedUserRepository.Setup(repository => repository.CreateUser(It.IsAny<User>())).Returns(Task.CompletedTask);
        mockedArticleRepository.Setup(repository => repository.CreateArticle(It.IsAny<Article>())).Returns(Task.CompletedTask);

        var newArticle = new Article("Jerry", "Let's code", "c#")
        { Id = string.Empty };

        var articleService = new ArticleService(mockedArticleRepository.Object, mockedUserRepository.Object);

        // when
        await articleService.CreateArticle(newArticle);

        // then
        mockedArticleRepository.Verify(repository => repository.CreateArticle(It.IsAny<Article>()), Times.Once);
        mockedUserRepository.Verify(repository => repository.CreateUser(It.IsAny<User>()), Times.Once);
    }
}
