using System;
using System.Threading.Tasks;
using MeuTodo.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            if (env.IsDevelopment()) await context.Database.MigrateAsync();
        }

    }

}
