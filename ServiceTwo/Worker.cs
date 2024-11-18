using DA.Interfaces;
using DA.ServiceManager;
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

        private CancellationTokenSource _delayCancellationTokenSource = new();
        public Worker(ILogger<Worker> logger, ISeveiceManager seveiceManager)
        {
            _logger = logger;
            _seveiceManager = seveiceManager;
            _connection = new HubConnectionBuilder()
          .WithUrl("https://localhost:7150/myHub") // Use the actual API URL
          .WithAutomaticReconnect()
          .Build();

        }


        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            // Quick initialization
            
            await InitService(cancellationToken);
            await base.StartAsync(cancellationToken);
            _logger.LogInformation("Worker starting...");
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, _delayCancellationTokenSource.Token);
                    CancellationToken token = linkedTokenSource.Token;
                    //redice
                    await _seveiceManager.Refresh();
                    if (_isRunning)
                    {
                        _logger.LogInformation("server two OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOON");
                        await _seveiceManager.IsOnline();
                        await Task.Delay(_delay, token);
                    }
                    else
                    {
                        await Task.Delay(_delay, token); // Idle delay
                        _logger.LogInformation("server two OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOF");
                    }
                }
                catch (TaskCanceledException)
                {
                    // Reset the cancellation token if it was canceled for an interval update
                    if (_delayCancellationTokenSource.IsCancellationRequested)
                    {
                        _logger.LogInformation("Delay canceled due to interval update.");
                        _delayCancellationTokenSource.Dispose();
                        _delayCancellationTokenSource = new CancellationTokenSource();
                    }
                    else
                    {
                        _logger.LogInformation("Worker stopping.");
                        break;
                    }
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
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            await base.StopAsync(stoppingToken);
        }

        private async Task InitService(CancellationToken stoppingToken)
        {
            await _seveiceManager.Init("service two");
            _workerId = _seveiceManager.Server.Id.ToString();
            _isRunning = _seveiceManager.Server.IsRunning;
            _delay = _seveiceManager.Server.DurationInSecond;

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
                        _delayCancellationTokenSource.Cancel();
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

