using Application.Models.Request;
using Application.Models.Response;
using Restaurante.Models;
using FluentAssertions;
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
        //CREATE DISH PARA TESTS
        private async Task<DishResponse> CreateTestDish(string name = "Item Test Order", decimal price = 1000m, int category = 1)
        {
            var dish = new DishRequest
            {
                Name = name + Guid.NewGuid(), // evitar duplicados
                Description = "Plato de prueba para Order tests",
                Price = price,
                Category = category,
                Image = "https://restaurant.com/images/test.jpg"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Dish", dish);
            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<DishResponse>())!;
        }
        //CREATE ORDER CON UN DISH NUEVO
        private async Task<(long orderNumber, long itemId, decimal totalAmount)> CreateOrderWithDish(string dishName, int quantity = 1)
        {
            var dish = await CreateTestDish(dishName, 1000m, 6); // categoría 6: pizzas

            var orderReq = new OrderRequest
            {
                Items = new List<Items> { new Items { Id = dish.Id, Quantity = quantity, Notes = "PATCH item" } },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" },
                Notes = "Orden para PATCH test"
            };

            var postOrder = await _client.PostAsJsonAsync("/api/v1/Order", orderReq);
            postOrder.EnsureSuccessStatusCode();
            var createdOrder = await postOrder.Content.ReadFromJsonAsync<OrderCreateResponse>();

            var getOrder = await _client.GetAsync($"/api/v1/Order/{createdOrder!.OrderNumber}");
            getOrder.EnsureSuccessStatusCode();
            var order = await getOrder.Content.ReadFromJsonAsync<OrderDetailsResponse>();
            var itemId = order!.Items.First().Id;

            return (createdOrder.OrderNumber, itemId, (decimal)order.TotalAmount);
        }

        // POST TESTS
        [Fact(DisplayName = "POST-1: 201 | Creación exitosa de una orden")]
        public async Task Post_Should_Return_201_When_New_Valid_Order()
        {
            var dish = await CreateTestDish();

            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 2, Notes = "Sin sal" }
                },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" },
                Notes = "Timbre: 5B"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<OrderCreateResponse>();
            created.Should().NotBeNull();
            created!.OrderNumber.Should().BeGreaterThan(0);
            created.TotalAmount.Should().BeGreaterThan(0);
            created.CreatedAt.Should().BeAfter(DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact(DisplayName = "POST-2: 400 | Orden con plato inexistente")]
        public async Task Post_Should_Return_400_When_Dish_Does_Not_Exist()
        {
            // Paso 1: Crear orden con un plato inexistente (GUID aleatorio)
            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items
                    {
                        Id = Guid.NewGuid(), // No existe en la base
                        Quantity = 1,
                        Notes = "Sin sal"
                    }
                },
                Delivery = new Delivery
                {
                    Id = 1,
                    To = "Av. Corrientes 1234"
                },
                Notes = "Orden con plato inválido"
            };
            var response = await _client.PostAsJsonAsync("/api/v1/Order", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error?.Message.Should().Be("El plato especificado no existe o no está disponible.");
        }

        [Fact(DisplayName = "POST-3: 400 | Orden con cantidad inválida")]
        public async Task Post_Should_Return_400_When_Quantity_Is_Zero()
        {
            var dish = await CreateTestDish();

            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 0, Notes = "Cantidad inválida" }
                },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" },
                Notes = "Orden con cantidad cero"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error?.Message.Should().Be("La cantidad debe ser mayor que 0.");
        }

        [Fact(DisplayName = "POST-4: 400 | Orden con entrega inválida")]
        public async Task Post_Should_Return_400_When_Missing_Delivery()
        {
            var dish = await CreateTestDish();

            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 1, Notes = "Item Test" }
                },
                Delivery = new Delivery
                {
                    Id = 0, // inválido
                    To = "string"
                },
                Notes = "Orden con entrega inválida"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error?.Message.Should().Be("Debe especificar un tipo de entrega válido.");
        }

        [Fact(DisplayName = "POST-5: 400 | Orden con ID de ítem inválido")]
        public async Task Post_Should_Return_400_When_Item_Id_Is_Invalid_Guid()
        {
            var payload = new
            {
                items = new[]
                {
                 new { Id = Guid.NewGuid(), quantity = 1, notes = "ID inválido" }
                },
                delivery = new { id = 1, to = "Av. Corrientes 1234" },
                notes = "Orden con ID incorrecto"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", payload);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error?.Message.Should().Be("El plato especificado no existe o no está disponible.");
        }

        [Fact(DisplayName = "POST-6: 400 | Orden sin ítems")]
        public async Task Post_Should_Return_400_When_Items_Are_Empty()
        {
            var payload = new
            {
                items = Array.Empty<object>(),
                delivery = new { id = 1, to = "Av. Corrientes 1234" }
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", payload);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Debe ingresarse un parámetro válido.");
        }
        [Fact(DisplayName = "POST-7: 400 | Orden con ítems nulos")]
        public async Task Post_Should_Return_400_When_Items_Are_Null()
        {
            var payload = new
            {
                items = (object?)null,
                delivery = new { id = 1, to = "Av. Corrientes 1234" }
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", payload);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Debe ingresarse un parámetro válido.");
        }

        //GET TESTS
        [Fact(DisplayName = "GET-1: 200 | Recupera lista de órdenes existentes")]
        public async Task Get_Should_Return_200_With_List_Of_Orders()
        {
            var dish = await CreateTestDish();

            // Crear una orden de prueba
            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 1, Notes = "Test Item" }
                },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" },
                Notes = "Orden para GET ALL"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Order", request);
            var postBody = await postResponse.Content.ReadAsStringAsync();
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created, $"POST devolvió: {postBody}");

            // Ejecutar GET ALL
            var response = await _client.GetAsync("/api/v1/Order");
            var body = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"GET ALL devolvió: {body}");

            var orders = await response.Content.ReadFromJsonAsync<List<OrderDetailsResponse>>();
            orders.Should().NotBeNull("La respuesta debe contener una lista de órdenes");
            orders!.Should().NotBeEmpty("Debe existir al menos una orden registrada");

            var orden = orders.FirstOrDefault(o => o.Notes == "Orden para GET ALL");
            orden.Should().NotBeNull("La orden creada debe estar presente en el listado");

        }

        [Fact(DisplayName = "GET-2: 200 | Filtra órdenes por estado (status=1)")]
        public async Task Get_Should_Return_200_When_Filtering_By_Status()
        {
            // Arrange: Crear plato y orden con estado 1 (Pendiente)
            var dish = await CreateTestDish();

            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 1 }
                },
                Delivery = new Delivery { Id = 1, To = "Filtro de estado" },
                Notes = "Orden filtrada por estado"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Order", request);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // Act: Ejecutar GET con filtro de estado y rango de fechas
            var from = DateTime.UtcNow.AddMinutes(-5).ToString("yyyy-MM-ddTHH:mm:ss");
            var to = DateTime.UtcNow.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ss");
            var response = await _client.GetAsync($"/api/v1/Order?status=1&from={from}&to={to}");

            // Assert: Validar respuesta y contenido
            response.StatusCode.Should().Be(HttpStatusCode.OK, "El filtro por estado debe devolver 200 OK");

            var orders = await response.Content.ReadFromJsonAsync<List<OrderDetailsResponse>>();
            orders.Should().NotBeNull("La respuesta debe contener una lista de órdenes");
            orders!.Should().NotBeEmpty("Debe haber al menos una orden con estado 1");
            orders.All(o => o.Status.Id == 1).Should().BeTrue("Todas las órdenes deben tener estado 1 (Pendiente)");
        }
        [Fact(DisplayName = "GET-3: 400 | Rango de fechas inválido (from > to)")]
        public async Task Get_Should_Return_400_When_DateRange_Is_Invalid()
        {
            var from = DateTime.UtcNow;
            var to = DateTime.UtcNow.AddDays(-1); // Fecha final anterior a la inicial

            var response = await _client.GetAsync($"/api/v1/Order?from={from:o}&to={to:o}");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("La fecha de inicio no puede ser posterior a la fecha de fin");
        }

        [Fact(DisplayName = "GET-4: 200 | Orden existente por ID")]
        public async Task GetById_Should_Return_200_When_Order_Exists()
        {
            var dish = await CreateTestDish();

            var request = new OrderRequest
            {
                Items = new List<Items> { new Items { Id = dish.Id, Quantity = 2, Notes = "Test Item" } },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" },
                Notes = "Orden para GET BY ID"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Order", request);
            var created = await postResponse.Content.ReadFromJsonAsync<OrderCreateResponse>();

            var response = await _client.GetAsync($"/api/v1/Order/{created!.OrderNumber}");
            var order = await response.Content.ReadFromJsonAsync<OrderDetailsResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            order!.OrderNumber.Should().Be(created.OrderNumber);
            order.TotalAmount.Should().Be(created.TotalAmount);
            order.Items.Should().HaveCount(1);
        }

        [Fact(DisplayName = "GET-5: 404 | Orden inexistente por ID")]
        public async Task GetById_Should_Return_404_When_Order_Not_Exists()
        {
            var response = await _client.GetAsync("/api/v1/Order/999999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Orden no encontrada.");
        }

        [Fact(DisplayName = "GET-6: 200 | Filtra órdenes solo por estado")]
        public async Task Get_Should_Return_200_When_Filtering_Only_By_Status()
        {
            var dish = await CreateTestDish();

            var request = new OrderRequest
            {
                Items = new List<Items> { new Items { Id = dish.Id, Quantity = 1 } },
                Delivery = new Delivery { Id = 1, To = "Only status test" }
            };

            await _client.PostAsJsonAsync("/api/v1/Order", request);

            var response = await _client.GetAsync("/api/v1/Order?status=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var orders = await response.Content.ReadFromJsonAsync<List<OrderDetailsResponse>>();
            orders.Should().NotBeNull();
            orders!.All(o => o.Status.Id == 1).Should().BeTrue();
        }
        [Fact(DisplayName = "GET-7: 404 | ID no numérico en GET BY ID")]
        public async Task GetById_Should_Return_404_When_Id_Is_Not_Number()
        {
            var response = await _client.GetAsync("/api/v1/Order/abc");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        // PATCH TESTS
        [Fact(DisplayName = "PATCH-1: 200 | Actualiza estado de ítem")]
        public async Task Patch_Should_Return_200_When_Update_Item_Status()
        {
            var (orderNumber, itemId, total) = await CreateOrderWithDish("plato patch1");

            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 2 } // En preparación
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updated = await patchResponse.Content.ReadFromJsonAsync<OrderUpdateResponse>();
            updated!.OrderNumber.Should().Be(orderNumber);
            updated.TotalAmount.Should().Be((double)total);
        }
        [Fact(DisplayName = "PATCH-2: 400 | Estado nulo")]
        public async Task Patch_Should_Return_400_When_Status_Is_Null()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("patch-null");

            var response = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = (int?)null }
            );

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Nuevo estado no puede ser nulo");
        }
        [Fact(DisplayName = "PATCH-3: 400 | Estado no reconocido")]
        public async Task Patch_Should_Return_400_When_Status_Is_Not_Valid()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("patch-invalid");

            var response = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 999 }
            );

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("El estado especificado no es válido");
        }

        [Fact(DisplayName = "PATCH-4: 400 | Transición inválida de estado")]
        public async Task Patch_Should_Return_400_When_Transition_Is_Not_Allowed()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("patch-transition");

            var response = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 4 } // Pendiente → Entregado (no permitido)
            );

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("No se puede cambiar de 'Pendiente' a 'Entregado'");
        }
        [Fact(DisplayName = "PATCH-5: 404 | Ítem inexistente en la orden")]
        public async Task Patch_Should_Return_404_When_Item_Not_Found()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("plato patch4");

            var fakeItemId = itemId + 9999; // aseguramos que no exista

            var response = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{fakeItemId}",
                new { status = 2 }
            );

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Item no encontrado en la orden");
        }



        // CAMBIO DE ESTADOS TEST
        [Fact(DisplayName = "PATCH-6: 200 | En preparación → Listo")]
        public async Task Patch_Should_Return_200_When_Preparation_To_Ready()
        {
            var (orderNumber, itemId, total) = await CreateOrderWithDish("patch-prep-ready");

            // 1) Pendiente → En preparación
            await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 2 }
            );

            // 2) En preparación → Listo
            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 3 }
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await patchResponse.Content.ReadFromJsonAsync<OrderUpdateResponse>();
            updated!.OrderNumber.Should().Be(orderNumber);
            updated.TotalAmount.Should().Be((double)total);
        }
        [Fact(DisplayName = "PATCH-7: 200 | En preparación → Cancelada")]
        public async Task Patch_Should_Return_200_When_Preparation_To_Cancelled()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("patch-prep-cancel");

            // Pendiente → En preparación
            await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 2 }
            );

            // En preparación → Cancelada
            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 5 }
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await patchResponse.Content.ReadFromJsonAsync<OrderUpdateResponse>();
            updated!.OrderNumber.Should().Be(orderNumber);
        }
        [Fact(DisplayName = "PATCH-8: 200 | Listo → Cancelada")]
        public async Task Patch_Should_Return_200_When_Ready_To_Cancelled()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("patch-ready-cancel");

            // Pendiente → En preparación → Listo
            await _client.PatchAsJsonAsync($"/api/v1/Order/{orderNumber}/item/{itemId}", new { status = 2 });
            await _client.PatchAsJsonAsync($"/api/v1/Order/{orderNumber}/item/{itemId}", new { status = 3 });

            // Listo → Cancelada
            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 5 }
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await patchResponse.Content.ReadFromJsonAsync<OrderUpdateResponse>();
            updated!.OrderNumber.Should().Be(orderNumber);
        }
        [Fact(DisplayName = "PATCH-9: 200 | Entregado → Cancelada")]
        public async Task Patch_Should_Return_200_When_Delivered_To_Cancelled()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("patch-delivered-cancel");

            // Pendiente → En preparación → Listo → Entregado
            await _client.PatchAsJsonAsync($"/api/v1/Order/{orderNumber}/item/{itemId}", new { status = 2 });
            await _client.PatchAsJsonAsync($"/api/v1/Order/{orderNumber}/item/{itemId}", new { status = 3 });
            await _client.PatchAsJsonAsync($"/api/v1/Order/{orderNumber}/item/{itemId}", new { status = 4 });

            // Entregado → Cancelada
            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 5 }
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await patchResponse.Content.ReadFromJsonAsync<OrderUpdateResponse>();
            updated!.OrderNumber.Should().Be(orderNumber);
        }
        [Fact(DisplayName = "PATCH-10: 400 | Cancelada → Pendiente (inválido)")]
        public async Task Patch_Should_Return_400_When_Cancelled_To_Pending()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("patch-cancel-pending");

            // Cancelar directamente
            await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 5 }
            );

            // Intentar reabrir
            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 1 }
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await patchResponse.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("No se puede cambiar de 'Cancelada' a 'Pendiente'");
        }
    }
}

