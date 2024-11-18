using DA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace WebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IServerRepository _repository;
        private readonly IHubContext<MyHub> _hubContext;
        public ServerController(IServerRepository repository, IHubContext<MyHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            return Ok(await _repository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _repository.Get(id,false));
        }
        [HttpGet("start/{id}")]
        public async Task<IActionResult> Start(int id)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveWorkerControl", id.ToString(), "start",0);
            await _repository.Start(id);
            return Ok();
        }

        [HttpGet("Stop/{id}")]
        public async Task<IActionResult> Stop(int id)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveWorkerControl", id.ToString(), "stop",0);
            await _repository.Stop(id);
            return Ok();
        }

        [HttpGet("ChangeDuration/{id}/{durationInSecond}")]
        public async Task<IActionResult> ChangeDuration(int id,int durationInSecond)
        {
            durationInSecond = durationInSecond * 1000;
            await _hubContext.Clients.All.SendAsync("ReceiveWorkerControl", id.ToString(), "setdelay", durationInSecond);
            await _repository.ChangeDuration(id, durationInSecond);
            return Ok();
        }







    }
}
