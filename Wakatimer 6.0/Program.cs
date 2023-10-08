using System;
using Microsoft.Extensions.Configuration;
using Wakatimer_6._0;

class Program
{

    public Variables variables;
    static void Main()
    {
        CredentialManagerHelper credentialManagerHelper = new CredentialManagerHelper();
        CredentialManagerHelper.main();
        Variables variables = new Variables();
        DatabaseHelper database = new DatabaseHelper();
        List<Dictionary<string, object>> users = DatabaseHelper.ExecuteQuery("SELECT * FROM users us inner join users_tokens tk on tk.user_id = us.usuario_id");
        validate_expirate(users);

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
