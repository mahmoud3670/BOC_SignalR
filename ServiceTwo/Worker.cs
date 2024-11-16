using DA.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace ServiceTwo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISeveiceManager _seveiceManager;

        
        private readonly HubConnection _connection;
        private string _workerId;
        private bool _isRunning;
        private int _delay = 1000;
        public Worker(ILogger<Worker> logger, ISeveiceManager seveiceManager)
        {
            _logger = logger;
            _seveiceManager = seveiceManager;
            _connection = new HubConnectionBuilder()
          .WithUrl("https://localhost:7150/myHub") // Use the actual API URL
          .WithAutomaticReconnect()
          .Build();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           await InitService(stoppingToken);


            while (!stoppingToken.IsCancellationRequested)
            {
                if (_isRunning)
                {
                    _logger.LogInformation($"Worker {_workerId} is working...");
                    await Task.Delay(_delay, stoppingToken);
                }
                else
                {
                    await Task.Delay(1000, stoppingToken); // Idle delay
                    _logger.LogInformation("server two OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOF");
                }
                //if (_seveiceManager.Server.IsRunning)
                //{


                //    await _seveiceManager.Refresh();
                //    await _seveiceManager.Update();
                //    _logger.LogInformation("server two OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOON");
                //}
                //else
                //{
                //    await _seveiceManager.Refresh();
                //    _logger.LogInformation("server two OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOF");

                //}

                //await Task.Delay(TimeSpan.FromSeconds(_seveiceManager.Server.DurationInSecond), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            await base.StopAsync(stoppingToken);
        }

        private async Task InitService(CancellationToken stoppingToken)
        {
            await _seveiceManager.Init("service two");
            _workerId = _seveiceManager.Server.Id.ToString();

            _connection.On<string, string, int?>("ReceiveWorkerControl", (workerId, command, delay) =>
            {
                if (workerId == _workerId)
                {
                    HandleCommand(command, delay);
                }
            });

            _connection.On<string>("ReceiveMessage", message =>
            {
                _logger.LogInformation($"Received message: {message}");
            });

            await _connection.StartAsync(stoppingToken);
            _logger.LogInformation("Connected to SignalR Hub.");
        }


        private void HandleCommand(string command, int? delay)
        {
            switch (command.ToLower())
            {
                case "start":
                    _isRunning = true;
                    _logger.LogInformation($"Worker {_workerId} started.");
                    break;

                case "stop":
                    _isRunning = false;
                    _logger.LogInformation($"Worker {_workerId} stopped.");
                    break;

                case "setdelay":
                    if (delay.HasValue)
                    {
                        _delay = delay.Value;
                        _logger.LogInformation($"Worker {_workerId} delay set to {_delay} ms.");
                    }
                    break;

                default:
                    _logger.LogInformation($"Unknown command for worker {_workerId}: {command}");
                    break;
            }
        }


    }
}

