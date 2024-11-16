using DA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Interfaces
{
    public interface ISeveiceManager
    {
        public Server Server { get; }
        Task Init(string name);
        Task Update();
        Task Refresh();
    }
}
