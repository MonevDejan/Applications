using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Todoitem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Iscomplete{ get; set; }
    }
}
