using DatabaseLab3.Infrastructure;
using DatabaseLab3.Models.DataBaseModels;
using Npgsql;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab3.Models.Repositories
{
    class ProducersRepository : BaseRepository
    {
        public ProducersRepository(string connectionString) : base(connectionString)
        {
            db = new DatabaseContext();
        }

        private DatabaseContext db;

        #region CRUD
        public IEnumerable<Producer> GetAllProducers()
        {
            List<Producer> producers = db.Producers.ToList();
            return producers;
        }

        public Producer GetProducer(int id)
        {
            Producer producer = db.Producers.Find(id);
            return producer;
        }
        public bool UpdateProducer(Producer producer)
        {
            try
            {
                Producer oldProducer = db.Producers.SingleOrDefault(x => x.ProducerId == producer.ProducerId);
                if (oldProducer != null)
                {
                    oldProducer.Name = producer.Name;
                    oldProducer.Studio = producer.Studio;
                    oldProducer.NumberOfWorks = producer.NumberOfWorks;
                    db.Producers.Update(oldProducer);
                    db.SaveChanges();
                }
                return true;
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
                db.Producers.Add(producer);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteProducer(int id)
        {
            try
            {
                Producer producer = db.Producers.Find(id);
                db.Producers.Remove(producer);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
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

        public Tuple<List<string>, List<int>> BestStudios()
        {
            List<string> name = new List<string>();
            List<int> num = new List<int>();

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($@"with tab1 as(
  select count(anime_id), producer_id 
  from links_anime_producers
  group by producer_id
  order by count DESC LIMIT 5)
select studio, count
from tab1, producers
where tab1.producer_id = producers.producer_id", con);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                name.Add(reader.GetString(0));
                num.Add(reader.GetInt32(1));
            }
            con.Close();
            return Tuple.Create(name, num);
        }

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
