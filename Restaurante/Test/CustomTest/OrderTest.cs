using Application.Models.Request;
using Application.Models.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        [Fact(DisplayName = "POST-1: 201 | Creación exitosa de una orden válida")]
        public async Task CreateOrder_ShouldReturn201AndValidResponse()
        {
            // 1️⃣ Crear un plato válido en la DB de pruebas
            var dishRequest = new DishRequest
            {
                Name = "Plato de prueba",
                Description = "Descripción de prueba",
                Price = 500m,
                Category = 1,
                Image = "https://restaurant.com/images/test-dish.jpg"
            };

            var dishResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dishRequest);
            //dishResponse.EnsureSuccessStatusCode();

            var createdDish = await dishResponse.Content.ReadFromJsonAsync<DishResponse>();

            // 2️⃣ Crear la orden usando el plato recién creado
            var orderRequest = new OrderRequest
            {
                Items = new List<Items>
        {
            new Items
            {
                Id = createdDish.Id,
                Quantity = 2,
                Notes = "Sin albahaca"
            }
        },
                Delivery = new Delivery
                {
                    Id = 1,
                    To = "Calle falsa 123",
                },
                Notes = "Entrega rápida"
            };

            // 3️⃣ Ejecutar la creación de la orden
            var orderResponse = await _client.PostAsJsonAsync("/api/v1/Order", orderRequest);

            // 4️⃣ Verificar que devuelva 201 Created

            Assert.Equal(HttpStatusCode.Created, orderResponse.StatusCode);

            // 5️⃣ Leer y validar la respuesta usando OrderCreateResponse
            var createdOrder = await orderResponse.Content.ReadFromJsonAsync<OrderCreateResponse>();
            Assert.NotNull(createdOrder);
            Assert.True(createdOrder.OrderNumber > 0);
            Assert.True(createdOrder.TotalAmount > 0);
            Assert.True(createdOrder.CreatedAt <= DateTime.UtcNow);
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

        [Fact(DisplayName = "GET-1: Traer todas las ordenes")]
        public async Task GetAllOrders_ShouldReturn200AndValidResponse()
        {
            // Act: enviar la solicitud GET
            var response = await _client.GetAsync("/api/v1/Order");
            // Assert: verificar respuesta exitosa
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var orders = await response.Content.ReadFromJsonAsync<List<OrderDetailsResponse>>();
            Assert.NotNull(orders);
            Assert.True(orders.Count >= 0); // Puede haber 0 o más órdenes

        }
    }
}
