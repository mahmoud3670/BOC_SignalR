using DA.Entity;
using DA.Interfaces;

namespace DA.ServiceManager
{
    public class SeveiceManager: ISeveiceManager
    {
        private Server server;
        private IServerRepository _repository;
        public SeveiceManager(IServerRepository serverRepository)
        {
            server=new Server();
            _repository=serverRepository;
        }
        public Server Server { get { return server; } }
        public async Task Init(string name)
        {
            var serverExist =await _repository.Get(name);
            if (serverExist == null) {
                server = new Server()
                {
                    DurationInSecond = 3,
                    IsOnline = true,
                    IsRunning=true,
                    LastRun = DateTime.Now,
                    Name = name,
                };
               await _repository.Add(server);
            }
            else
            {
                server = serverExist;
                server.IsOnline = true;
                await _repository.Update(server);
            }
           
            
        }

        public async Task Update() { 
            server.IsOnline = true;
            server.LastRun = DateTime.Now;
            await _repository.Update(server);
        }

        public async Task Refresh()
        {
            var x= await _repository.Get(server.Id, false);
            server = await _repository.Get(server.Id, false);
            
        }
    }
}
