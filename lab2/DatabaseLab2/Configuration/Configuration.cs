using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Configuration
{
    static class Configuration
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["PostgreConnection"].ConnectionString;
        }
    }
}
