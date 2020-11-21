using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.DataBaseModels
{
    class Producer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Studio { get; set; }
        public int NumberOfWorks { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Studio: {Studio}, Number of works: {NumberOfWorks}";
        }
    }
}
