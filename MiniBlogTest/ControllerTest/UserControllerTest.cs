using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MiniBlog;
using MiniBlog.Model;
using MiniBlog.Stores;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

namespace MiniBlogTest.ControllerTest
{
    [Collection("IntegrationTest")]
    public class UserControllerTest : TestBase
    {
        public UserControllerTest(CustomWebApplicationFactory<Startup> factory)
            : base(factory)

        {
        }

        [Fact]
        public async Task Should_get_all_users()
        {
            // given
            var articleStore = new ArticleStore();
            var userStore = new UserStore(new List<User>());
            var client = GetClient(articleStore, userStore, articleStore, userStore);

            // when
            var response = await client.GetAsync("/user");

            // then
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(body);
            Assert.True(users.Count == 0);
        }

        [Fact]
        public async Task Should_register_user_success()
        {
            // given
            var articleStore = new ArticleStore();
            var userStore = new UserStore(new List<User>());
            var client = GetClient(articleStore, userStore, articleStore, userStore);

            var userName = "Tom";
            var email = "a@b.com";
            var user = new User(userName, email);
            var userJson = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(userJson, Encoding.UTF8, MediaTypeNames.Application.Json);

            // when
            var registerResponse = await client.PostAsync("/user", content);

            // It fail, please help
            Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);

            var response = await client.GetAsync("/user");
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(body);
            Assert.True(users.Count == 1);
            Assert.Equal(email, users[0].Email);
            Assert.Equal(userName, users[0].Name);
        }

        [Fact]
        public async Task Should_register_user_fail_when_UserStore_unavailable()
        {
            var articleStore = new ArticleStore();
            var client = GetClient(articleStore, null, articleStore, null);

            var userName = "Tom";
            var email = "a@b.com";
            var user = new User(userName, email);
            var userJson = JsonConvert.SerializeObject(user);

            StringContent content = new StringContent(userJson, Encoding.UTF8, MediaTypeNames.Application.Json);
            var registerResponse = await client.PostAsync("/user", content);
            Assert.Equal(HttpStatusCode.InternalServerError, registerResponse.StatusCode);
        }

        [Fact]
        public async Task Should_update_user_email_success_()
        {
            var articleStore = new ArticleStore();
            var userStore = new UserStore(new List<User>());
            var client = GetClient(articleStore, userStore, articleStore, userStore);

            var userName = "Tom";
            var originalEmail = "a@b.com";
            var updatedEmail = "tom@b.com";
            var originalUser = new User(userName, originalEmail);

            var newUser = new User(userName, updatedEmail);
            StringContent registerUserContent = new StringContent(JsonConvert.SerializeObject(originalUser), Encoding.UTF8, MediaTypeNames.Application.Json);
            var registerResponse = await client.PostAsync("/user", registerUserContent);

            StringContent updateUserContent = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PutAsync("/user", updateUserContent);

            var response = await client.GetAsync("/user");
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(body);
            Assert.Equal(1, users.Count);
            Assert.Equal(updatedEmail, users[0].Email);
            Assert.Equal(userName, users[0].Name);
        }

        [Fact]
        public async Task Should_delete_user_and_related_article_success()
        {
            // given
            var userName = "Tom";
            var articleStore = new ArticleStore(
                    new List<Article>
                    {
                        new Article(userName, string.Empty, string.Empty),
                        new Article(userName, string.Empty, string.Empty),
                    });
            var userStore = new UserStore(
                    new List<User>
                    {
                        new User(userName, string.Empty),
                    });
            var client = GetClient(articleStore, userStore, articleStore, userStore);

            var articlesResponse = await client.GetAsync("/article");

            articlesResponse.EnsureSuccessStatusCode();
            var articles = JsonConvert.DeserializeObject<List<Article>>(
                await articlesResponse.Content.ReadAsStringAsync());
            Assert.Equal(2, articles.Count);

            var userResponse = await client.GetAsync("/user");
            userResponse.EnsureSuccessStatusCode();
            var users = JsonConvert.DeserializeObject<List<User>>(
                await userResponse.Content.ReadAsStringAsync());
            Assert.True(users.Count == 1);

            // when
            await client.DeleteAsync($"/user?name={userName}");

            // then
            var articlesResponseAfterDeletion = await client.GetAsync("/article");
            articlesResponseAfterDeletion.EnsureSuccessStatusCode();
            var articlesLeft = JsonConvert.DeserializeObject<List<Article>>(
                await articlesResponseAfterDeletion.Content.ReadAsStringAsync());
            Assert.True(articlesLeft.Count == 0);

            var userResponseAfterDeletion = await client.GetAsync("/user");
            userResponseAfterDeletion.EnsureSuccessStatusCode();
            var usersLeft = JsonConvert.DeserializeObject<List<User>>(
                await userResponseAfterDeletion.Content.ReadAsStringAsync());
            Assert.True(usersLeft.Count == 0);
        }
    }
}
