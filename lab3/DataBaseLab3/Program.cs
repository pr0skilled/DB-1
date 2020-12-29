using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLab3.Models.DataBaseModels;
using DatabaseLab3.Models.Repositories;
using DatabaseLab3.Views;
using Microsoft.SqlServer.Server;
using Npgsql;
using NpgsqlTypes;

namespace DatabaseLab3
{
    class Program
    {
        static void Main(string[] args)
        {
            MainViewConsole mainView = new MainViewConsole(new Controllers.MainViewController("Host=localhost;Port=5432;Database=anime_girls;Username=postgres;Password=asd"));

            mainView.Run();
        }
    }
}
