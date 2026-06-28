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
    public class PedidosRS
    {
        private readonly HttpClient _httpClient;
        private List<Pedido>? _cachedPedidos;
        private readonly JsonSerializerOptions _jsonOptions;

        public PedidosRS(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
            };
        }

        public async Task<List<Pedido>> ListarTodosAsync()
        {
            if (_cachedPedidos != null) return _cachedPedidos;

            try
            {
                var response = await _httpClient.GetAsync("pedidos");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    _cachedPedidos = JsonSerializer.Deserialize<List<Pedido>>(jsonResponse, _jsonOptions) ?? new List<Pedido>();
                    return _cachedPedidos;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTodos: {ex.Message}");
            }
            return new List<Pedido>();
        }

        public async Task<Pedido?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"pedidos/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Pedido>(jsonResponse, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerPorId: {ex.Message}");
            }
            return null;
        }

        public async Task<int> InsertarAsync(Pedido pedido)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(pedido, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("pedidos", content);
                if (response.IsSuccessStatusCode)
                {
                    _cachedPedidos = null;
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Pedido pedido_aux = JsonSerializer.Deserialize<Pedido>(jsonResponse, _jsonOptions);
                    return pedido_aux != null ? pedido_aux.id : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return 0;
        }

        public async Task<bool> ActualizarAsync(Pedido pedido)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(pedido, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"pedidos/{pedido.id}", content);
                if (response.IsSuccessStatusCode) _cachedPedidos = null;
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
                var response = await _httpClient.DeleteAsync($"pedidos/{id}");
                if (response.IsSuccessStatusCode) _cachedPedidos = null;
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