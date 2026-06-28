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
    public class CategoriasRS
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public CategoriasRS(HttpClient httpClient)
        {
            _httpClient = httpClient;

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
                // Usamos solo el endpoint relativo "categorias"
                var response = await _httpClient.GetAsync("categorias");
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

        public async Task<Categoria?> InsertarAsync(Categoria categoria)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(categoria, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Concatenamos el nombre de la categoría a la ruta relativa
                var response = await _httpClient.PostAsync($"categorias/{categoria.nombre}", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return  JsonSerializer.Deserialize<Categoria>(jsonResponse, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> EliminarAsync(Categoria categoria)
        {
            try
            {
                // Concatenamos el nombre de la categoría a la ruta relativa
                var response = await _httpClient.DeleteAsync($"categorias/{categoria.nombre}");
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