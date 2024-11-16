using DA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Interfaces
{
    public interface IServerRepository
    {
        Task <Server> Add(Server entity);
        Task<Server> Update(Server entity);
        Task<Server> Delete(Server entity);
        Task<List<Server>> GetAll();
        Task<Server> Get(int id,bool track);
        Task<Server> Get(string name);
        Task Stop(int id);
        Task Start(int id);
        Task ChangeDuration(int id,int duration);

    }
}
