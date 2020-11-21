using DatabaseLab2.Infrastructure;
using DatabaseLab2.Models.DataBaseModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.Repositories
{
    class AnimeRepository : BaseRepository
    {

        public AnimeRepository(string connectionString) : base(connectionString)
        {
        }

        #region CRUD
        public IEnumerable<Anime> GetAllAnime()
        {
            List<Anime> anime = new List<Anime>();
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM anime ORDER BY anime_id ASC", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {

                anime.Add(ReadAnime(reader));
            }

            con.Close();
            return anime;
        }

        public Anime GetAnime(int id)
        {
            Anime anime = null;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM anime WHERE anime_id = {id}", con);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                anime = ReadAnime(reader);
            }

            con.Close();
            return anime;
        }
        public bool UpdateAnime(Anime anime) 
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"UPDATE anime SET title = @title, year = @year, series = @series, rating = @rating WHERE anime_id = {anime.Id}", con);

                command.Parameters.AddWithValue("@title", anime.Title);
                command.Parameters.AddWithValue("@year", anime.Year);
                command.Parameters.AddWithValue("@series", anime.Series);
                command.Parameters.AddWithValue("@rating", anime.Rating);

                if (command.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                con.Close();
                return res;
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
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO anime(title, year, series, rating) VALUES(@title, @year, @series, @rating)", con);

                command.Parameters.AddWithValue("@title", anime.Title);
                command.Parameters.AddWithValue("@year", anime.Year);
                command.Parameters.AddWithValue("@series", anime.Series);
                command.Parameters.AddWithValue("@rating", anime.Rating);

                if (command.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                con.Close();
                return false;
            }
        }
        public bool DeleteAnime(int id)
        {
            bool res = false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"DELETE from anime WHERE anime_id = {id}", con);

            if (command.ExecuteNonQuery() != 0)
            {
                res = true;
            }

            con.Close();
            return res;
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
