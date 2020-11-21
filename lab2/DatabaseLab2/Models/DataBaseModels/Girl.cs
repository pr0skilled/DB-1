using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.DataBaseModels
{
    class Girl
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public int Age { get; set; }
        public string Hair { get; set; }
        public string Eyes { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Fullname: {Fullname}, Age: {Age}, Hair color: {Hair}, Eye color: {Eyes}";
        }
    }
}
