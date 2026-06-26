using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using Progra3_Frontend.Model;

namespace Progra3_Frontend.Services
{
    public class ImagenesRS
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlRest;
        private readonly JsonSerializerOptions _jsonOptions;

        public ImagenesRS(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            string baseUrl = "http://localhost:8080/Servicios-1.0-SNAPSHOT/api/";
            _urlRest = $"{baseUrl}imagenes";
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
            };
        }

        public async Task<List<Imagen>> ListarTodasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_urlRest}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Imagen>>(jsonResponse, _jsonOptions) ?? new List<Imagen>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTodas: {ex.Message}");
            }
            return new List<Imagen>();
        }

        public async Task<Imagen?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_urlRest}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Imagen>(jsonResponse, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerPorId: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> EliminarAsync(Imagen imagen)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_urlRest}/{imagen.id}");
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
