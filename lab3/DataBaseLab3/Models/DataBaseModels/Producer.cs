using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseLab3.Models.DataBaseModels
{
    public partial class Producer
    {
        public Producer()
        {
            LinksAnimeProducers = new HashSet<LinksAnimeProducer>();
        }

        public int ProducerId { get; set; }
        public string Name { get; set; }
        public string Studio { get; set; }
        public int NumberOfWorks { get; set; }

        public override string ToString()
        {
            return $"Id: {ProducerId}, Name: {Name}, Studio: {Studio}, Number of works: {NumberOfWorks}";
        }

        public virtual ICollection<LinksAnimeProducer> LinksAnimeProducers { get; set; }
    }
}
