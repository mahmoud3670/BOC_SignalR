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
                    DurationInSecond = 3000,
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

        public async Task IsOnline(bool isOnline= true) { 
            server.IsOnline = isOnline;
            server.LastRun = DateTime.Now;
            await _repository.Update(server);
        }
       

        public async Task Refresh()
        {
           
            server = await _repository.Get(server.Id, false);
            
        }
        
    }
}
