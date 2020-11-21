using DatabaseLab2.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Controllers
{
    class MainViewArgs
    {
        public int AnimeId { get; set; }
        public int GirlId { get; set; }
        public int ProducerId { get; set; }

        public Anime Anime { get; set; }
        public Girl Girl { get; set; }
        public Producer Producer { get; set; }

        public int RandomCount { get; set; }

        public SearchOneParameters SearchOneParameters { get; set; }
        public SearchTwoParameters SearchTwoParameters { get; set; }
        public SearchThreeParameters SearchThreeParameters { get; set; }
    }
}
