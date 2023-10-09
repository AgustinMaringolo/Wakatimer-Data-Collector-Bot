using System;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class DatabaseHelper
{
    public static string GetConnectionString()
    {
        // Cambia los valores de conexión según tu configuración de PostgreSQL
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string host = configuration["db_host"];
        string port = configuration["db_port"];
        string database = configuration["db_database"];
        string username = configuration["db_user"];
        string password = configuration["db_password"];

        // Construye la cadena de conexión
        string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

        return connectionString;
    }

    public static List<Dictionary<string, object>> ExecuteQuery(string sql)
    {
        // Obtener la cadena de conexión
        string connectionString = GetConnectionString();
        List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                // Abrir la conexión
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            // Procesar los resultados
                      
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object columnValue = reader[i];
                                    row[columnName] = columnValue;
                                }

                                results.Add(row);
                            }

                            

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");

 
            }

            return results;
        }
    }
}
