using DA.Entity;
using DA.Interfaces;
using DA.Context;
using Microsoft.EntityFrameworkCore;
namespace DA.Repository
{
    public class ServerRepository : IServerRepository
    {
        private readonly AppDbContext _dbContext;
        public ServerRepository(AppDbContext dbContext)
        {
            _dbContext=dbContext;
        }
        public async Task<Server> Add(Server entity)
        {
           await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task ChangeDuration(int id,int duration)
        {
           var server=await Get(id,true);
            server.DurationInSecond = duration;
            await _dbContext.SaveChangesAsync();
        }

        public Task<Server> Delete(Server entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Server> Get(int id, bool track)
        {
            _dbContext.ChangeTracker.Clear();
            var server =  _dbContext.Servers;
            if (!track) 
                server.AsNoTracking();
            return await server.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Server?> Get(string name)
        {
            return await _dbContext.Servers.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
        }
        public async Task<List<Server>> GetAll()
        {
            return await _dbContext.Servers.AsNoTracking().ToListAsync();
        }

        public async Task Start(int id)
        {
            var service=await Get(id,true);
            service.IsRunning = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Stop(int id)
        {
            var service = await Get(id, true);
            service.IsRunning = false;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Server> Update(Server entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
