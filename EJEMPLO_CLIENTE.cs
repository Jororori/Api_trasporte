// Cliente de ejemplo para consumir la API de Transportistas

using System.Net.Http.Json;

public class TransportistaClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:5001/api/transportista";

    public TransportistaClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Obtiene todos los transportistas
    /// </summary>
    public async Task ObtenerTodos()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<dynamic>($"{BaseUrl}");
            Console.WriteLine($"Respuesta: {response}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene un transportista por su ID
    /// </summary>
    public async Task ObtenerPorId(int id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<dynamic>($"{BaseUrl}/{id}");
            Console.WriteLine($"Respuesta: {response}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

// Ejemplo de uso en Program.cs de una aplicación cliente
public class Program
{
    public static async Task Main(string[] args)
    {
        // Configurar HttpClient
        var httpClient = new HttpClient();
        var client = new TransportistaClient(httpClient);

        // Obtener todos los transportistas
        await client.ObtenerTodos();

        // Obtener transportista por ID
        await client.ObtenerPorId(1);
    }
}
