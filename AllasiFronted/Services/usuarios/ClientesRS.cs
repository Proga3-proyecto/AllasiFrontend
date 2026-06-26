using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using Progra3_Frontend.Model;

namespace Progra3_Frontend.Services
{
    public class ClientesRS
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlRest;
        private List<Cliente>? _cachedClientes;
        private readonly JsonSerializerOptions _jsonOptions;

        public ClientesRS(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            string baseUrl = "http://localhost:8080/Servicios-1.0-SNAPSHOT/api/";
            _urlRest = $"{baseUrl}clientes";
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
            };
        }

        public async Task<List<Cliente>> ListarTodosAsync()
        {
            if (_cachedClientes != null) return _cachedClientes;

            try
            {
                var response = await _httpClient.GetAsync($"{_urlRest}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    _cachedClientes = JsonSerializer.Deserialize<List<Cliente>>(jsonResponse, _jsonOptions) ?? new List<Cliente>();
                    return _cachedClientes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTodos: {ex.Message}");
            }
            return new List<Cliente>();
        }

        public async Task<int> InsertarAsync(Cliente cliente)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(cliente, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_urlRest}", content);
                if (response.IsSuccessStatusCode)
                {
                    _cachedClientes = null;
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Cliente cliente_aux = JsonSerializer.Deserialize<Cliente>(jsonResponse, _jsonOptions);
                    return cliente_aux != null ? cliente_aux.idUsuario : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return 0;
        }

        public async Task<bool> ActualizarAsync(Cliente cliente)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(cliente, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_urlRest}/{cliente.idUsuario}", content);
                if (response.IsSuccessStatusCode) _cachedClientes = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Actualizar: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_urlRest}/{id}");
                if (response.IsSuccessStatusCode) _cachedClientes = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Eliminar: {ex.Message}");
                return false;
            }
        }
    }
}
