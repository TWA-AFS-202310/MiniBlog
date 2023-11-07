using System.Collections.Generic;
using System.Threading.Tasks;
using MiniBlog.Model;
using MiniBlog.Repositories;
using MiniBlog.Services;
using MiniBlog.Stores;
using Moq;
using Xunit;

namespace MiniBlogTest.ServiceTest;

public class ArticleServiceTest
{
    [Fact]
    public async void Should_create_new_article_and_new_user_given_the_user_of_the_article_doesn_t_exist()
    {
        // given
        var newArticle = new Article("Jerry", "Let's code", "c#");
        var articleStore = new ArticleStore();
        var articleCountBeforeAddNewOne = articleStore.Articles.Count;
        var userStore = new UserStore(new List<User>());
        var userCountBeforeAddNewArticle = userStore.Users.Count;
        var articleService = new ArticleService(articleStore, userStore);

        // when
        var addedArticle = await articleService.CreateArticle(newArticle);

        // then
        Assert.Equal(articleCountBeforeAddNewOne + 1, articleStore.Articles.Count);
        Assert.Equal(userCountBeforeAddNewArticle + 1, userStore.Users.Count);
        Assert.Equal(newArticle.Title, addedArticle.Title);
        Assert.Equal(newArticle.Content, addedArticle.Content);
        Assert.Equal(newArticle.UserName, addedArticle.UserName);
    }

    [Fact]
    public async void Should_create_new_article_only_given_the_user_of_the_article_exists()
    {
        // given
        var userName = "Jerry";
        var newArticle = new Article(userName, "Let's code", "c#");
        var articleStore = new ArticleStore();
        var articleCountBeforeAddNewOne = articleStore.Articles.Count;
        var userStore = new UserStore(new List<User> { new User(userName) });
        var userCountBeforeAddNewArticle = userStore.Users.Count;
        var articleService = new ArticleService(articleStore, userStore);

        // when
        var addedArticle = await articleService.CreateArticle(newArticle);

        // then
        Assert.Equal(articleCountBeforeAddNewOne + 1, articleStore.Articles.Count);
        Assert.Equal(userCountBeforeAddNewArticle, userStore.Users.Count);
        Assert.Equal(newArticle.Title, addedArticle.Title);
        Assert.Equal(newArticle.Content, addedArticle.Content);
        Assert.Equal(newArticle.UserName, addedArticle.UserName);
    }
}
