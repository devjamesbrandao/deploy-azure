using System;
using System.Threading.Tasks;
using MeuTodo.Data;
using MeuTodo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MeuTodo.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(IApplicationBuilder serviceScope)
        {
            var services = serviceScope.ApplicationServices.CreateScope().ServiceProvider;

            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<AppDataContext>();

            await DbHealthChecker.TestConnection(context);

            await context.Database.EnsureDeletedAsync();

            await context.Database.EnsureCreatedAsync();

            await EnsureSeedUsers(context);
        }

        private static async Task EnsureSeedUsers(AppDataContext context)
        {
            if(await context.Users.AnyAsync()) return;

            await context.Users.AddAsync(new User(){ Id = 1, Name = "Naruto", Password = "Lamem"});

            await context.Users.AddAsync(new User() { Id = 2, Name = "Sakura ", Password = "Sasuke"});

            await context.SaveChangesAsync();
        }

    }

}
