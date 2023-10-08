using System;
using Microsoft.Extensions.Configuration;

namespace Wakatimer_6._0
{
    internal class Variables
    {
        public string database_string;

        static void main()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            string db_host = configuration["db_host"];
            string db_password = configuration["db_password"];
            string db_user = configuration["db_user"];
            string db_database = configuration["db_database"];
            string db_port = configuration["db_port"];

            string database_string = database_strings(db_host, db_password, db_user, db_database, db_port);

        }

        static string database_strings(string db_host,string db_password,string db_user,string db_database,string db_port)
        {
            return $"Host={db_host};Port={db_port};Database={db_database};Username={db_user};Password={db_password}";
        }
        
    }
}
