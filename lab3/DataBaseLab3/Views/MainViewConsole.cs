using DatabaseLab3.Controllers;
using DatabaseLab3.Models.DataBaseModels;
using System.Globalization;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab3.Views
{
    class MainViewConsole
    {
        MainViewController mainController;

        public MainViewConsole(MainViewController controller)
        {
            mainController = controller;
            viewCommands = new Dictionary<string, Func<MainViewArgs>>
            {
                #region animeCRUD
                {
                    "get_all_anime",
                    () => new MainViewArgs()
                },
                {
                    "get_anime",
                    () => new MainViewArgs{AnimeId = GetId()}
                },
                {
                    "delete_anime",
                    () => new MainViewArgs{AnimeId = GetId()}
                },
                {
                    "insert_anime",//
                    () =>
                    {
                        Anime anime = GetAnimeValues(null);
                        return new MainViewArgs{ Anime = anime};
                    }
                },
                {
                    "update_anime",
                    () =>
                    {
                        Anime anime = mainController.GetAnime(GetId());
                        if(anime == null) throw new Exception();
                        Anime anime1 = GetAnimeValues(anime);
                        return new MainViewArgs{Anime = anime1};
                    }
                },
                #endregion

                #region AnimeGirls
                {
                    "AddGirlToAnime",
                    () =>
                    {
                        return new MainViewArgs{AnimeId = GetId("anime"), GirlId = GetId("girl")};
                    }
                },
                {
                    "DeleteGirlFromAnime",
                    () =>
                    {
                        return new MainViewArgs{AnimeId = GetId("anime"), GirlId = GetId("girl")};
                    }
                },
                {
                    "GetGirlsOfAnime",
                    () => new MainViewArgs{ AnimeId = GetId("anime")}
                },
                #endregion

                #region AnimeProducers
                {
                    "AddProducerToAnime",
                    () =>
                    {
                        return new MainViewArgs{AnimeId = GetId("anime"), ProducerId = GetId("producer")};
                    }
                },
                {
                    "DeleteProducerFromAnime",
                    () =>
                    {
                        return new MainViewArgs{AnimeId = GetId("anime"), ProducerId = GetId("producer")};
                    }
                },
                {
                    "GetProducersOfAnime",
                    () => new MainViewArgs{ AnimeId = GetId("anime")}
                },
                #endregion

                #region GirlsCRUD
                {
                    "get_girls",
                    () => new MainViewArgs()
                },
                {
                    "get_girl",
                    () => new MainViewArgs{GirlId = GetId()}
                },
                {
                    "delete_girl",
                    () => new MainViewArgs{GirlId = GetId()}
                },
                {
                    "insert_girl",
                    () =>
                    {
                        Girl girl = GetGirlValues(null);
                        return new MainViewArgs{ Girl = girl};
                    }
                },
                {
                    "update_girl",
                    () =>
                    {
                        Girl girl = mainController.GetGirl(GetId());
                        if(girl == null) throw new Exception();
                        Girl girl1 = GetGirlValues(girl);
                        return new MainViewArgs{Girl = girl1};
                    }
                },
                {
                    "get_girls_anime",
                    () => new MainViewArgs{ GirlId = GetId()}
                },
                #endregion

                #region ProducersCRUD
                {
                    "get_producers",
                    () => new MainViewArgs()
                },
                {
                    "get_producer",
                    () => new MainViewArgs{ProducerId = GetId()}
                },
                {
                    "delete_producer",
                    () => new MainViewArgs{ProducerId = GetId()}
                },
                {
                    "insert_producer",
                    () =>
                    {
                        Producer producer = GetProducerValues(null);
                        return new MainViewArgs{ Producer = producer};
                    }
                },
                {
                    "update_producer",
                    () =>
                    {
                        Producer producer = mainController.GetProducer(GetId());
                        if(producer == null) throw new Exception();
                        Producer producer1 = GetProducerValues(producer);
                        return new MainViewArgs{Producer = producer1};
                    }
                },
                {
                    "get_producers_anime",
                    () => new MainViewArgs{ ProducerId = GetId()}
                },
                #endregion
                {
                    "add_random_anime",
                    () => new MainViewArgs{ RandomCount = GetCount()}
                },
                {
                    "add_random_girls",
                    () => new MainViewArgs{ RandomCount = GetCount()}
                },
                {
                    "add_random_producers",
                    () => new MainViewArgs{ RandomCount = GetCount()}
                },
                {
                    "add_random_links_ga",
                    () => new MainViewArgs{ RandomCount = GetCount()}
                },
                {
                    "add_random_links_ap",
                    () => new MainViewArgs{ RandomCount = GetCount()}
                },
                {
                    "search1",
                    () => new MainViewArgs
                    {
                        SearchOneParameters = new SearchOneParameters
                        {
                            MinSeries = GetInt("Enter minimum series in title:"),
                            MinWorks = GetInt("Enter minimum producer's works:"),
                            Name = GetStr("Enter girl's name:")
                        }
                    }
                },
                {
                    "search2",
                    () => new MainViewArgs
                    {
                        SearchTwoParameters = new SearchTwoParameters
                        {
                             MinRating = GetDouble("Enter minimal title's rating(0.00 - 10.00):"),
                             MinAge = GetInt("Enter minimum girl's age:"),
                             MaxAge = GetInt("Enter maximum girl's age:"),
                        }
                    }
                },
                {
                    "search3",
                    () => new MainViewArgs
                    {
                        SearchThreeParameters = new SearchThreeParameters
                        {
                             Name = GetStr("Enter producer's name:"),
                             MinYear = GetInt("Enter minimum anime year:"),
                             MaxYear = GetInt("Enter maximum anime year:"),
                        }
                    }
                },
                {
                    "MRA",
                    () => new MainViewArgs()
                },
                {
                    "RC",
                    () => new MainViewArgs{AnimeId = GetId()}
                },
                {
                    "BS",
                    () => new MainViewArgs()
                },
            };
        }

        private string GetStr(string msg)
        {
            Console.WriteLine(msg);
            return Console.ReadLine();
        }

        private int GetInt(string msg)
        {
            Console.WriteLine(msg);
            return Convert.ToInt32(Console.ReadLine());
        }

        private double GetDouble(string msg)
        {
            Console.WriteLine(msg);
            return double.Parse(Console.ReadLine(), new CultureInfo("en-US").NumberFormat);
        }

        private int GetId(string message = "")
        {
            Console.Write($"Enter {message} id:");
            return Convert.ToInt32(Console.ReadLine());
        }

        private int GetCount()
        {
            Console.Write($"Enter number of entities:");
            return Convert.ToInt32(Console.ReadLine());
        }

        private Producer GetProducerValues(Producer producer)
        {
            Producer res = new Producer();
            if (producer != null)
            {
                Console.WriteLine("Current producer");
                Console.WriteLine(producer);
                res.ProducerId = producer.ProducerId;
            }

            Console.WriteLine("Enter name:");
            res.Name = Console.ReadLine();

            Console.WriteLine("Enter studio:");
            res.Studio = Console.ReadLine();

            Console.WriteLine("Enter number of works:");
            res.NumberOfWorks = Convert.ToInt32(Console.ReadLine());

            return res;
        }

        private Anime GetAnimeValues(Anime anime)
        {
            Anime res = new Anime();
            if(anime != null)
            {
                Console.WriteLine("Current anime");
                Console.WriteLine(anime);
                res.AnimeId = anime.AnimeId;
            }

            Console.WriteLine("Enter title:");
            res.Title = Console.ReadLine();

            Console.WriteLine("Enter release year:");
            res.Year = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter series:");
            res.Series = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter rating:");
            string str = Console.ReadLine();
            decimal d = decimal.Parse(str, NumberStyles.AllowDecimalPoint); // ?????????????????????????????
            res.Rating = d;

            return res;
        }

        private Girl GetGirlValues(Girl girl)
        {
            Girl res = new Girl();
            if (girl != null)
            {
                Console.WriteLine("Current Girl");
                Console.WriteLine(girl);
                res.GirlId = girl.GirlId;
            }

            Console.WriteLine("Enter name:");
            res.Fullname = Console.ReadLine();

            Console.WriteLine("Enter age:");
            res.Age = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter hair color:");
            res.Hair = Console.ReadLine();

            Console.WriteLine("Enter eye color:");
            res.Eyes = Console.ReadLine();

            return res;
        }

        private void WriteCommands()
        {
            Console.WriteLine("Commands list:");
            foreach(var str in mainController.CommandsHandler.Keys.ToList())
            {
                Console.WriteLine(str);
            }
        }
        private Dictionary<string, Func<MainViewArgs>> viewCommands;
        public void Run()
        {
            do
            {
                WriteCommands();
                Console.WriteLine("\n");
                Console.WriteLine("Enter command:");
                var command = Console.ReadLine();
                try
                {
                    Console.WriteLine(mainController.CommandsHandler[command](viewCommands[command]()));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("bad value");
                }
                

                Console.WriteLine("\nPress any key");
                Console.ReadKey();
                Console.WriteLine("\n");
            } while (true);
        }
    }
}
