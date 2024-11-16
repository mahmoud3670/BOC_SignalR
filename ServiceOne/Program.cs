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

            var host = builder.Build();
            host.Run();
        }
    }
}