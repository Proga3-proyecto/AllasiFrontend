using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Progra3_Frontend.Model;

namespace Progra3_Frontend.Services
{
    public class MarcasRS
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public MarcasRS(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
            };
        }

        public async Task<List<Marca>> ListarTodasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("marcas");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Marca>>(jsonResponse, _jsonOptions) ?? new List<Marca>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTodas: {ex.Message}");
            }
            return new List<Marca>();
        }

        public async Task<Marca?> InsertarAsync(Marca marca)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(marca, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"marcas/{marca.nombre}", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Marca>(jsonResponse, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> EliminarAsync(Marca marca)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"marcas/{marca.nombre}");
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