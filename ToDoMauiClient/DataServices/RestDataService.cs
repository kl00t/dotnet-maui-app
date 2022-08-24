using ToDoMauiClient.Models;
using System.Text.Json;
using System.Diagnostics;
using System.Text;

namespace ToDoMauiClient.DataServices;

public class RestDataService : IRestDataService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseAddress;
    private readonly string _url;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public RestDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5097" : "https://localhost:7097";
        _url = $"{_baseAddress}/api";
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task AddToDoAsync(ToDo toDo)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Debug.WriteLine("No Internet Access.");
            return;
        }

        try
        {
            string jsonToDo = JsonSerializer.Serialize<ToDo>(toDo, _jsonSerializerOptions);
            var content = new StringContent(jsonToDo, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/todo", content);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Successfully Created ToDo.");
            }
            else
            {
                Debug.WriteLine("Non Http 2xx Response.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}");
        }
    }

    public async Task DeleteToDoAsync(int id)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Debug.WriteLine("No Internet Access.");
            return;
        }

        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_url}/todo/{id}");
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Successfully Deleted ToDo.");
            }
            else
            {
                Debug.WriteLine("Non Http 2xx Response.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}");
        }

        return;
    }

    public async Task<List<ToDo>> GetAllToDosAsync()
    {
        List<ToDo> todos = new();
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Debug.WriteLine("No Internet Access.");
            return todos;
        }

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/todo");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                todos = JsonSerializer.Deserialize<List<ToDo>>(content, _jsonSerializerOptions);
            }
            else
            {
                Debug.WriteLine("Non Http 2xx Response.");
            }
        }
        catch(Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}");
        }

        return todos;
    }

    public async Task<ToDo> GetToDoAsync(int id)
    {
        ToDo todo = new();
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Debug.WriteLine("No Internet Access.");
            return todo;
        }

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/todo/{id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                todo = JsonSerializer.Deserialize<ToDo>(content, _jsonSerializerOptions);
            }
            else
            {
                Debug.WriteLine("Non Http 2xx Response.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}");
        }

        return todo;

    }

    public async Task UpdateToDoAsync(ToDo toDo)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Debug.WriteLine("No Internet Access.");
            return;
        }

        try
        {
            string jsonToDo = JsonSerializer.Serialize<ToDo>(toDo, _jsonSerializerOptions);
            var content = new StringContent(jsonToDo, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{_url}/todo/{toDo.Id}", content);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Successfully Updated ToDo.");
            }
            else
            {
                Debug.WriteLine("Non Http 2xx Response.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}");
        }
    }
}