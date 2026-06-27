using System;
using System.IO;
using System.Net.Http.Headers;
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
    public class RecetasRS
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlRest;
        private List<Receta>? _cachedRecetas;
        private readonly JsonSerializerOptions _jsonOptions;

        public RecetasRS(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            string baseUrl = "http://localhost:8080/Servicios-1.0-SNAPSHOT/api/";
            _urlRest = $"{baseUrl}recetas";
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
            };
        }

        public async Task<List<Receta>> ListarTodosAsync()
        {
            if (_cachedRecetas != null)
            {
                return _cachedRecetas;
            }

            try
            {
                var response = await _httpClient.GetAsync($"{_urlRest}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    _cachedRecetas = JsonSerializer.Deserialize<List<Receta>>(jsonResponse, _jsonOptions) ?? new List<Receta>();
                    return _cachedRecetas;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTodos: {ex.Message}");
            }
            return new List<Receta>();
        }

        public async Task<Receta?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_urlRest}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Receta>(jsonResponse, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerPorId: {ex.Message}");
            }
            return null;
        }

        public async Task<int> InsertarAsync(Receta receta)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(receta, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_urlRest}", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    _cachedRecetas = null;
                    Receta receta_aux = JsonSerializer.Deserialize<Receta>(jsonResponse, _jsonOptions);
                    return receta_aux != null ? receta_aux.id : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return 0;
        }

        public async Task<bool> ActualizarAsync(Receta receta)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(receta, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_urlRest}/{receta.id}", content);
                if (response.IsSuccessStatusCode) _cachedRecetas = null;
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
                if (response.IsSuccessStatusCode) _cachedRecetas = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Eliminar: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SubirImagenAsync(int idReceta, Stream fileStream, string fileName, string contentType)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                content.Add(streamContent, "archivo", fileName);

                var response = await _httpClient.PostAsync($"{_urlRest}/{idReceta}/subir", content);
                if (response.IsSuccessStatusCode) _cachedRecetas = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SubirImagenAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SubirImagenPrincipalAsync(int idReceta, Stream fileStream, string fileName, string contentType)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                content.Add(streamContent, "archivo", fileName);

                var response = await _httpClient.PostAsync($"{_urlRest}/{idReceta}/subirPrincipal", content);
                if (response.IsSuccessStatusCode) _cachedRecetas = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SubirImagenPrincipalAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarCategoriaAsync(int idReceta, string nombreCategoria)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_urlRest}/{idReceta}/categoria/{nombreCategoria}");
                if (response.IsSuccessStatusCode) _cachedRecetas = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EliminarCategoriaAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AgregarCategoriaAsync(int idReceta, Categoria categoria)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(categoria, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_urlRest}/{idReceta}/categoria", content);
                if (response.IsSuccessStatusCode) _cachedRecetas = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AgregarCategoriaAsync: {ex.Message}");
                return false;
            }
        }
    }
}
