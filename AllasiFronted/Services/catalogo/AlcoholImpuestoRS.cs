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
    public class AlcoholImpuestoRS
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        // 1. Eliminamos IConfiguration del constructor ya que no lo necesitamos
        public AlcoholImpuestoRS(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
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
                var response = await _httpClient.GetAsync("alcohol_impuestos");
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

                var response = await _httpClient.PostAsync("alcohol_impuestos", content);
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

                var response = await _httpClient.PutAsync($"alcohol_impuestos/{alcoholImpuesto.id}", content);
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
                var response = await _httpClient.DeleteAsync($"alcohol_impuestos/{id}");
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