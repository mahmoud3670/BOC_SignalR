using DA;

namespace ServiceOne
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddLogging(config =>
            {
                config.AddConsole();
            });
            var host = builder.Build();
            
            host.Run();
        }
    }
}