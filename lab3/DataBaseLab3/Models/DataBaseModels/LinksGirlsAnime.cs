using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseLab3.Models.DataBaseModels
{
    public partial class LinksGirlsAnime
    {
        public int Id { get; set; }
        public int GirlId { get; set; }
        public int AnimeId { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual Girl Girl { get; set; }
    }
}
