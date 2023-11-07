using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MiniBlog;
using MiniBlog.Model;
using MiniBlog.Repositories;
using MiniBlog.Stores;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace MiniBlogTest.ControllerTest
{
    [Collection("IntegrationTest")]
    public class ArticleControllerTest : TestBase
    {
        public ArticleControllerTest(CustomWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async void Should_get_all_Article()
        {
            var mockedArticleRepository = new Mock<IArticleRepository>();
            mockedArticleRepository.Setup(repository => repository.GetAllArticles()).Returns(Task.FromResult(new List<Article>
            {
                new Article(null, "Happy new year", "Happy 2021 new year"),
                new Article(null, "Happy Halloween", "Halloween is coming"),
            }));
            var mockedUserRepository = new Mock<IUserRepository>();
            var client = GetClient(null, null, mockedArticleRepository.Object, mockedUserRepository.Object);
            var response = await client.GetAsync("/article");
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<Article>>(body);
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async void Should_create_article_fail_when_ArticleStore_unavailable()
        {
            var mockedUserRepository = new Mock<IUserRepository>();
            var client = GetClient(null, null, null, mockedUserRepository.Object);
            string userNameWhoWillAdd = "Tom";
            string articleContent = "What a good day today!";
            string articleTitle = "Good day";
            Article article = new Article(userNameWhoWillAdd, articleTitle, articleContent);

            var httpContent = JsonConvert.SerializeObject(article);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/article", content);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async void Should_create_article_and_register_user_correct()
        {
            string userNameWhoWillAdd = "Tom";
            User user = new User(userNameWhoWillAdd);
            string articleContent = "What a good day today!";
            string articleTitle = "Good day";
            Article article = new Article(userNameWhoWillAdd, articleTitle, articleContent)
            { Id = "123" };

            var mockedArticleRepository = new Mock<IArticleRepository>();
            mockedArticleRepository.Setup(repository => repository.CreateArticle(It.IsAny<Article>())).Returns(Task.CompletedTask);
            mockedArticleRepository.Setup(repository => repository.GetAllArticles()).Returns(Task.FromResult(new List<Article> { article }));

            var mockedUserRepository = new Mock<IUserRepository>();
            mockedUserRepository.Setup(repository => repository.GetUserByName(It.IsAny<string>())).Returns(Task.FromResult<User>(null));
            mockedUserRepository.Setup(repository => repository.CreateUser(It.IsAny<User>())).Returns(Task.CompletedTask);
            mockedUserRepository.Setup(repository => repository.GetAllUsers()).Returns(Task.FromResult(new List<User> { user }));
            var client = GetClient(new ArticleStore(), new UserStore(), mockedArticleRepository.Object, mockedUserRepository.Object);

            var httpContent = JsonConvert.SerializeObject(article);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var createArticleResponse = await client.PostAsync("/article", content);

            // It fail, please help
            Assert.Equal(HttpStatusCode.Created, createArticleResponse.StatusCode);

            var articleResponse = await client.GetAsync("/article");
            var body = await articleResponse.Content.ReadAsStringAsync();
            var articles = JsonConvert.DeserializeObject<List<Article>>(body);
            Assert.True(articles.Count == 1);
            Assert.Equal(articleTitle, articles[0].Title);
            Assert.Equal(articleContent, articles[0].Content);
            Assert.Equal(userNameWhoWillAdd, articles[0].UserName);

            var userResponse = await client.GetAsync("/user");
            var usersJson = await userResponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(usersJson);

            Assert.True(users.Count == 1);
            Assert.Equal(userNameWhoWillAdd, users[0].Name);
            Assert.Equal("anonymous@unknow.com", users[0].Email);
        }
    }
}
