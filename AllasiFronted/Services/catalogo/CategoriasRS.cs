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
    public class CategoriasRS
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlRest;
        private readonly JsonSerializerOptions _jsonOptions;

        public CategoriasRS(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            string baseUrl = "http://localhost:8080/Servicios-1.0-SNAPSHOT/api/";
            _urlRest = $"{baseUrl}categorias";
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
            };
        }

        public async Task<List<Categoria>> ListarTodasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_urlRest}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Categoria>>(jsonResponse, _jsonOptions) ?? new List<Categoria>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTodas: {ex.Message}");
            }
            return new List<Categoria>();
        }

        public async Task<int> InsertarAsync(Categoria categoria)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(categoria, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_urlRest}/{categoria.nombre}", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Categoria categoria_aux = JsonSerializer.Deserialize<Categoria>(jsonResponse, _jsonOptions);
                    return categoria_aux != null ? categoria_aux.id : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return 0;
        }

        public async Task<bool> EliminarAsync(Categoria categoria)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_urlRest}/{categoria.nombre}");
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
