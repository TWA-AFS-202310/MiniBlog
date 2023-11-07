 using MiniBlog.Model;
using MiniBlog.Repositories;
using MiniBlog.Services;
using MiniBlog.Stores;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MiniBlogTest.ServiceTest;

public class ArticleServiceTest
{
    [Fact]
    public void Should_return_all_article_When_getArticles()
    {
        var mock = new Mock<IArticleService>();
        List<Article> result = new List<Article>
            {
                new Article(null, "Happy new year", "Happy 2021 new year"),
                new Article(null, "Happy Halloween", "Halloween is coming"),
            };
        mock.Setup(service => service.GetAll()).Returns(Task.FromResult(result));

        IArticleService articleService = mock.Object;
        Task<List<Article>> articles = articleService.GetAll();

        mock.Verify(service => service.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_call_article_repository_once_When_created_article_When_createArticle_Given_no_userName()
    {
        var mock = new Mock<IArticleService>();
        string userNameWhoWillAdd = null;
        string articleContent = "What a good day today!";
        string articleTitle = "Good day";
        Article article = new Article(userNameWhoWillAdd, articleTitle, articleContent);
        mock.Setup(service => service.CreateArticle(article)).Returns(Task.FromResult(article));

        IArticleService articleService = mock.Object;
        Task<Article> articles = articleService.CreateArticle(article);

        mock.Verify(service => service.CreateArticle(article), Times.Once());
    }

    [Fact]
    public void Should_call_user_and_article_repository_When_created_article_When_createArticle_Given_userName()
    {
        var mockArticle = new Mock<IArticleService>();
        var mockUser = new Mock<IUserRepository>();
        var userName = "KE";
        var originalEmail = "a@b.com";
        var user = new User(userName, originalEmail);
        string userNameWhoWillAdd = "KE";
        string articleContent = "What a good day today!";
        string articleTitle = "Good day";
        Article article = new Article(userNameWhoWillAdd, articleTitle, articleContent);
        mockArticle.Setup(service => service.CreateArticle(article)).Returns(Task.FromResult(article));
        mockUser.Setup(repository => repository.CreateUser(user)).Returns(Task.FromResult(user));

        IArticleService articleService = mockArticle.Object;
        IUserRepository userRepository = mockUser.Object;
        Task<Article> articles = articleService.CreateArticle(article);
        Task<User> userGot = userRepository.CreateUser(user);

        mockUser.Verify(repository => repository.CreateUser(user), Times.Once());
        mockArticle.Verify(service => service.CreateArticle(article), Times.Once());
    }

    [Fact]
    public void Should_return_the_article_When_getArticle_Given_id()
    {
        var mock = new Mock<IArticleService>();
        string userNameWhoWillAdd = "Tom";
        string articleContent = "What a good day today!";
        string articleTitle = "Good day";
        string id = "mock_id";
        Article article = new Article(userNameWhoWillAdd, articleTitle, articleContent);
        mock.Setup(service => service.GetById(id)).Returns(Task.FromResult(article));

        IArticleService articleService = mock.Object;
        Task<Article> articleGot = articleService.GetById(id);

        mock.Verify(service => service.GetById(id), Times.Once());
    }
}
