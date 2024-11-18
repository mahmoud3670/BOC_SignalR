using DA.Interfaces;

namespace ServiceOne
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISeveiceManager _seveiceManager;
        public Worker(ILogger<Worker> logger, ISeveiceManager seveiceManager)
        {
            _logger = logger;
            _seveiceManager = seveiceManager;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            // Quick initialization
          
            await _seveiceManager.Init("service one");
            await base.StartAsync(cancellationToken);
            _logger.LogInformation("Worker starting...");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _seveiceManager.Refresh();

                    if (_seveiceManager.Server.IsRunning)
                    {
                        _logger.LogInformation("------------server one OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOON-------------");
                        await _seveiceManager.IsOnline();

                    }
                    else
                    {
                        _logger.LogInformation("-------------server one OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOF------------------");
                    }

                    await Task.Delay(_seveiceManager.Server.DurationInSecond, stoppingToken);
                }
                catch (Exception ex)
                {

                    throw;
                }
               
            }
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {

            await _seveiceManager.IsOnline(false);
          
            await base.StopAsync(stoppingToken);
        }
    }
}
