using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player.Uti
{
    public class Helper
    {
        public static string GetConnectionString(string Connection)
        {
            return ConfigurationManager.ConnectionStrings[Connection].ConnectionString;
        }
    }
}
