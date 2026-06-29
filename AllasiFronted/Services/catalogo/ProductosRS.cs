using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Progra3_Frontend.Model;

namespace Progra3_Frontend.Services
{
    public class ProductosRS
    {
        private readonly HttpClient _httpClient;
        private List<Producto>? _cachedProductos;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductosRS(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
            };
        }

        public async Task<List<Producto>> ListarTodosAsync()
        {
            if (_cachedProductos != null) return _cachedProductos;

            try
            {
                var response = await _httpClient.GetAsync("productos");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    _cachedProductos = JsonSerializer.Deserialize<List<Producto>>(jsonResponse, _jsonOptions) ?? new List<Producto>();
                    return _cachedProductos;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTodos: {ex.Message}");
            }
            return new List<Producto>();
        }

        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"productos/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Producto>(jsonResponse, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerPorId: {ex.Message}");
            }
            return null;
        }

        public async Task<Producto?> InsertarAsync(Producto producto)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(producto, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("productos", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    _cachedProductos = null;
                    return JsonSerializer.Deserialize<Producto>(jsonResponse, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> ActualizarAsync(Producto producto)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(producto, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"productos/{producto.id}", content);
                if (response.IsSuccessStatusCode) _cachedProductos = null;
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
                var response = await _httpClient.DeleteAsync($"productos/{id}");
                if (response.IsSuccessStatusCode) _cachedProductos = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Eliminar: {ex.Message}");
                return false;
            }
        }

        public async Task<Imagen?> SubirImagenAsync(int idProducto, Stream fileStream, string fileName, string contentType)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                content.Add(streamContent, "archivo", fileName);

                var response = await _httpClient.PostAsync($"productos/{idProducto}/imagen", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    _cachedProductos = null;
                    return JsonSerializer.Deserialize<Imagen>(jsonResponse, _jsonOptions);
                }

                return null;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error en SubirImagenAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> EliminarImagenAsync(int idProducto, int idImagen)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"productos/{idProducto}/imagen/{idImagen}");
                if (response.IsSuccessStatusCode) _cachedProductos = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EliminarImagenAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SubirImagenPrincipalAsync(int idProducto, int idImagen)
        {
            try
            {
                //using var content = new MultipartFormDataContent();
                //var streamContent = new StreamContent(fileStream);
                //streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                //content.Add(streamContent, "archivo", fileName);

                var response = await _httpClient.PutAsync($"productos/{idProducto}/imagenPrincipal/{idImagen}", null);
                if (response.IsSuccessStatusCode) _cachedProductos = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SubirImagenPrincipalAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarCategoriaAsync(int idProducto, string nombreCategoria)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"productos/{idProducto}/categoria/{nombreCategoria}");
                if (response.IsSuccessStatusCode) _cachedProductos = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EliminarCategoriaAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AgregarCategoriaAsync(int idProducto, Categoria categoria)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(categoria, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"productos/{idProducto}/categoria", content);
                if (response.IsSuccessStatusCode) _cachedProductos = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AgregarCategoriaAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CambiarMarcaAsync(int idProducto, Marca marca)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(marca, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"productos/{idProducto}/marca", content);
                if (response.IsSuccessStatusCode) _cachedProductos = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CambiarMarcaAsync: {ex.Message}");
                return false;
            }
        }
    }
}