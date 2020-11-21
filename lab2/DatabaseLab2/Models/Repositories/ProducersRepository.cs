using DatabaseLab2.Infrastructure;
using DatabaseLab2.Models.DataBaseModels;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.Repositories
{
    class ProducersRepository : BaseRepository
    {
        public ProducersRepository(string connectionString) : base(connectionString)
        {
        }

        #region CRUD
        public IEnumerable<Producer> GetAllProducers()
        {
            List<Producer> producers = new List<Producer>();

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand("SELECT * from producers ORDER BY producer_id ASC", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {

                producers.Add(ReadProducer(reader));
            }

            con.Close();
            return producers;
        }

        public Producer GetProducer(int id)
        {
            Producer producer = null;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM producers WHERE producer_id = {id}", con);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                producer = ReadProducer(reader);
            }

            con.Close();
            return producer;
        }
        public bool UpdateProducer(Producer producer)
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"UPDATE producers SET name = @name, studio = @studio, number_of_works = @number_of_works WHERE producer_id = {producer.Id}", con);

                command.Parameters.AddWithValue("@name", producer.Name);
                command.Parameters.AddWithValue("@studio", producer.Studio);
                command.Parameters.AddWithValue("@number_of_works", producer.NumberOfWorks);

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
        public bool InsertProducer(Producer producer)
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO producers(name, studio, number_of_works) VALUES(@name, @studio, @number_of_works)", con);

                command.Parameters.AddWithValue("@name", producer.Name);
                command.Parameters.AddWithValue("@studio", producer.Studio);
                command.Parameters.AddWithValue("@number_of_works", producer.NumberOfWorks);

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
        public bool DeleteProducer(int id)
        {
            bool res = false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"DELETE from producers WHERE producer_id = {id}", con);

            if (command.ExecuteNonQuery() != 0)
            {
                res = true;
            }

            con.Close();
            return res;
        }
        #endregion

        public IEnumerable<Anime> GetProducersAnime(int producerId)
        {
            con.Open();
            List<Anime> anime = new List<Anime>();

            NpgsqlCommand command = new NpgsqlCommand($"select * from anime where anime_id in (select anime_id from links_anime_producers where producer_id = {producerId})", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                anime.Add(ReadAnime(reader));
            }
            con.Close();
            return anime;
        }

        #region search
        public string SearchThree(string name, int minYear, int maxYear)
        {
            string res = "";

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($@"SELECT 
producers.producer_id, producers.name, producers.number_of_works, anime.anime_id, anime.title, anime.year
from links_anime_producers
inner join anime on links_anime_producers.anime_id = anime.anime_id
inner join producers on links_anime_producers.producer_id = producers.producer_id
WHERE producers.name like '%" + name + "%' AND anime.year between @minYear and @maxYear", con);

            command.Parameters.AddWithValue("minYear", minYear);
            command.Parameters.AddWithValue("maxYear", maxYear);

            var sw = new Stopwatch();
            sw.Start();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                res += "Producer id: ";
                res += reader.GetInt32(0);
                res += ". Name: ";
                res += reader.GetString(1);
                res += ". Number of works: ";
                res += reader.GetInt32(2);
                res += ". Anime id: ";
                res += reader.GetInt32(3);
                res += ". Title: ";
                res += reader.GetString(4);
                res += ". Release year: ";
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
        public bool AddRandomProducersToDB(int count)
        {
            if (count <= 0)
                return false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"call random_producers(@count);", con);
            command.Parameters.AddWithValue("count", count);

            command.ExecuteScalar();

            con.Close();
            return true;
        }
        #endregion
    }
}
