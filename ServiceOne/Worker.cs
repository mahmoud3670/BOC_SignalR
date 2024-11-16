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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _seveiceManager.Init("service one");
            
            while (!stoppingToken.IsCancellationRequested)
            {

                if (_seveiceManager.Server.IsRunning)
                {
                   

                        await _seveiceManager.Refresh();
                        await _seveiceManager.Update();
                    _logger.LogInformation("server one OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOON");
                }
                else
                {
                    await _seveiceManager.Refresh();
                    _logger.LogInformation("server one OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOF");

                }

                await Task.Delay(TimeSpan.FromSeconds(_seveiceManager.Server.DurationInSecond), stoppingToken);
            }
        }
    }
}
