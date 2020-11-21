using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLab2.Configuration;
using DatabaseLab2.Models.DataBaseModels;
using DatabaseLab2.Models.Repositories;
using DatabaseLab2.Views;
using Microsoft.SqlServer.Server;
using Npgsql;
using NpgsqlTypes;

namespace DatabaseLab2
{
    class Program
    {
        static void Main(string[] args)
        {
            MainViewConsole mainView = new MainViewConsole(new Controllers.MainViewController(Configuration.Configuration.GetConnectionString()));

            mainView.Run();
        }
    }
}
