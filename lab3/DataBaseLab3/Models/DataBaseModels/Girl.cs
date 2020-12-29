using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseLab3.Models.DataBaseModels
{
    public partial class Girl
    {
        public Girl()
        {
            LinksGirlsAnimes = new HashSet<LinksGirlsAnime>();
        }

        public int GirlId { get; set; }
        public string Fullname { get; set; }
        public int Age { get; set; }
        public string Hair { get; set; }
        public string Eyes { get; set; }

        public override string ToString()
        {
            return $"Id: {GirlId}, Fullname: {Fullname}, Age: {Age}, Hair color: {Hair}, Eye color: {Eyes}";
        }

        public virtual ICollection<LinksGirlsAnime> LinksGirlsAnimes { get; set; }
    }
}
