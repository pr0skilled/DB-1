using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseLab3.Models.DataBaseModels
{
    public partial class Anime
    {
        public Anime()
        {
            LinksAnimeProducers = new HashSet<LinksAnimeProducer>();
            LinksGirlsAnimes = new HashSet<LinksGirlsAnime>();
        }

        public int AnimeId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int Series { get; set; }
        public decimal Rating { get; set; }

        public override string ToString()
        {
            return $"Id: {AnimeId}, Title: {Title}, Year: {Year}, Series: {Series}, Rating: {Rating}";
        }

        public virtual ICollection<LinksAnimeProducer> LinksAnimeProducers { get; set; }
        public virtual ICollection<LinksGirlsAnime> LinksGirlsAnimes { get; set; }
    }
}
