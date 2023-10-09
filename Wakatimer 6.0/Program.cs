using System;
using System.Net;
using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;
using Wakatimer_6._0;

class Program
{
    public static DateTime today = DateTime.Today;
    public static DateTime real_today = DateTime.Today;
    public static String summeris_endpoint = "/api/v1/users/current/summaries";
    public static String stats_endpoint = "/api/v1/users/current/stats/";
    static void Main(string[] args)
    {

        Console.WriteLine("Iniciando bot..");
        Console.WriteLine(args[0]);

        try
        {
            
            Variables variables = new Variables();

            



            DatabaseHelper database = new DatabaseHelper();
            List<Dictionary<string, object>> users = DatabaseHelper.ExecuteQuery("SELECT * FROM users us inner join users_tokens tk on tk.user_id = us.usuario_id");
            validate_expirate(users);
            Console.WriteLine(variables.api_host);

            foreach (var row in users)
            {

                Console.WriteLine($"***Comienza la busqueda de información para el usuario {row["nombre"]}***");
                ApiClient apiClient = new ApiClient(variables.api_host, (string)row["access_token"]);

                get_summeries_by_day(apiClient, row, database);

                if (today.DayOfWeek == DayOfWeek.Monday)
                {
                    Console.WriteLine($"Obteniendo los datos semanales para el usuario {row["nombre"]}");
                    get_summeries_by_week(apiClient, row, database);

                }

                if (today.DayOfWeek == DayOfWeek.Tuesday)
                {
                    string mode = "last_7_days";
                    Console.WriteLine($"Obteniendo los datos de estadisticas para el usuario {row["nombre"]} modo: {mode}");
                    get_stats(apiClient, row, database, mode, 1);

                }

                if (today.DayOfWeek == DayOfWeek.Wednesday)
                {
                    string mode = "last_30_days";
                    Console.WriteLine($"Obteniendo los datos de estadisticas para el usuario {row["nombre"]} modo: {mode}");
                    get_stats(apiClient, row, database, mode, 2);

                }

                if (today.DayOfWeek == DayOfWeek.Tuesday)
                {
                    string mode = "last_6_months";
                    Console.WriteLine($"Obteniendo los datos de estadisticas para el usuario {row["nombre"]} modo: {mode}");
                    get_stats(apiClient, row, database, mode, 3);
                }

                if (today.DayOfWeek == DayOfWeek.Friday)
                {
                    string mode = "last_year";
                    Console.WriteLine($"Obteniendo los datos de estadisticas para el usuario {row["nombre"]} modo: {mode}");
                    get_stats(apiClient, row, database, mode, 4);

                }

                if (today.DayOfWeek == DayOfWeek.Saturday)
                {
                    string mode = "all_time";
                    Console.WriteLine($"Obteniendo los datos de estadisticas para el usuario {row["nombre"]} modo: {mode}");
                    get_stats(apiClient, row, database, mode, 5);
                }
                
            }

            DatabaseHelper.ExecuteQuery($"insert into log (resultado, date) values ('Ok', to_timestamp('{real_today.ToString("yyyyMMdd")}', 'yyyymmdd'))");
            Console.WriteLine($"Bot Finalizo corretamente.");

        } catch (Exception e)
        {
            Console.WriteLine($"Error en la ejecución del bot {e.ToString()}");
            DatabaseHelper.ExecuteQuery($"insert into log (resultado, exception, date) values ('ERROR', '{e.ToString()}', to_timestamp('{real_today.ToString("yyyyMMdd")}', 'yyyymmdd'))");

        };
            
    }

    static void get_summeries_by_day(ApiClient apicli, Dictionary<string, object> row, DatabaseHelper database)
    {
        DateTime yesterday = today.AddDays(-1);

        string today_string = today.ToString("yyyyMMdd");
        string yestarday_string = yesterday.ToString("yyyyMMdd");


        string final_endpoint = $"{summeris_endpoint}?start={yestarday_string}&end={today_string}";


        string response = apicli.Get(final_endpoint);

        Console.WriteLine(response);

        Console.WriteLine("Guardando datos en la base de datos..");

        string insert_sql = $"Insert into summery (summery, user_id, created_date, mode, from_date, to_date) values ('{response}', {row["usuario_id"]}, to_timestamp('{real_today.ToString("yyyyMMdd")}', 'yyyymmdd'), {1}, to_timestamp('{yestarday_string}', 'yyyymmdd'), to_timestamp('{today_string}', 'yyyymmdd'))";

        Console.WriteLine(insert_sql);

        DatabaseHelper.ExecuteQuery(insert_sql);

    }

    static void get_summeries_by_week(ApiClient apicli, Dictionary<string, object> row, DatabaseHelper database)
    {
        DateTime last_monday = today.AddDays(-6);

        string last_monday_string = last_monday.ToString("yyyyMMdd");
        string today_string = today.ToString("yyyyMMdd");


        string final_endpoint = $"{summeris_endpoint}?start={last_monday}&end={today_string}";

        string response = apicli.Get(final_endpoint);

        Console.WriteLine(response);

        Console.WriteLine("Guardando datos en la base de datos..");

        string insert_sql = $"Insert into summery (summery, user_id, created_date, mode, from_date, to_date) values ('{response}', {row["usuario_id"]}, to_timestamp('{real_today.ToString("yyyyMMdd")}', 'yyyymmdd'), {2}, to_timestamp('{last_monday_string}', 'yyyymmdd'), to_timestamp('{today_string}', 'yyyymmdd'))";

        Console.WriteLine(insert_sql);

        DatabaseHelper.ExecuteQuery(insert_sql);

    }

    static void get_stats(ApiClient apicli, Dictionary<string, object> row, DatabaseHelper database, string mode, int mode_db)
    {

        string final_endpoint = $"{stats_endpoint}/{mode}";

        string response = apicli.Get(final_endpoint);

        Console.WriteLine(response);

        Console.WriteLine("Guardando datos en la base de datos..");

        string insert_sql = $"Insert into stats (stats, user_id, created_date, mode) values ('{response}', {row["usuario_id"]}, to_timestamp('{real_today.ToString("yyyyMMdd")}', 'yyyymmdd'), {mode_db}";

        Console.WriteLine(insert_sql);

        DatabaseHelper.ExecuteQuery(insert_sql);

    }

    static void validate_expirate(List<Dictionary<string, object>> result)
    {
        DateTime today = DateTime.Today;

        foreach (var row in result)
        {
            TimeSpan res = (DateTime)row["expires_at"] - today;

            if ( res.TotalDays > 30 )
            {
                Console.WriteLine("La fecha de expiración es correcta.");
            } else
            {
                Console.WriteLine($"El Access Token expirara en menos de 30 dias. Actualizar Access Token. USUARIO {row["nombre"]}");

            }

        }


    }

}
