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
    private readonly string baseUrl;
    private readonly string bearerToken;

    public ApiClient(string baseUrl, string bearerToken)
    {
        this.baseUrl = baseUrl;
        this.bearerToken = bearerToken;
    }

    public async Task<string> PostAsync(string endpoint, string content)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            // Configurar la dirección base de la API
            httpClient.BaseAddress = new Uri(baseUrl);

            // Establecer el encabezado de autenticación Bearer
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            // Configurar el contenido de la solicitud POST
            StringContent httpContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

            // Realizar la solicitud POST
            HttpResponseMessage response = await httpClient.PostAsync(endpoint, httpContent);

            // Leer y devolver la respuesta como una cadena
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else
            {
                Console.WriteLine($"Error en la solicitud: {response.StatusCode}");
                return null;
            }
        }
    }
}