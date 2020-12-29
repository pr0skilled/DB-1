using DatabaseLab3.Models.DataBaseModels;
using DatabaseLab3.Models.Repositories;
using DatabaseLab3.Views;
using System;
using System.Collections.Generic;
using QuickChart;
using System.Drawing;
using System.Text.Json;
using NpgsqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DatabaseLab3.Controllers
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
                {
                    "MRA",
                    x =>
                        {
                            string s = "\n";
                            Tuple<List<string>, List<double>> t = AnimeRepo.MostRatedAnime();
                            Chart qc = new Chart();

                            qc.Width = 500;
                            qc.Height = 300;
                            qc.Config = $@"{{
                              type: 'bar',
                              data: {{
                                labels: {JsonSerializer.Serialize(t.Item1)},
                                datasets: [
                                  {{
                                    label: 'Most rated anime',
                                    data: {JsonSerializer.Serialize(t.Item2)},
                                    backgroundColor: 'rgba(54, 162, 235, 0.5)',
                                    borderColor: 'rgb(54, 162, 235)',
                                    borderWidth: 1,
                                  }},
                                ],
                              }},
                              options: {{
                                plugins: {{
                                  datalabels: {{
                                    anchor: 'center',
                                    align: 'center',
                                    color: '#fff',
                                    font: {{
                                      weight: 'bold',
                                    }},
                                  }},
                                }},
                              }},
                            }}";

                            var processes = System.Diagnostics.Process.GetProcessesByName("Firefox");
                            var path  = processes.FirstOrDefault()?.MainModule?.FileName;
                            System.Diagnostics.Process.Start(path, qc.GetUrl());
                            return s;
                       }
                },
                {
                    "RC",
                    x =>
                        {
                            string s = "\n";
                            Tuple<List<decimal>, List<string>> t = AnimeRepo.RatingChanges(x.AnimeId);
                            Chart qc = new Chart();

                            qc.Width = 500;
                            qc.Height = 3000;
                            qc.Config = $@"{{
                              type: 'line',
                              data: {{
                                labels: {JsonSerializer.Serialize(t.Item2)},
                                datasets: [
                                  {{
                                    label: 'Rating changes',
                                    fill: false,
                                    data: {JsonSerializer.Serialize(t.Item1)},
                                    backgroundColor: 'rgb(54, 162, 235)',
                                    borderColor: 'rgb(54, 162, 235)',
                                    borderWidth: 1,
                                  }},
                                ],
                              }},
                            }}";

                            var processes = System.Diagnostics.Process.GetProcessesByName("Firefox");
                            var path  = processes.FirstOrDefault()?.MainModule?.FileName;
                            System.Diagnostics.Process.Start(path, qc.GetUrl());
                            return s;
                       }
                },
                {
                    "BS",
                    x =>
                        {
                            string s = "\n";
                            Tuple<List<string>, List<int>> t = ProducersRepo.BestStudios();
                            Chart qc = new Chart();

                            qc.Width = 500;
                            qc.Height = 300;
                            qc.Config = $@"{{
                              type: 'pie',
                              data: {{
                                datasets: [
                                  {{
                                    data: {JsonSerializer.Serialize(t.Item2)},
                                    backgroundColor: [
                                      'rgb(255, 99, 132)',
                                      'rgb(255, 159, 64)',
                                      'rgb(255, 205, 86)',
                                      'rgb(75, 192, 192)',
                                      'rgb(54, 162, 235)',
                                    ],
                                    label: 'Studios with most titles per year',
                                  }},
                                ],
                                labels: {JsonSerializer.Serialize(t.Item1)},
                              }},
                            }}";

                            var processes = System.Diagnostics.Process.GetProcessesByName("Firefox");
                            var path  = processes.FirstOrDefault()?.MainModule?.FileName;
                            System.Diagnostics.Process.Start(path, qc.GetUrl());
                            return s;
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
