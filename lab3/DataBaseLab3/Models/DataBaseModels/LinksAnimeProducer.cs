using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseLab3.Models.DataBaseModels
{
    public partial class LinksAnimeProducer
    {
        public int Id { get; set; }
        public int AnimeId { get; set; }
        public int ProducerId { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual Producer Producer { get; set; }
    }
}
