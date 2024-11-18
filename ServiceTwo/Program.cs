using DA;

namespace ServiceTwo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("DefaultConnection"));

            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

         


             var host = builder.Build();
            
            
            
            host.Run();
        }
    }
}