using DatabaseLab3.Infrastructure;
using DatabaseLab3.Models.DataBaseModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab3.Models.Repositories
{
    class AnimeRepository : BaseRepository
    {

        public AnimeRepository(string connectionString) : base(connectionString)
        {
            db = new DatabaseContext();
        }

        private DatabaseContext db;

        #region CRUD
        public IEnumerable<Anime> GetAllAnime()
        {
            List<Anime> anime = db.Animes.ToList();
            return anime;
        }

        public Anime GetAnime(int id)
        {
            Anime anime = db.Animes.Find(id);
            return anime;
        }
        public bool UpdateAnime(Anime anime) 
        {
            try
            {
                Anime oldAnime = db.Animes.SingleOrDefault(x => x.AnimeId == anime.AnimeId);
                if (oldAnime != null)
                {
                    oldAnime.Title = anime.Title;
                    oldAnime.Year = anime.Year;
                    oldAnime.Series = anime.Series;
                    oldAnime.Rating = anime.Rating;
                    db.Animes.Update(oldAnime);
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertAnime(Anime anime)
        {
            try
            {
                db.Animes.Add(anime);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteAnime(int id)
        {
            try
            {
                Anime anime = db.Animes.Find(id);
                db.Animes.Remove(anime);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region animeWithGirls
        public bool AddGirlToAnime(int girlId, int animeId)
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"insert into links_girls_anime (girl_id, anime_id) values({girlId}, {animeId})", con);

                if (command.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                con.Close();
                return res;
            }
            catch
            {
                con.Close();
                return false;
            }
        }

        public bool DeleteGirlFromAnime(int girlId, int animeId)
        {
            bool res = false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"delete from links_girls_anime where girl_id = {girlId} and anime_id = {animeId}", con);

            if (command.ExecuteNonQuery() != 0)
            {
                res = true;
            }

            con.Close();
            return res;
        }

        public IEnumerable<Girl> GetGirlsOfAnime(int animeId)
        {
            con.Open();
            List<Girl> girls = new List<Girl>();
            NpgsqlCommand command = new NpgsqlCommand($@"select * from girls where girl_id in (select girl_id from links_girls_anime where anime_id = {animeId})", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                girls.Add(ReadGirl(reader));
            }

            con.Close();

            return girls;
        }
        #endregion

        #region animeWithProducers
        public bool AddProducerToAnime(int producerId, int animeId)
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"insert into links_anime_producers (producer_id, anime_id) values({producerId}, {animeId})", con);

                if (command.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                con.Close();
                return res;
            }
            catch
            {
                con.Close();
                return false;
            }
        }

        public bool DeleteProducerFromAnime(int producerId, int animeId)
        {
            bool res = false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"delete from links_anime_producers where producer_id = {producerId} and anime_id = {animeId}", con);

            if (command.ExecuteNonQuery() != 0)
            {
                res = true;
            }

            con.Close();
            return res;
        }

        public IEnumerable<Producer> GetProducersOfAnime(int animeId)
        {
            con.Open();
            List<Producer> producers = new List<Producer>();
            NpgsqlCommand command = new NpgsqlCommand($@"select * from producers where producer_id in (select producer_id from links_anime_producers where anime_id = {animeId})", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                producers.Add(ReadProducer(reader));
            }

            con.Close();

            return producers;
        }
        #endregion

        #region search
        public string SearchOne(int minSeries, int minWorks, string name)
        {
            string res = "";

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($@"SELECT 
an.anime_id, an.title, an.series, prod.producer_id, prod.name, prod.number_of_works, gir.girl_id, gir.fullname
FROM anime AS an
    LEFT JOIN links_anime_producers AS anprod
        ON an.anime_id = anprod.anime_id
    LEFT JOIN producers AS prod
        ON anprod.producer_id = prod.producer_id
    LEFT JOIN links_girls_anime AS ganime
        ON an.anime_id = ganime.anime_id
    LEFT JOIN girls AS gir
        ON ganime.girl_id = gir.girl_id
WHERE an.series > @minSeries
AND prod.number_of_works > @minWorks
AND gir.fullname like '%" + name + "%'", con);

            command.Parameters.AddWithValue("minSeries", minSeries);
            command.Parameters.AddWithValue("minWorks", minWorks);

            var sw = new Stopwatch();
            sw.Start();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                res += "Anime id: ";
                res += reader.GetInt32(0);
                res += ". Title: ";
                res += reader.GetString(1);
                res += ". Series: ";
                res += reader.GetInt32(2);
                res += ". Producer id: ";
                res += reader.GetInt32(3);
                res += ". Name: ";
                res += reader.GetString(4);
                res += ". Number of works: ";
                res += reader.GetInt32(5);
                res += ". Girl id: ";
                res += reader.GetInt32(6);
                res += ". Name: ";
                res += reader.GetString(7);
                res += ".\n";
            }
            var elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine($"Query executed and results returned in {elapsed} milliseconds");
            con.Close();
            return res;
        }

        public string SearchTwo(double minRating, int minAge, int maxAge)
        {
            string res = "";

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($@"SELECT anime.anime_id, anime.title, anime.rating, girls.girl_id, girls.fullname, girls.age
from links_girls_anime
inner join anime on links_girls_anime.anime_id = anime.anime_id
inner join girls on links_girls_anime.girl_id = girls.girl_id
WHERE anime.rating > @minRating
AND girls.age between @minAge and @maxAge", con);

            command.Parameters.AddWithValue("minRating", minRating);
            command.Parameters.AddWithValue("minAge", minAge);
            command.Parameters.AddWithValue("maxAge", maxAge);

            var sw = new Stopwatch();
            sw.Start();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                res += "Anime id: ";
                res += reader.GetInt32(0);
                res += ". Title: ";
                res += reader.GetString(1);
                res += ". Rating: ";
                res += reader.GetDouble(2);
                res += ". Girl id: ";
                res += reader.GetInt32(3);
                res += ". Name: ";
                res += reader.GetString(4);
                res += ". Age: ";
                res += reader.GetInt32(5);
                res += ".\n";
            }
            var elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine($"Query executed and results returned in {elapsed} milliseconds");
            con.Close();
            return res;
        }

        public Tuple<List<string>, List<double>> MostRatedAnime()
        {
            List<string> titles = new List<string>();
            List<double> ratings = new List<double>();

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($@"select title, rating from anime where year > 2000 and series < 3 order by rating desc limit 10", con);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                titles.Add(reader.GetString(0));
                ratings.Add(reader.GetDouble(1));
            }
            con.Close();
            return Tuple.Create(titles, ratings);
        }

        public Tuple<List<decimal>, List<string>> RatingChanges(int id)
        {
            List<decimal> ratings = new List<decimal>();
            List<string> dates = new List<string>();

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($@"SELECT new_rating, date FROM public.rating_changes where anime_id = {id}", con);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ratings.Add(reader.GetDecimal(0));
                dates.Add(reader.GetDate(1).ToString());
            }
            con.Close();
            return Tuple.Create(ratings, dates);
        }
        #endregion

        #region random
        public bool AddRandomAnimeToDB(int count)
        {
            if (count <= 0)
                return false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"call random_anime(@count);", con);
            command.Parameters.AddWithValue("count", count);

            command.ExecuteScalar();

            con.Close();
            return true;
        }

        public bool AddRandomLinksGirlsAnimeToDB(int count)
        {
            if (count <= 0)
                return false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"call random_links_g_a(@count);", con);
            command.Parameters.AddWithValue("count", count);

            command.ExecuteScalar();

            con.Close();
            return true;
        }

        public bool AddRandomLinksAnimeProducersToDB(int count)
        {
            if (count <= 0)
                return false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"call random_links_a_p(@count);", con);
            command.Parameters.AddWithValue("count", count);

            command.ExecuteScalar();

            con.Close();
            return true;
        }

        #endregion
    }
}
