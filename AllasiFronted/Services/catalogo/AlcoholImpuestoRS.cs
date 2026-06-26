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
    public class AlcoholImpuestoRS
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlRest;
        private readonly JsonSerializerOptions _jsonOptions;

        public AlcoholImpuestoRS(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            string baseUrl = "http://localhost:8080/Servicios-1.0-SNAPSHOT/api/";
            _urlRest = $"{baseUrl}alcohol_impuestos";
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(), new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
            };
        }

        public async Task<List<AlcoholImpuesto>> ListarTodosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_urlRest}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<AlcoholImpuesto>>(jsonResponse, _jsonOptions) ?? new List<AlcoholImpuesto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTodos: {ex.Message}");
            }
            return new List<AlcoholImpuesto>();
        }

        public async Task<int> InsertarAsync(AlcoholImpuesto alcoholImpuesto)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(alcoholImpuesto, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_urlRest}", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    AlcoholImpuesto alcoholImpuesto_aux = JsonSerializer.Deserialize<AlcoholImpuesto>(jsonResponse, _jsonOptions);
                    return alcoholImpuesto_aux != null ? alcoholImpuesto_aux.id : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insertar: {ex.Message}");
            }
            return 0;
        }

        public async Task<bool> ActualizarAsync(AlcoholImpuesto alcoholImpuesto)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(alcoholImpuesto, _jsonOptions);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_urlRest}/{alcoholImpuesto.id}", content);
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
