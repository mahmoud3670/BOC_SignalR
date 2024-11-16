using Microsoft.AspNetCore.SignalR;

namespace WebApplicationAPI
{
    public class MyHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task ControlWorker(string workerId, string command, int? delay = null)
        {
            await Clients.All.SendAsync("ReceiveWorkerControl", workerId, command, delay);
        }
       
    }
}
