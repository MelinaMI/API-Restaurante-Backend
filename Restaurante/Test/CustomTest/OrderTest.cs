using Application.Models.Request;
using Application.Models.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

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
            var dishRequest = new DishRequest
            {
                Name = "Plato de prueba",
                Description = "Descripción de prueba",
                Price = 500m,
                Category = 1,
                Image = "https://restaurant.com/images/test-dish.jpg"
            };

            var dishResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dishRequest);
            var createdDish = await dishResponse.Content.ReadFromJsonAsync<DishResponse>();
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

            var orderResponse = await _client.PostAsJsonAsync("/api/v1/Order", orderRequest);
            Assert.Equal(HttpStatusCode.Created, orderResponse.StatusCode);
            var createdOrder = await orderResponse.Content.ReadFromJsonAsync<OrderCreateResponse>();
            Assert.NotNull(createdOrder);
            Assert.True(createdOrder.OrderNumber > 0);
            Assert.True(createdOrder.TotalAmount > 0);
            Assert.True(createdOrder.CreatedAt <= DateTime.UtcNow);
        }

        [Fact(DisplayName = "POST-2: 400 | No se permite crear orden con plato no disponible")]
        public async Task CreateOrder_ShouldReturn400_ForInvalidDish()
        {
            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = Guid.NewGuid(), Quantity = 1, Notes = "Plato inválido" } // ID inválido
                },
                Delivery = new Delivery { Id = 1, To = "Calle falsa 123" },
                Notes = "Plato no disponible"
            };
            var response = await _client.PostAsJsonAsync("/api/v1/Order", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            Assert.Equal("El plato especificado no existe o no está disponible.", error.Message);
        }

        [Fact(DisplayName = "POST-3: 400 | No se permite crear orden sin ítems")]
        public async Task CreateOrder_WithoutItems_ShouldReturn400()
        {
            var request = new OrderRequest
            {
                Items = new List<Items>(), // vacío
                Delivery = new Delivery { Id = 1, To = "Calle falsa 123" },
                Notes = "Sin ítems"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            Assert.Equal("La orden debe contener al menos un ítem.", error.Message);
        }
        [Fact(DisplayName = "POST-4: 400 | No se permite ítem con cantidad igual a 0")]
        public async Task CreateOrder_ItemWithZeroQuantity_ShouldReturn400()
        {
            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = Guid.NewGuid(), Quantity = 0, Notes = "Cantidad cero" } // Cantidad inválida
                },
                Delivery = new Delivery { Id = 1, To = "Calle falsa 123" },
                Notes = "Ítem con cantidad cero"
            };
            var response = await _client.PostAsJsonAsync("/api/v1/Order", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            Assert.Equal("La cantidad debe ser mayor que 0.", error.Message);
        }
        [Fact(DisplayName = "POST-8: 400 | No se permite crear orden sin dirección de entrega")]
        public async Task CreateOrder_WithoutDeliveryAddress_ShouldReturn400()
        {
            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = Guid.NewGuid(), Quantity = 1, Notes = "Ítem válido" }
                },
                Delivery = new Delivery { Id = 1, To = "" }, // Dirección vacía
                Notes = "Sin dirección de entrega"
            };
            var response = await _client.PostAsJsonAsync("/api/v1/Order", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            Assert.Equal("Debe especificarse la dirección de entrega.", error.Message);
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
        [Fact(DisplayName = "GET-2: 200 | Listar órdenes por fecha y estado")]
        public async Task GetOrders_ByDateAndStatus_ShouldReturnFilteredResults()
        {
            // Paso 1: Crear plato válido
            var dishRequest = new DishRequest
            {
                Name = Guid.NewGuid().ToString(), // nombre único
                Description = "Plato para test de filtrado",
                Price = 500m,
                Category = 1,
                Image = "https://restaurant.com/images/test.jpg"
            };

            var dishResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dishRequest);
            var createdDish = await dishResponse.Content.ReadFromJsonAsync<DishResponse>();
            Assert.Equal(HttpStatusCode.Created, dishResponse.StatusCode);

            // Paso 2: Crear orden válida
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
                    To = "Calle falsa 123"
                },
                Notes = "Entrega rápida"
            };

            var orderResponse = await _client.PostAsJsonAsync("/api/v1/Order", orderRequest);
            var createdOrder = await orderResponse.Content.ReadFromJsonAsync<OrderCreateResponse>();
            Assert.Equal(HttpStatusCode.Created, orderResponse.StatusCode);

            // Paso 3: Filtrar por fecha y estado
            var fecha = createdOrder.CreatedAt.ToString("yyyy-MM-dd");
            var estado = 1; // Pending

            var filterResponse = await _client.GetAsync($"/api/v1/Order?date={fecha}&status={estado}");
            Assert.Equal(HttpStatusCode.OK, filterResponse.StatusCode);

            var orders = await filterResponse.Content.ReadFromJsonAsync<List<OrderDetailsResponse>>();
            Assert.NotEmpty(orders);

            // Paso 4: Validar que la orden creada esté en el resultado
            var ordenFiltrada = orders.FirstOrDefault(o => o.OrderNumber == createdOrder.OrderNumber);
            Assert.NotNull(ordenFiltrada);
            Assert.Equal(fecha, ordenFiltrada.CreatedAt.ToString("yyyy-MM-dd"));
            Assert.Equal(estado, ordenFiltrada.Status.Id);
        }
        [Fact(DisplayName = "PUT-1: 200 | Actualizar estado de ítem")]
        public async Task UpdateOrderItemStatus_ShouldReturn200()
        {
            // Paso 1: Crear plato
            var dishRequest = new DishRequest
            {
                Name = Guid.NewGuid().ToString(),
                Description = "Plato para actualizar estado",
                Price = 500m,
                Category = 1,
                Image = "https://restaurant.com/images/test.jpg"
            };
            var dishResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dishRequest);
            Assert.Equal(HttpStatusCode.Created, dishResponse.StatusCode);
            var dish = await dishResponse.Content.ReadFromJsonAsync<DishResponse>();
            Assert.NotNull(dish);

            // Paso 2: Crear orden con ese plato
            var orderRequest = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 1, Notes = "Sin sal" }
                },
                Delivery = new Delivery { Id = 1, To = "Calle falsa 123" },
                Notes = "Actualizar estado"
            };
            var orderResponse = await _client.PostAsJsonAsync("/api/v1/Order", orderRequest);
            Assert.Equal(HttpStatusCode.Created, orderResponse.StatusCode);
            var createdOrder = await orderResponse.Content.ReadFromJsonAsync<OrderCreateResponse>();
            Assert.NotNull(createdOrder);

            // Paso 3: Obtener detalles de la orden (usar OrderNumber o la propiedad que tu API expone)
            var detailsResponse = await _client.GetAsync($"/api/v1/Order/{createdOrder.OrderNumber}");
            Assert.Equal(HttpStatusCode.OK, detailsResponse.StatusCode);
            var details = await detailsResponse.Content.ReadFromJsonAsync<OrderDetailsResponse>();
            Assert.NotNull(details);
            Assert.NotNull(details.Items);
            Assert.NotEmpty(details.Items);

            var itemId = details.Items.First().Id;
            var orderId = details.OrderNumber; // o createdOrder.OrderNumber — mantén consistencia con tu DTO

            // Paso 4: Actualizar estado del ítem -> USAR LA RUTA CORRECTA
            var updateRequest = new OrderItemUpdateRequest { Status = 2 }; // 2 = "En preparación", por ej.
            var putResponse = await _client.PutAsJsonAsync($"/api/v1/Order/{orderId}/item/{itemId}", updateRequest);

            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

            var updatedItem = await putResponse.Content.ReadFromJsonAsync<OrderItemResponse>();
            Assert.NotNull(updatedItem);
            Assert.Equal(itemId, updatedItem.Id);
            Assert.NotNull(updatedItem.Status);
            Assert.Equal(2, updatedItem.Status.Id);
        }

        [Fact(DisplayName = "PUT-2: 200 | Agregar plato a orden existente")]
        public async Task AddDishToExistingOrder_ShouldReturn200()
        {
            // Plato base
            var dish1Request = new DishRequest
            {
                Name = Guid.NewGuid().ToString(),
                Description = "Plato base",
                Price = 500m,
                Category = 1,
                Image = "https://restaurant.com/images/base.jpg"
            };
            var dish1Response = await _client.PostAsJsonAsync("/api/v1/Dish", dish1Request);
            var dish1 = await dish1Response.Content.ReadFromJsonAsync<DishResponse>();

            // Plato adicional
            var dish2Request = new DishRequest
            {
                Name = Guid.NewGuid().ToString(),
                Description = "Plato adicional",
                Price = 600m,
                Category = 1,
                Image = "https://restaurant.com/images/add.jpg"
            };
            var dish2Response = await _client.PostAsJsonAsync("/api/v1/Dish", dish2Request);
            var dish2 = await dish2Response.Content.ReadFromJsonAsync<DishResponse>();

            // Crear orden con plato base
            var orderRequest = new OrderRequest
            {
                Items = new List<Items> { new Items { Id = dish1.Id, Quantity = 1 } },
                Delivery = new Delivery { Id = 1, To = "Calle falsa 123" },
                Notes = "Agregar plato"
            };
            var orderResponse = await _client.PostAsJsonAsync("/api/v1/Order", orderRequest);
            var createdOrder = await orderResponse.Content.ReadFromJsonAsync<OrderCreateResponse>();

            // Agregar plato adicional
            var updateRequest = new OrderUpdateRequest
            {
                Items = new List<Items> { new Items { Id = dish2.Id, Quantity = 1, Notes = "Sin sal" } }
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/v1/Order/{createdOrder.OrderNumber}/AddDish", updateRequest);
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var updatedOrder = await updateResponse.Content.ReadFromJsonAsync<OrderDetailsResponse>();
            Assert.Contains(updatedOrder.Items, i => i.Dish.Id == dish2.Id);
        }
        [Fact(DisplayName = "DELETE-1: 200 | Eliminar plato sin orden asociada")]
        public async Task DeleteDish_WithoutOrder_ShouldReturn200()
        {
            var dishRequest = new DishRequest
            {
                Name = Guid.NewGuid().ToString(),
                Description = "Plato sin orden",
                Price = 500m,
                Category = 1,
                Image = "https://restaurant.com/images/delete.jpg"
            };
            var dishResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dishRequest);
            var dish = await dishResponse.Content.ReadFromJsonAsync<DishResponse>();

            var deleteResponse = await _client.DeleteAsync($"/api/v1/Dish/{dish.Id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
        [Fact(DisplayName = "DELETE-2: 400 | No se permite eliminar plato con orden asociada")]
        public async Task DeleteDish_WithActiveOrder_ShouldReturn400()
        {
            // Paso 1: Crear plato
            var dishRequest = new DishRequest
            {
                Name = Guid.NewGuid().ToString(), // nombre único
                Description = "Plato vinculado",
                Price = 500m,
                Category = 1,
                Image = "https://restaurant.com/images/linked.jpg"
            };

            var dishResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dishRequest);
            Assert.Equal(HttpStatusCode.Created, dishResponse.StatusCode);
            var dish = await dishResponse.Content.ReadFromJsonAsync<DishResponse>();

            // Paso 2: Crear orden que incluya ese plato
            var orderRequest = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 1, Notes = "Sin sal" }
                },
                Delivery = new Delivery { Id = 1, To = "Calle falsa 123" },
                Notes = "Plato vinculado"
            };

            var orderResponse = await _client.PostAsJsonAsync("/api/v1/Order", orderRequest);
            Assert.Equal(HttpStatusCode.Created, orderResponse.StatusCode);

            // Paso 3: Intentar eliminar el plato
            var deleteResponse = await _client.DeleteAsync($"/api/v1/Dish/{dish.Id}");
            Assert.Equal(HttpStatusCode.BadRequest, deleteResponse.StatusCode);

            var error = await deleteResponse.Content.ReadFromJsonAsync<ApiError>();
            Assert.Equal("No se puede eliminar el plato porque está en órdenes activas", error.Message);
        }
    }
}
