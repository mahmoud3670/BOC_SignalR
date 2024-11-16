using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Entity
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public bool IsRunning { get; set; }
        public DateTime LastRun { get; set; }
        public int DurationInSecond { get; set; }
    }
}
