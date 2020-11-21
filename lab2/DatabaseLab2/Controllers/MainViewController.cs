using DatabaseLab2.Configuration;
using DatabaseLab2.Models.DataBaseModels;
using DatabaseLab2.Models.Repositories;
using DatabaseLab2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Controllers
{
    class MainViewController
    {
        public Dictionary<string, Func<MainViewArgs, object>> CommandsHandler { get; private set; }
        private GirlsRepository GirlsRepo;
        private ProducersRepository ProducersRepo;
        private AnimeRepository AnimeRepo;
        

        public MainViewController(string connectionString)
        {
            AnimeRepo = new AnimeRepository(connectionString);
            ProducersRepo = new ProducersRepository(connectionString);
            GirlsRepo = new GirlsRepository(connectionString);


            CommandsHandler = new Dictionary<string, Func<MainViewArgs, object>>
            {
                #region AnimeCRUD
                {
                    "get_all_anime",
                    x =>
                        {
                            string res = "";
                            foreach(var anime in AnimeRepo.GetAllAnime())
                            {
                                res += (anime.ToString() + "\n");
                            }
                            return res;
                       }
                },
                {
                    "get_anime",
                    x =>
                    {
                        return AnimeRepo.GetAnime(x.AnimeId).ToString();
                    }
                },
                {
                    "delete_anime",
                    x =>
                    {
                        return GetBoolReturnMessage(AnimeRepo.DeleteAnime(x.AnimeId));
                    }
                },
                {
                    "insert_anime",
                    x =>
                    {
                        return GetBoolReturnMessage(AnimeRepo.InsertAnime(x.Anime));
                    }
                },
                {
                    "update_anime",
                    x =>
                    {
                        return GetBoolReturnMessage(AnimeRepo.UpdateAnime(x.Anime));
                    }

                },
                #endregion

                #region AnimeGirls
                {
                    "AddGirlToAnime",
                    x =>
                    {
                        return GetBoolReturnMessage(AnimeRepo.AddGirlToAnime(x.GirlId, x.AnimeId));
                    }
                },
                {
                    "DeleteGirlFromAnime",
                    x =>
                    {
                        return GetBoolReturnMessage(AnimeRepo.DeleteGirlFromAnime(x.GirlId, x.AnimeId));
                    }
                },
                {
                    "GetGirlsOfAnime",
                    x =>
                    {
                        string res = "";
                        foreach(var Girl in AnimeRepo.GetGirlsOfAnime(x.AnimeId))
                        {
                            res += (Girl.ToString() + "\n");
                        }
                        return res;
                    }
                },
                #endregion

                #region AnimeProducers
                {
                    "AddProducerToAnime",
                    x =>
                    {
                        return GetBoolReturnMessage(AnimeRepo.AddProducerToAnime(x.ProducerId, x.AnimeId));
                    }
                },
                {
                    "DeleteProducerFromAnime",
                    x =>
                    {
                        return GetBoolReturnMessage(AnimeRepo.DeleteProducerFromAnime(x.ProducerId, x.AnimeId));
                    }
                },
                {
                    "GetProducersOfAnime",
                    x =>
                    {
                        string res = "";
                        foreach(var Producer in AnimeRepo.GetProducersOfAnime(x.AnimeId))
                        {
                            res += (Producer.ToString() + "\n");
                        }
                        return res;
                    }
                },
                #endregion

                #region GirlsCRUD
                {
                    "get_girls",
                    x =>
                        {
                            string res = "";
                            foreach(var Girl in GirlsRepo.GetAllGirls())
                            {
                                res += (Girl.ToString() + "\n");
                            }
                            return res;
                       }
                },
                {
                    "get_girl",
                    x =>
                    {
                        return GirlsRepo.GetGirl(x.GirlId).ToString();
                    }
                },
                {
                    "delete_girl",
                    x =>
                    {
                        return GetBoolReturnMessage(GirlsRepo.DeleteGirl(x.GirlId));
                    }
                },
                {
                    "insert_girl",
                    x =>
                    {
                        return GetBoolReturnMessage(GirlsRepo.InsertGirl(x.Girl));
                    }
                },
                {
                    "update_girl",
                    x =>
                    {
                        return GetBoolReturnMessage(GirlsRepo.UpdateGirl(x.Girl));
                    }

                },
                {
                    "get_girls_anime",
                    x =>
                    {
                        string res = "";
                        foreach(var Anime in GirlsRepo.GetGirlsAnime(x.GirlId))
                        {
                            res += (Anime.ToString() + "\n");
                        }
                        return res;
                    }
                },
                #endregion

                #region ProducersCRUD
                {
                    "get_producers",
                    x =>
                        {
                            string res = "";
                            foreach(var Producer in ProducersRepo.GetAllProducers())
                            {
                                res += (Producer.ToString() + "\n");
                            }
                            return res;
                       }
                },
                {
                    "get_producer",
                    x =>
                    {
                        return ProducersRepo.GetProducer(x.ProducerId).ToString();
                    }
                },
                {
                    "delete_producer",
                    x =>
                    {
                        return GetBoolReturnMessage(ProducersRepo.DeleteProducer(x.ProducerId));
                    }
                },
                {
                    "insert_producer",
                    x =>
                    {
                        return GetBoolReturnMessage(ProducersRepo.InsertProducer(x.Producer));
                    }
                },
                {
                    "update_producer",
                    x =>
                    {
                        return GetBoolReturnMessage(ProducersRepo.UpdateProducer(x.Producer));
                    }

                },
                {
                    "get_producers_anime",
                    x =>
                    {
                        string res = "";
                        foreach(var Anime in ProducersRepo.GetProducersAnime(x.ProducerId))
                        {
                            res += (Anime.ToString() + "\n");
                        }
                        return res;
                    }
                },
                #endregion

                #region rand+Search
                {
                    "add_random_anime",
                    x =>
                    {
                        return GetBoolReturnMessage(AnimeRepo.AddRandomAnimeToDB(x.RandomCount));
                    }
                },
                {
                    "add_random_girls",
                    x =>
                    {
                        return GetBoolReturnMessage(GirlsRepo.AddRandomGirlsToDB(x.RandomCount));
                    }
                },
                {
                    "add_random_producers",
                    x =>
                    {
                        return GetBoolReturnMessage(ProducersRepo.AddRandomProducersToDB(x.RandomCount));
                    }
                },
                {
                    "add_random_links_ga",
                    x =>
                    {
                        return GetBoolReturnMessage(AnimeRepo.AddRandomLinksGirlsAnimeToDB(x.RandomCount));
                    }
                },
                {
                    "add_random_links_ap",
                    x =>
                    {
                        return GetBoolReturnMessage(AnimeRepo.AddRandomLinksAnimeProducersToDB(x.RandomCount));
                    }
                },
                {
                    "search1",
                    x =>
                    {
                        return AnimeRepo.SearchOne(x.SearchOneParameters.MinSeries, x.SearchOneParameters.MinWorks, x.SearchOneParameters.Name);
                        
                    }
                },
                {
                    "search2",
                    x =>
                        {
                            return AnimeRepo.SearchTwo(x.SearchTwoParameters.MinRating, x.SearchTwoParameters.MinAge, x.SearchTwoParameters.MaxAge);
                       }
                },
                {
                    "search3",
                    x =>
                        {
                            return ProducersRepo.SearchThree(x.SearchThreeParameters.Name, x.SearchThreeParameters.MinYear, x.SearchThreeParameters.MaxYear);
                       }
                },
                #endregion
            };
        }


        public Producer GetProducer(int id)
        {
            return ProducersRepo.GetProducer(id);
        }
        public Girl GetGirl(int id)
        {
            return GirlsRepo.GetGirl(id);
        }
        public Anime GetAnime(int id)
        {
            return AnimeRepo.GetAnime(id);
        }
        private string GetBoolReturnMessage(bool b)
        {
            if (b) return "success";
            else return "ERROR";
        }
    }
}
