using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using DatabaseLab2.Infrastructure;
using DatabaseLab2.Models.DataBaseModels;
using Npgsql;
using NpgsqlTypes;

namespace DatabaseLab2.Models.Repositories
{
    class GirlsRepository : BaseRepository
    {
        public GirlsRepository(string connectionString) : base(connectionString)
        {
        }

        #region CRUD
        public IEnumerable<Girl> GetAllGirls()
        {
            List<Girl> girls = new List<Girl>();

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand("SELECT * from girls ORDER BY girl_id ASC", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                girls.Add(ReadGirl(reader));
            }

            con.Close();
            return girls;
        }

        public Girl GetGirl(int id)
        {
            Girl girl = null;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"Select * from girls where girl_id = {id}", con);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                girl = ReadGirl(reader);
            }

            con.Close();
            return girl;
        }

        public bool UpdateGirl(Girl girl)
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"UPDATE girls SET fullname = @fullname, age = @age, hair = @hair, eyes = @eyes WHERE girl_id = {girl.Id}", con);

                command.Parameters.AddWithValue("@fullname", girl.Fullname);
                command.Parameters.AddWithValue("@age", girl.Age);
                command.Parameters.AddWithValue("@hair", girl.Hair);
                command.Parameters.AddWithValue("@eyes", girl.Eyes);

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

        public bool InsertGirl(Girl girl)
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO girls (fullname, age, hair, eyes) values(@fullname, @age, @hair, @eyes)", con);

                command.Parameters.AddWithValue("@fullname", girl.Fullname);
                command.Parameters.AddWithValue("@age", girl.Age);
                command.Parameters.AddWithValue("@hair", girl.Hair);
                command.Parameters.AddWithValue("@eyes", girl.Eyes);

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

        public bool DeleteGirl(int id)
        {
            bool res = false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"DELETE from girls WHERE girl_id = {id}", con);

            if (command.ExecuteNonQuery() != 0)
            {
                res = true;
            }
            con.Close();
            return res;
        }
        #endregion

        public IEnumerable<Anime> GetGirlsAnime(int girlId)
        {
            con.Open();
            List<Anime> anime = new List<Anime>();

            NpgsqlCommand command = new NpgsqlCommand($"select * from anime where anime_id in (select anime_id from links_girls_anime where girl_id = {girlId})", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                anime.Add(ReadAnime(reader));
            }
            con.Close();
            return anime;
        }

        #region random
        public bool AddRandomGirlsToDB(int count)
        {
            if (count <= 0)
                return false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"call random_girls(@count);", con);
            command.Parameters.AddWithValue("count", count);

            command.ExecuteScalar();

            con.Close();
            return true;
        }
        #endregion
    }
}