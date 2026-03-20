using Identity.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Common.Interfaces;
using Shared.Infrastructure.Persistence;
using Shared.Infrastructure.Repositories.Identities;
using Shared.Infrastructure.Services;

namespace Shared.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in configuration.");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly("Shared.Infrastructure");
                        sqlServerOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);
                    }));

            // Register CurrentUserService
            services.Scan(scan => scan
                .FromAssemblies(
                    typeof(ICurrentUserService).Assembly,  // Application
                    typeof(CurrentUserService).Assembly)   // Infrastructure
                .AddClasses(classes => classes.Where(type =>
                    type.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // Register Repositories & QueryServices
            services.Scan(scan => scan
                .FromAssemblies(
                    typeof(IUserRepository).Assembly,  // Identity.Domain
                    typeof(Workflow.Domain.Repositories.IWorkflowCategoryRepository).Assembly, // Workflow.Domain
                    typeof(UserRepository).Assembly)   // Shared.Infrastructure
                .AddClasses(classes => classes.Where(type =>
                    type.Name.EndsWith("Repository") ||
                    type.Name.EndsWith("QueryService")))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}