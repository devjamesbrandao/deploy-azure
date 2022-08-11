
using MeuTodo;
using MeuTodo.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Factory
{
    public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Startup> where TEntryPoint : Startup
    {
        // Override in Startup.cs to configure InMemory DbContext to tests
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDataContext>));

                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<AppDataContext>(options => { options.UseInMemoryDatabase("InMemoryTodoTest"); });
            });
        }
    }
}