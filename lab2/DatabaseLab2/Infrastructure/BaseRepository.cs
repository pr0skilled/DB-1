using DatabaseLab2.Models.DataBaseModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Infrastructure
{
    abstract class BaseRepository
    {
        protected readonly NpgsqlConnection con;

        protected Girl ReadGirl(NpgsqlDataReader reader)
        {
            return new Girl
            {
                Id = Convert.ToInt32(reader["girl_id"]),
                Fullname = reader["fullname"].ToString(),
                Age = Convert.ToInt32(reader["age"]),
                Hair = reader["hair"].ToString(),
                Eyes = reader["eyes"].ToString()
            };
        }

        protected Anime ReadAnime(NpgsqlDataReader reader)
        {
            return new Anime
            {
                Id = Convert.ToInt32(reader["anime_id"]),
                Title = reader["title"].ToString(),
                Year = Convert.ToInt32(reader["year"]),
                Series = Convert.ToInt32(reader["series"]),
                Rating = Convert.ToDouble(reader["rating"])
            };
        }

        protected Producer ReadProducer(NpgsqlDataReader reader)
        {
            return new Producer
            {
                Id = Convert.ToInt32(reader["producer_id"]),
                Name = reader["name"].ToString(),
                Studio = reader["studio"].ToString(),
                NumberOfWorks = Convert.ToInt32(reader["number_of_works"])
            };
        }

        public BaseRepository(string connectionString)
        {
            con = new NpgsqlConnection(connectionString);
        }
    }
}
