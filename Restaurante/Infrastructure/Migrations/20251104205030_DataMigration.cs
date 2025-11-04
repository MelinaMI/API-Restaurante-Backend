using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Dish",
                columns: new[] { "DishId", "Available", "Category", "CreateDate", "Description", "ImageUrl", "Name", "Price", "UpdateDate" },
                values: new object[,]
                {
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b001"), true, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Botella de agua sin gas 500ml", "https://hiperlibertad.vteximg.com.br/arquivos/ids/162797-1000-1000/Agua-mineral-sin-gas-Villavicencio-500-cc-2-5960.jpg?v=637287221936070000", "Agua mineral", 1200m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b002"), true, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tarta casera con calabaza horneada y queso gratinado", "https://static.wixstatic.com/media/c4453b_f544f31a8b8e483bbda4abc1a509441d~mv2.jpg/v1/fit/w_2500,h_1330,al_c/c4453b_f544f31a8b8e483bbda4abc1a509441d~mv2.jpg", "Tarta de Calabaza y Queso", 1150m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b003"), true, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tiras de costilla de vaca a la parrilla, jugosas y doradas", "https://bacoclub.com.ar/wp-content/uploads/2022/03/Diseno-sin-titulo-84.png", "Asado de tira", 5500m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b004"), true, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hamburguesa de lentejas con vegetales grillados", "https://th.bing.com/th/id/R.f1503bcd01728f6753b9f248c976b498?rik=McGsV%2fHuW9E%2bMA&pid=ImgRaw&r=0", "Hamburguesa Vegana", 1200m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b005"), true, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Milanesa de carne con lechuga y tomate", "https://www.infobae.com/resizer/v2/UZIPTP5U5JGKXCIAT35NTWH2OY.jpg?auth=e918349c7ab3c3362141e0871b125f8b0845005fe3755715814e7a1aa9763e1f&smart=true&width=1200&height=1200&quality=85", "Sándwich de milanesa", 2800m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b006"), true, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cerveza roja 500ml", "https://www.clarin.com/2022/03/09/K6mP1BcmN_2000x1500__1.jpg", "Roja artesanal", 2300m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b007"), true, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ñoquis caseros con mezcla de salsa blanca y tomate", "https://img.freepik.com/foto-gratis/noquis-salsa-rosa_163431-42.jpg?size=626&ext=jpgg", "Ñoquis con salsa rosa", 1100m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b008"), true, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Torta fría de galletitas de chocolate, dulce de leche y queso crema", "https://upload.wikimedia.org/wikipedia/commons/8/8c/Chocotorta_recipe.png", "Chocotorta", 1400m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b009"), true, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sandwich de lomo, lechuga, tomate, huevo y papas", "https://canalc.com.ar/wp-content/uploads/2022/04/lomito.jpeg", "Lomito completo", 3200m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00a"), true, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Empanadas horneadas rellenas de carne y cebolla", "https://recetasargentinas.net/wp-content/uploads/2014/02/IMG_6988-scaled.jpg", "Empanadas de carne", 1200m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00b"), true, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tostadas de pan con tomate, ajo y albahaca", "https://global.filippoberio.com/wp-content/uploads/2018/11/tomatobasilbruschetta.jpg", "Bruschetta", 950m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00c"), true, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pizza rellena de cebolla y queso", "https://www.thespruceeats.com/thmb/Rlhhh1kXMArdvRrVSZEkjhTp2-4=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/fugazza1-56b549cd5f9b5829f82d3106.jpg", "Fugazzeta", 3600m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00d"), true, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Matambre a la parrilla con salsa de tomate y queso gratinado", "https://media.airedesantafe.com.ar/p/b5592bf920dcbb803f78a99a3c8e7903/adjuntos/268/imagenes/003/777/0003777919/1200x675/smart/como-hacer-matambre-la-pizza-casa-la-receta-mas-facil-y-rica.png", "Matambre a la pizza", 5300m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00e"), true, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pizza con jamón, tomate y huevo duro", "https://easyways.cl/storage/20210208143331pizza-napolitana.jpg", "Pizza Napolitana", 3800m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00f"), true, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Huevos revueltos con vegetales frescos", "https://okdiario.com/img/recetas/2017/01/07/revuelto-de-verduras.jpg", "Revueltos de verduras", 1900m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b010"), true, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tortilla española con papas y cebolla", "https://alicante.com.ar/wp-content/uploads/2022/06/14_receta-1200x900.jpg", "Tortilla de papas", 2000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b011"), true, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Torta de chocolate con crema chantilly y cerezas", "https://petitfitbycris.com/wp-content/uploads/2021/06/TARTA-SELVA-NEGRA.jpg", "Torta selva negra", 1600m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b012"), true, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brownie caliente con helado de vainilla", "https://cloudfront-us-east-1.images.arcpublishing.com/elespectador/BS7XK3UP25GMJO3G5IXXDXCRCE.png", "Brownie con helado", 1500m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b013"), true, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flan casero acompañado con dulce de leche", "https://tse1.mm.bing.net/th/id/OIP.mouC6Anzrihj38e8HaoAqgHaDt?rs=1&pid=ImgDetMain&o=7&rm=3", "Flan con Dulce de Leche", 5000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b014"), true, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Base de masa dulce con crema y frutillas frescas", "https://kobietamag.pl/wp-content/uploads/2016/04/tarta-z-truskawkami-i-bita-smietana-ozdobiona-liscmi-miety.jpg", "Tarta de Frutilla", 1200m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b015"), true, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tomates cherry, aceitunas negras, queso feta, pepino y aceite de oliva", "https://www.gastrolabweb.com/u/fotografias/m/2021/5/18/f800x450-13354_64800_5942.jpg", "Ensalada Mediterránea", 15000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b016"), true, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lata de 330ml", "https://statics.dinoonline.com.ar/imagenes/full_600x600_ma/3080111_f.jpg", "Gaseosa Cola", 900m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b017"), true, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Capas de pasta, salsa bolognesa, bechamel y mucho queso", "https://www.gourmet.cl/wp-content/uploads/2016/09/iStock-505754880-1.jpg", "Lasagna Bolognesa Garfield", 1600m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b018"), true, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cerveza rubia 500ml", "https://capacitacioneselmolino.com/wp-content/uploads/2020/07/rubia.png", "Rubia artesanal", 2200m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b019"), true, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Botella 500ml, natural", "https://buenazo.cronosmedia.glr.pe/original/2022/02/07/62014ff370f93115af6521ad.jpg", "Jugo de naranja", 1500m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b01a"), true, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Queso provolone gratinado con especias", "https://statics.eleconomista.com.ar/2023/11/654969aa42ec0.jpg", "Provolone a la parrilla", 1500m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b01b"), true, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pasta con crema, queso y manteca", "https://assets.tmecosys.com/image/upload/t_web_rdp_recipe_584x480_1_5x/img/recipe/ras/Assets/376F934D-2E64-4FC4-A92F-9729A571EA61/Derivates/39233641-090E-46F7-B2E1-7B4E204EB111.jpg", "Fetuccine Alfredo", 2600m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b01c"), true, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ravioles caseros rellenos de ricota y nuez con salsa de tomate", "https://cdn2.cocinadelirante.com/1020x600/filters:format(webp):quality(75)/sites/default/files/images/2022/03/dia-de-los-ravioles-1.jpg", "Ravioles de Ricota", 1000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b01d"), true, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cerveza negra 500ml", "https://media.c5n.com/p/8257e620ec73971b92c765e48bbe38b6/adjuntos/326/imagenes/000/245/0000245175/790x0/smart/cerveza-negra.jpg", "Negra artesanal", 2400m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b01e"), true, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bondiola de cerdo marinada y asada a la parrilla", "https://i.ytimg.com/vi/mydgw6LJQMk/maxresdefault.jpg", "Bondiola", 4800m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b001"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b002"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b003"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b004"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b005"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b006"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b007"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b008"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b009"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00a"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00b"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00c"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00d"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00e"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b00f"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b010"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b011"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b012"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b013"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b014"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b015"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b016"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b017"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b018"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b019"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b01a"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b01b"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b01c"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b01d"));

            migrationBuilder.DeleteData(
                table: "Dish",
                keyColumn: "DishId",
                keyValue: new Guid("f0e1e2ab-4c49-496a-8fd5-1ebc0c77b01e"));
        }
    }
}
