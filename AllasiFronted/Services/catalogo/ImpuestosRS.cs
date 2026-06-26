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
    public class ImpuestosRS
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlRest;
        private readonly JsonSerializerOptions _jsonOptions;

        public ImpuestosRS(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            string baseUrl = "http://localhost:8080/Servicios-1.0-SNAPSHOT/api/";
            _urlRest = $"{baseUrl}impuestos";
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
            };
        }

        public async Task<List<Impuesto>> ListarTodosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_urlRest}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Impuesto>>(jsonResponse, _jsonOptions) ?? new List<Impuesto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTodos: {ex.Message}");
            }
            return new List<Impuesto>();
        }

        public async Task<int> InsertarAsync(Impuesto impuesto)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(impuesto, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_urlRest}", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Impuesto impuesto_aux = JsonSerializer.Deserialize<Impuesto>(jsonResponse, _jsonOptions);
                    return impuesto_aux != null ? impuesto_aux.id : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return 0;
        }

        public async Task<bool> ActualizarAsync(Impuesto impuesto)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(impuesto, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_urlRest}/{impuesto.id}", content);
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
