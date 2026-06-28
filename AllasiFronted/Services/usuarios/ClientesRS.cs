using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Progra3_Frontend.Model;
using AllasiFrontend.Model.carrito;

namespace Progra3_Frontend.Services
{
    public class ClientesRS
    {
        private readonly HttpClient _httpClient;
        private List<Cliente>? _cachedClientes;
        private readonly JsonSerializerOptions _jsonOptions;

        public ClientesRS(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                var response = await _httpClient.GetAsync("clientes");
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

        public async Task<Cliente?> GetClienteAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"clientes/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Cliente>(jsonResponse, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetCliente: {ex.Message}");
            }
            return null;
        }

        public async Task<Cliente?> InsertarAsync(Cliente cliente)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(cliente, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("clientes", content);
                if (response.IsSuccessStatusCode)
                {
                    _cachedClientes = null;
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Cliente>(jsonResponse, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> ActualizarAsync(Cliente cliente)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(cliente, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"clientes/{cliente.idUsuario}", content);
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
                var response = await _httpClient.DeleteAsync($"clientes/{id}");
                if (response.IsSuccessStatusCode) _cachedClientes = null;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Eliminar: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Pedido>> ObtenerPedidosPasadosAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"clientes/{id}/pedidos");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Pedido>>(jsonResponse, _jsonOptions) ?? new List<Pedido>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerPedidosPasados: {ex.Message}");
            }
            return new List<Pedido>();
        }

        public async Task<List<DetalleProducto>> ObtenerCarritoProductosAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"clientes/{id}/carritoProductos");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<DetalleProducto>>(jsonResponse, _jsonOptions) ?? new List<DetalleProducto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerCarritoProductos: {ex.Message}");
            }
            return new List<DetalleProducto>();
        }

        public async Task<List<DetalleReceta>> ObtenerCarritoRecetasAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"clientes/{id}/carritoRecetas");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<DetalleReceta>>(jsonResponse, _jsonOptions) ?? new List<DetalleReceta>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerCarritoRecetas: {ex.Message}");
            }
            return new List<DetalleReceta>();
        }

        public async Task<bool> CambiarPasswordAsync(int id, string nuevaPassword)
        {
            try
            {
                var requestBody = new { newPassword = nuevaPassword };

                var jsonBody = JsonSerializer.Serialize(requestBody, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"clientes/{id}/password", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CambiarPassword: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AgregarProductoAlCarritoAsync(int idCliente, int idProducto, int cantidad)
        {
            try
            {
                var requestBody = new { idProducto = idProducto, cantidad = cantidad };

                var jsonBody = JsonSerializer.Serialize(requestBody, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"clientes/{idCliente}/carritoProductos", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AgregarProductoAlCarrito: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AgregarRecetaAlCarritoAsync(int idCliente, int idReceta, int cantidad)
        {
            try
            {
                var requestBody = new { idReceta = idReceta, cantidad = cantidad };

                var jsonBody = JsonSerializer.Serialize(requestBody, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"clientes/{idCliente}/carritoRecetas", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AgregarRecetaAlCarrito: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> EliminarProductoDelCarritoAsync(int idCliente, int idProducto)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"clientes/{idCliente}/carritoProductos/{idProducto}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EliminarProductoDelCarrito: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarRecetaDelCarritoAsync(int idCliente, int idReceta)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"clientes/{idCliente}/carritoRecetas/{idReceta}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EliminarRecetaDelCarrito: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> ActualizarCantidadRecetaAsync(int idCliente, int idReceta, int _cantidad)
        {
            try
            {
                var requestBody = new ActualizarCantidadRequest(_cantidad);
                var jsonBody = JsonSerializer.Serialize(requestBody, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"clientes/{idCliente}/carritoRecetas/{idReceta}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en actualizar cantidad recetas: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ActualizarCantidadProductoAsync(int idCliente, int idProducto, int _cantidad)
        {
            try
            {
                var requestBody = new ActualizarCantidadRequest(_cantidad);
                var jsonBody = JsonSerializer.Serialize(requestBody, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"clientes/{idCliente}/carritoProductos/{idProducto}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en actualizar cantidad productos: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LimpiarCarritoAsync(int idCliente)
        {
            try
            {
                //var requestBody = new { };

                //var jsonBody = JsonSerializer.Serialize(requestBody, _jsonOptions);
                var response = await _httpClient.PostAsync($"clientes/{idCliente}/limpiarCarrito", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en actualizar cantidad productos: {ex.Message}");
                return false;
            }
        }
    }
}