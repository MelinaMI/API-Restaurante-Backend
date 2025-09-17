using Application.Models.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test.CustomTest
{
    public class OrderTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public OrderTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        [Fact(DisplayName = "POST-1: 201 | Creación exitosa de una órden válida")]
        public async Task CreateOrder_ShouldReturn201AndValidResponse()
        {
            // Arrange: cuerpo de la orden
            var orderRequest = new
            {
                items = new[]
                {
                new { id = "111BED38-4236-41C8-90D8-AEC18602BFD3", quantity = 2, notes = "Sin aderezo cesar" },
               
            },
                delivery = new { id = 1, to = "Av. Corrientes 1234, Buenos Aires" },
                notes = "Timbre: Departamento 5B"
            };
            // Act: enviar la solicitud POST
            var response = await _client.PostAsJsonAsync("/api/v1/Order", orderRequest);
           
            // Assert: verificar respuesta
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OrderCreateResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.True(result.OrderNumber > 0);
            Assert.True(result.TotalAmount > 0);
            Assert.True(result.CreatedAt <= DateTime.UtcNow);
        }
        [Fact(DisplayName = "POST-Error-1: Plato no válido")]
        public async Task CreateOrder_ShouldReturn400_ForInvalidDish()
        {
            // Arrange: cuerpo de la orden con plato inválido
            var orderRequest = new
            {
                items = new[]
                {
                new { DishId = "INVALID-DISH-ID", quantity = 2, notes = "Sin aderezo cesar" },
            },
                delivery = new { id = 1, to = "Av. Corrientes 1234, Buenos Aires" },
                notes = "Timbre: Departamento 5B"
            };
            // Act: enviar la solicitud POST
            var response = await _client.PostAsJsonAsync("/api/v1/Order", orderRequest);
            // Assert: verificar respuesta de error
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("El plato especificado no existe o no está disponible.", responseBody);
        }
    }
}
