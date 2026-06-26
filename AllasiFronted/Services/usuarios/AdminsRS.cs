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
    public class AdminsRS
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlRest;
        private List<Admin>? _cachedAdmins;
        private readonly JsonSerializerOptions _jsonOptions;

        public AdminsRS(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            string baseUrl = "http://localhost:8080/Servicios-1.0-SNAPSHOT/api/";
            _urlRest = $"{baseUrl}administradores";
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
            };
        }

        public async Task<List<Admin>> ListarTodosAsync()
        {
            if (_cachedAdmins != null) return _cachedAdmins;

            try
            {
                var response = await _httpClient.GetAsync($"{_urlRest}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    _cachedAdmins = JsonSerializer.Deserialize<List<Admin>>(jsonResponse, _jsonOptions) ?? new List<Admin>();
                    return _cachedAdmins;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTodos: {ex.Message}");
            }
            return new List<Admin>();
        }

        public async Task<int> InsertarAsync(Admin admin)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(admin, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_urlRest}", content);
                if (response.IsSuccessStatusCode)
                {
                    _cachedAdmins = null;
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Admin admin_aux = JsonSerializer.Deserialize<Admin>(jsonResponse, _jsonOptions);
                    return admin_aux != null ? admin_aux.idUsuario : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return 0;
        }

        public async Task<bool> ActualizarAsync(Admin admin)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(admin, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_urlRest}/{admin.idUsuario}", content);
                if (response.IsSuccessStatusCode) _cachedAdmins = null;
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
                if (response.IsSuccessStatusCode) _cachedAdmins = null;
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

