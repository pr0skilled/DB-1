using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.DataBaseModels
{
    class Anime
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int Series { get; set; }
        public double Rating { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Year: {Year}, Series: {Series}, Rating: {Rating}";
        }
    }
}
