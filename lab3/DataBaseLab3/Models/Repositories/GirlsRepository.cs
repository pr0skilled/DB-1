using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLab3.Infrastructure;
using DatabaseLab3.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace DatabaseLab3.Models.Repositories
{
    class GirlsRepository : BaseRepository
    {
        public GirlsRepository(string connectionString) : base(connectionString)
        {
            db = new DatabaseContext();
        }

        private DatabaseContext db;

        #region CRUD
        public List<Girl> GetAllGirls()
        {
            List<Girl> girls = db.Girls.ToList();
            return girls;
        }

        public Girl GetGirl(int id)
        {
            Girl girl = db.Girls.Find(id);
            return girl;
        }

        public bool UpdateGirl(Girl girl)
        {
            try
            {
                Girl oldGirl = db.Girls.SingleOrDefault(x => x.GirlId == girl.GirlId);
                if (oldGirl != null)
                {
                    oldGirl.Fullname = girl.Fullname;
                    oldGirl.Age = girl.Age;
                    oldGirl.Hair = girl.Hair;
                    oldGirl.Eyes = girl.Eyes;
                    db.Girls.Update(oldGirl);
                    db.SaveChanges();
                }
                return true;
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
                db.Girls.Add(girl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteGirl(int id)
        {
            try
            {
                Girl girl = db.Girls.Find(id);
                db.Girls.Remove(girl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
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