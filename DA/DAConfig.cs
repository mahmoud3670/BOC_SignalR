using DA.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DA.Interfaces;
using DA.Repository;
using DA.ServiceManager;

namespace DA
{
    public static class DAConfig
    {
        public static void AddPersistenceServices(this IServiceCollection services, string connectionString)
        {

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString),ServiceLifetime.Singleton);


            services.AddSingleton<IServerRepository, ServerRepository>();
            services.AddSingleton<ISeveiceManager, SeveiceManager>();



        }
    }
}
