using System;
using System.Net;

using Wakatimer_6._0;

namespace Wakatimer_6._0
{
    public class CredentialManagerHelper
    {
        public string db_password;
        public static void main()
        {
            string targetName = "postgres"; // Reemplaza con el nombre del objetivo en el Administrador de Credenciales

            string db_password = GetPassword(targetName);

            if (db_password != null)
            {
                Console.WriteLine($"Contraseña para '{targetName}': {db_password}");
            }
            else
            {
                Console.WriteLine("No se pudo obtener la contraseña.");
            }
        }

        public static string GetPassword(string targetName)
        {
            try
            {
                // Intenta obtener las credenciales genéricas del Administrador de Credenciales
                CredentialCache credentialCache = new CredentialCache();

                NetworkCredential networkCredential = credentialCache.GetCredential(new Uri("http://generic-credential"), "Negotiate");

                credentialCache.Add(new Uri("http://generic-credential"), "Negotiate", networkCredential);

                if (networkCredential != null)
                {
                    return networkCredential.Password;
                }
                else
                {
                    Console.WriteLine("No se encontraron credenciales genéricas.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las credenciales genéricas: {ex.Message}");
                return null;
            }
        }


    }
    }

