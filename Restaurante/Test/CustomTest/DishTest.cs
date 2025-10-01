using Application.Models.Request;
using Application.Models.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Restaurante.Models;
using System.Net;
using System.Net.Http.Json;

namespace Test.CustomTest
{
    public class DishTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public DishTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        // ---------- 2-6) POST TESTS ----------

        [Fact(DisplayName = "POST-1: 201 | Creación exitosa de un plato válido")]
        public async Task Post_Should_Return_201_When_New_Valid_Dish()
        {
            // Arrange: preparar un plato válido
            var newDish = new DishRequest
            {
                Name = "Pizza Napolitana",
                Description = "Pizza con salsa de tomate, mozzarella, albahaca fresca y aceite de oliva",
                Price = 1000m,
                Category = 6,
                Image = "https://solorecetas.com/wp-content/uploads/2017/05/pizza-napolitana-2.jpg"
            };

            // Act: enviar la solicitud POST
            var response = await _client.PostAsJsonAsync("/api/v1/Dish", newDish);

            // Assert: verificar que se creó correctamente
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdDish = await response.Content.ReadFromJsonAsync<DishResponse>();
            createdDish.Should().NotBeNull();
            createdDish!.Name.Should().Be(newDish.Name);
            createdDish.Description.Should().Be(newDish.Description);
            createdDish.Price.Should().Be(newDish.Price);
            createdDish.Image.Should().Be(newDish.Image);
            createdDish.Category.Id.Should().Be(newDish.Category);
            createdDish.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "POST-2: 409 | No se puede crear un plato con nombre duplicado")]
        public async Task Post_Should_Return_409_When_Duplicate_Name()
        {
            // Arrange: preparar un plato con nombre único
            var dishRequest = new DishRequest
            {
                Name = "Ravioles de Ricota",
                Description = "Ravioles caseros rellenos de ricota y nuez con salsa de tomate",
                Price = 1000m,
                Category = 3,
                Image = "https://cdn2.cocinadelirante.com/1020x600/filters:format(webp):quality(75)/sites/default/files/images/2022/03/dia-de-los-ravioles-1.jpg"
            };

            // Act 1: crear el plato por primera vez
            var firstResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dishRequest);
            firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdDish = await firstResponse.Content.ReadFromJsonAsync<DishResponse>();
            createdDish.Should().NotBeNull();
            createdDish!.Name.Should().Be(dishRequest.Name);

            // Act 2: intentar crear el mismo plato nuevamente
            var duplicateResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dishRequest);

            // Assert: verificar que se rechaza por nombre duplicado
            duplicateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var error = await duplicateResponse.Content.ReadFromJsonAsync<ApiError>();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Ya existe un plato con ese nombre");
        }

        [Fact(DisplayName = "POST-3: 400 | No se puede crear un plato sin nombre")]
        public async Task Post_Should_Return_400_When_Name_Is_Empty()
        {
            // Arrange: preparar el plato con nombre vacío
            var invalidDish = new DishRequest
            {
                Name = "", // nombre vacío
                Description = "Intento de creación con nombre vacío",
                Price = 1000m,
                Category = 3,
                Image = "https://restaurant.com/images/pizza-test.jpg"
            };
            // Act: enviar la solicitud POST
            var response = await _client.PostAsJsonAsync("/api/v1/Dish", invalidDish);
            // Assert: verificar que se devuelve 400 y el mensaje de error esperado
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error.Should().NotBeNull();
            error!.Message.Should().Be("El nombre del plato es obligatorio");
        }

        [Fact(DisplayName = "POST-4: 400 | No se puede crear un plato con precio inválido")]
        public async Task Post_Should_Return_400_When_Price_Invalid()
        {
            // Arrange: preparar un plato con precio negativo
            var invalidDish = new DishRequest
            {
                Name = "Empanada de Carne",
                Description = "Empanada con precio inválido para test",
                Price = -1000m, // precio inválido
                Category = 3,
                Image = "https://restaurant.com/images/empanada.jpg"
            };

            // Act: enviar la solicitud POST
            var response = await _client.PostAsJsonAsync("/api/v1/Dish", invalidDish);

            // Assert: verificar que se devuelve 400 y el mensaje de error esperado
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error.Should().NotBeNull();
            error!.Message.Should().Be("El precio debe ser mayor a cero");
        }

        [Fact(DisplayName = "POST-5: 404 | No se puede crear un plato con categoría inexistente")]
        public async Task Post_Should_Return_404_When_Category_Not_Exists()
        {
            // Arrange: preparar un plato con categoría inválida (no registrada)
            var invalidCategoryDish = new DishRequest
            {
                Name = "Tarta de Espinaca",
                Description = "Plato con categoría inexistente para test",
                Price = 1000m,
                Category = 33, // categoría que no existe
                Image = "https://restaurant.com/images/tarta-espinaca.jpg"
            };

            // Act: enviar la solicitud POST
            var response = await _client.PostAsJsonAsync("/api/v1/Dish", invalidCategoryDish);

            // Assert: verificar que se devuelve 404 y el mensaje de error esperado
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error.Should().NotBeNull();
            error!.Message.Should().Be("La categoría no existe");
        }

        // ---------- 7-11) GET TESTS ----------

        [Fact(DisplayName = "GET-1: 200 | Obtener todos los platos existentes")]
        public async Task Get_Should_Return_200_With_All_Dishes()
        {
            // Arrange: preparar una lista de platos de prueba
            var dishesToCreate = new List<DishRequest>
            {
                new DishRequest
                {
                    Name = "Ñoquis con salsa rosa",
                    Description = "Ñoquis caseros con mezcla de salsa blanca y tomate",
                    Price = 1100m,
                    Category = 3,
                    Image = "https://img.freepik.com/foto-gratis/noquis-salsa-rosa_163431-42.jpg?size=626&ext=jpgg"
                },
                new DishRequest
                {
                    Name = "Ensalada César",
                    Description = "Lechuga, pollo grillado, croutons y aderezo César",
                    Price = 950m,
                    Category = 2,
                    Image = "https://tse2.mm.bing.net/th/id/OIP.X3yPzovImtJ8pbcrD6e57gEyDM?rs=1&pid=ImgDetMain&o=7&rm=3"
                },
                new DishRequest
                {
                    Name = "Hamburguesa Vegana",
                    Description = "Hamburguesa de lentejas con vegetales grillados",
                    Price = 1200m,
                    Category = 4,
                    Image = "https://th.bing.com/th/id/R.f1503bcd01728f6753b9f248c976b498?rik=McGsV%2fHuW9E%2bMA&pid=ImgRaw&r=0"
                }
            };

            // Act 1: crear los platos en la base de datos
            foreach (var dish in dishesToCreate)
            {
                var postResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dish);
                postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            }

            // Act 2: obtener todos los platos
            var getResponse = await _client.GetAsync("/api/v1/Dish");

            // Assert: verificar que se devuelve 200 y que los platos creados están presentes
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var allDishes = await getResponse.Content.ReadFromJsonAsync<List<DishResponse>>();
            allDishes.Should().NotBeNull();

            foreach (var dish in dishesToCreate)
            {
                allDishes.Should().Contain(d => d.Name == dish.Name);
            }
        }

        [Fact(DisplayName = "GET-2: 200 | Filtrar platos por nombre")]
        public async Task Get_Should_Return_200_Filter_By_Name()
        {
            // Arrange: crear un plato que contenga "Tarta" en el nombre
            var dish = new DishRequest
            {
                Name = "Tarta de Calabaza y Queso",
                Description = "Tarta casera con calabaza horneada y queso gratinado",
                Price = 1150m,
                Category = 2,
                Image = "https://static.wixstatic.com/media/c4453b_f544f31a8b8e483bbda4abc1a509441d~mv2.jpg/v1/fit/w_2500,h_1330,al_c/c4453b_f544f31a8b8e483bbda4abc1a509441d~mv2.jpg"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dish);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // Act: realizar la búsqueda filtrando por nombre
            var getResponse = await _client.GetAsync("/api/v1/Dish?name=Tarta");

            // Assert: verificar que se devuelve 200 y que el resultado contiene el plato esperado
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var filteredDishes = await getResponse.Content.ReadFromJsonAsync<List<DishResponse>>();
            filteredDishes.Should().NotBeNull();
            filteredDishes!.Should().OnlyContain(d =>
                d.Name.Contains("Tarta", StringComparison.OrdinalIgnoreCase));
        }

        [Fact(DisplayName = "GET-3: 200 | Ordenar platos existentes por precio ascendente")]
        public async Task Get_Should_Return_200_Sorted_By_Price_Asc()
        {
            // Arrange: crear platos con precios variados
            var dishesToCreate = new List<DishRequest>
            {
                new DishRequest
                {
                    Name = "Polenta con Ragú de Carne",
                    Description = "Polenta cremosa con salsa de carne cocida lentamente con vino tinto",
                    Price = 11000m,
                    Category = 3,
                    Image = "https://spagora.com.br/wp-content/uploads/2024/12/polenta-cremosa-com-ragu-de-carne-sabor-irresistivel-em-uma-receita-rapida-e-sem-complicacoes.jpg"
                },
                new DishRequest
                {
                    Name = "Sopa Thai de Coco y Curry",
                    Description = "Sopa exótica con leche de coco, curry rojo, pollo y cilantro fresco",
                    Price = 9500m,
                    Category = 5,
                    Image = "https://www.melonsinjamon.com/wp-content/uploads/2020/07/sopa-thai-coco-curry-vegana.jpg"
                },
                new DishRequest
                {
                    Name = "Ensalada Mediterránea",
                    Description = "Tomates cherry, aceitunas negras, queso feta, pepino y aceite de oliva",
                    Price = 15000m,
                    Category = 2,
                    Image = "https://www.gastrolabweb.com/u/fotografias/m/2021/5/18/f800x450-13354_64800_5942.jpg"
                }
            };

            foreach (var dish in dishesToCreate)
            {
                var postResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dish);
                postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            }

            // Act: obtener los platos ordenados por precio ascendente
            var getResponse = await _client.GetAsync("/api/v1/Dish?sortByPrice=asc");

            // Assert: verificar que se devuelve 200 y que los platos están ordenados correctamente
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var sortedDishes = await getResponse.Content.ReadFromJsonAsync<List<DishResponse>>();
            sortedDishes.Should().NotBeNull();
            sortedDishes!.Should().BeInAscendingOrder(d => d.Price);
        }

        [Fact(DisplayName = "GET-4: 200 | Ordenar platos existentes por precio descendente")]
        public async Task Get_Should_Return_200_Sorted_By_Price_Desc()
        {
            // Act: obtener los platos ordenados por precio descendente
            var getResponse = await _client.GetAsync("/api/v1/Dish?sortByPrice=desc");

            // Assert: verificar que se devuelve 200 y que los platos están ordenados correctamente
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var sortedDishes = await getResponse.Content.ReadFromJsonAsync<List<DishResponse>>();
            sortedDishes.Should().NotBeNull();
            sortedDishes!.Should().BeInDescendingOrder(d => d.Price);
        }

        [Fact(DisplayName = "GET-5: 400 | Ordenamiento inválido en parámetro sortByPrice")]
        public async Task Get_Should_Return_400_When_Sort_Invalid()
        {
            // Arrange: parámetro de ordenamiento inválido
            var invalidSortParam = "invalid";

            // Act: enviar la solicitud con un valor no convertible a OrderPrice
            var response = await _client.GetAsync($"/api/v1/Dish?sortByPrice={invalidSortParam}");

            // Assert: verificar que se devuelve 400
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Como ASP.NET Core no llega al controller, el contenido puede no ser ApiError
            var content = await response.Content.ReadAsStringAsync();

            // Verificamos que incluya la palabra "invalid" (o parte del mensaje por default de binding)
            content.Should().Contain("invalid", because: "el binding no pudo convertir el valor a enum");
        }

        [Fact(DisplayName = "GET-6: 200 | Filtrar platos por categoría")]
        public async Task Get_Should_Return_200_Filter_By_Category()
        {
            // Arrange: definir la categoría a filtrar (por ejemplo, 4 = Pastas)
            var categoryId = 4;

            // Act: enviar la solicitud GET con el filtro por categoría
            var response = await _client.GetAsync($"/api/v1/Dish?category={categoryId}");

            // Assert: verificar que se devuelve 200 y que todos los platos pertenecen a la categoría indicada
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var filteredDishes = await response.Content.ReadFromJsonAsync<List<DishResponse>>();
            filteredDishes.Should().NotBeNull();
            filteredDishes!.Should().OnlyContain(d => d.Category.Id == categoryId);
        }

        // ---------- 12-15) PUT TESTS ----------

        [Fact(DisplayName = "PUT-1: 200 | Actualización exitosa de un plato válido")]
        public async Task Put_Should_Return_200_When_Update_Valid()
        {
            // Arrange: crear un plato inicial
            var originalDish = new DishRequest
            {
                Name = "Fideos al Pesto",
                Description = "Fideos caseros con salsa de albahaca, ajo y nueces",
                Price = 1000m,
                Category = 3,
                Image = "https://tse3.mm.bing.net/th/id/OIP.l6Ji0GVz-tCVmb1jPkEIcAHaFj?rs=1&pid=ImgDetMain&o=7&rm=3"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/v1/Dish", originalDish);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdDish = await createResponse.Content.ReadFromJsonAsync<DishResponse>();
            createdDish.Should().NotBeNull();

            // Act: actualizar el plato con nuevos datos
            var updateRequest = new DishUpdateRequest
            {
                Name = "Risotto de Hongos Trufados",
                Description = "Risotto cremoso con hongos portobello, parmesano y aceite de trufa",
                Price = 1400m,
                Category = 3,
                Image = "https://byotoya.com/wp-content/uploads/2020/05/cba6178b-6e09-42ad-9a64-8c3247874ae9-2.jpg",
                IsActive = false
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/v1/Dish/{createdDish.Id}", updateRequest);

            // Assert: verificar que la actualización fue exitosa y los datos fueron modificados correctamente
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedDish = await updateResponse.Content.ReadFromJsonAsync<DishResponse>();
            updatedDish.Should().NotBeNull();
            updatedDish!.Name.Should().Be(updateRequest.Name);
            updatedDish.Description.Should().Be(updateRequest.Description);
            updatedDish.Price.Should().Be(updateRequest.Price);
            updatedDish.Image.Should().Be(updateRequest.Image);
            updatedDish.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = "PUT-2: 400 | No se puede actualizar un plato con precio inválido")]
        public async Task Put_Should_Return_400_When_Price_Invalid()
        {
            // Arrange: crear un plato válido inicialmente
            var originalDish = new DishRequest
            {
                Name = "Lasagna Bolognesa Garfield",
                Description = "La favorita de Garfield: capas infinitas de pasta, salsa bolognesa casera, bechamel cremosa y mucho, pero mucho queso gratinado.",
                Price = 1600m,
                Category = 4,
                Image = "https://www.gourmet.cl/wp-content/uploads/2016/09/iStock-505754880-1.jpg"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Dish", originalDish);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdDish = await postResponse.Content.ReadFromJsonAsync<DishResponse>();
            createdDish.Should().NotBeNull();

            // Act: intentar actualizar el plato con precio negativo
            var updateRequest = new DishUpdateRequest
            {
                Name = createdDish!.Name,
                Description = createdDish.Description,
                Image = createdDish.Image,
                Price = -10m, // precio inválido
                Category = createdDish.Category.Id,
                IsActive = false
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/v1/Dish/{createdDish.Id}", updateRequest);

            // Assert: verificar que se devuelve 400 y el mensaje de error esperado
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await updateResponse.Content.ReadFromJsonAsync<ApiError>();
            error.Should().NotBeNull();
            error!.Message.Should().Be("El precio debe ser mayor a cero");
        }


        [Fact(DisplayName = "PUT-3: 404 | No se puede actualizar un plato inexistente")]
        public async Task Put_Should_Return_404_When_Dish_Not_Found()
        {
            // Arrange: preparar una solicitud de actualización para un plato que no existe
            var updateRequest = new DishUpdateRequest
            {
                Name = "Plato inexistente",
                Description = "No debería actualizarse",
                Price = 1200m,
                Category = 3,
                Image = "https://restaurant.com/images/inexistente.jpg",
                IsActive = true
            };

            var nonExistentId = Guid.NewGuid(); // ID aleatorio que no corresponde a ningún plato

            // Act: enviar la solicitud PUT con el ID inexistente
            var response = await _client.PutAsJsonAsync($"/api/v1/Dish/{nonExistentId}", updateRequest);

            // Assert: verificar que se devuelve 404 y el mensaje de error esperado
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error.Should().NotBeNull();
            error!.Message.Should().Be("El plato no existe");
        }

        [Fact(DisplayName = "PUT-4: 409 | No se puede actualizar un plato con nombre duplicado")]
        public async Task Put_Should_Return_409_When_Name_Duplicate()
        {
            // Arrange: crear el primer plato con un nombre único
            var dishA = new DishRequest
            {
                Name = "Flan con Dulce de Leche",
                Description = "Flan casero acompañado con dulce de leche",
                Price = 5000m,
                Category = 10,
                Image = "https://tse1.mm.bing.net/th/id/OIP.mouC6Anzrihj38e8HaoAqgHaDt?rs=1&pid=ImgDetMain&o=7&rm=3"
            };

            var responseA = await _client.PostAsJsonAsync("/api/v1/Dish", dishA);
            responseA.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdA = await responseA.Content.ReadFromJsonAsync<DishResponse>();
            createdA.Should().NotBeNull();

            // Arrange: crear el segundo plato con otro nombre
            var dishB = new DishRequest
            {
                Name = "Agua mineral",
                Description = "Botella de agua sin gas 500ml",
                Price = 1200m,
                Category = 8,
                Image = "https://hiperlibertad.vteximg.com.br/arquivos/ids/162797-1000-1000/Agua-mineral-sin-gas-Villavicencio-500-cc-2-5960.jpg?v=637287221936070000"
            };

            var responseB = await _client.PostAsJsonAsync("/api/v1/Dish", dishB);
            responseB.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdB = await responseB.Content.ReadFromJsonAsync<DishResponse>();
            createdB.Should().NotBeNull();

            // Act: intentar actualizar el segundo plato usando el nombre del primer plato
            var updateRequest = new DishUpdateRequest
            {
                Name = createdA!.Name, // nombre duplicado
                Description = "Intento de duplicado",
                Price = 1300m,
                Category = createdB!.Category.Id,
                Image = "https://restaurant.com/images/plato-b-update.jpg",
                IsActive = true
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/v1/Dish/{createdB.Id}", updateRequest);

            // Assert: verificar que se devuelve 409 y el mensaje de error esperado
            updateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var error = await updateResponse.Content.ReadFromJsonAsync<ApiError>();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Ya existe otro plato con ese nombre");
        }
    }
}

