## Practice Requirement
- Fork this repository
- Make all test cases pass

#### Environment Requirement
- .Net Core 7.0
- Visual Studio

#### Reference
* https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0

#### Tips:
Custom TestServer:
```c#
        protected HttpClient GetClient(ArticleRepository articleRepository, UserRepository userRepository)
        {
            return Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(
                    services =>
                    {
                        services.AddSingleton<ArticleRepository>(provider =>
                        {
                            return articleRepository;
                        });
                        services.AddSingleton<UserRepository>(provider =>
                        {
                            return userRepository;
                        });
                    });
            }).CreateDefaultClient();
        }
```
