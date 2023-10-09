using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class ApiClient
{
    private readonly HttpClient httpClient;

    public ApiClient(string baseUrl, string bearerToken)
    {
        httpClient = new HttpClient();
        // Configurar la dirección base de la API
        httpClient.BaseAddress = new Uri(baseUrl);

        // Establecer el encabezado de autenticación Bearer
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
    }


    public string Post(string endpoint, string content)
    {
        // Configurar el contenido de la solicitud POST
        StringContent httpContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

        // Realizar la solicitud POST de forma síncrona
        HttpResponseMessage response = httpClient.PostAsync(endpoint, httpContent).Result;

        // Leer y devolver la respuesta como una cadena
        if (response.IsSuccessStatusCode)
        {
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return responseBody;
        }
        else
        {
            Console.WriteLine($"Error en la solicitud: {response.StatusCode}, {response.Content}");
            return null;
        }
    }

    public string Get(string endpoint)
    {
        // Realizar la solicitud POST de forma síncrona
        HttpResponseMessage response = httpClient.GetAsync(endpoint).Result;

        // Leer y devolver la respuesta como una cadena
        if (response.IsSuccessStatusCode)
        {
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return responseBody;
        }
        else
        {
            string responseBody = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Error en la solicitud: {response.StatusCode}, {responseBody}");
            return null;
        }
    }

}