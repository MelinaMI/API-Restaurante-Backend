using Application.Interfaces.ICategory;
using Application.Interfaces.IDeliveryType;
using Application.Interfaces.IDish;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrderItem;
using Application.Interfaces.IStatus;
using Application.Mapper;
using Application.Services.CategoryService;
using Application.Services.DeliveryTypeService;
using Application.Services.DishService;
using Application.Services.OrderItemService;
using Application.Services.OrderService;
using Application.Services.StatusService;
using Application.Validators;
using Application.Validators.DishValidator;
using Application.Validators.OrderValidator;
using Infrastructure.Command;
using Infrastructure.Commands;
using Infrastructure.Persistence;
using Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//CUSTOM
//Inyección de dependencias
var connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddSwaggerGen(c =>
{
    // Mapea el enum OrderPrice como string con valores restringidos
    c.MapType<Application.Enum.OrderPrice>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
        {
            new Microsoft.OpenApi.Any.OpenApiString("asc"),
            new Microsoft.OpenApi.Any.OpenApiString("desc")
        }
    });
});

// Inyeccion de servicios
//DISH
builder.Services.AddScoped<ICreateValidation, CreateDishValidator>();
builder.Services.AddScoped<IUpdateValidation, UpdateDishValidator>();
builder.Services.AddScoped<IGetAllDishValidation, GetAllDishValidator>();
builder.Services.AddScoped<IGetDishByIdValidation, GetDishByIdValidator>();
builder.Services.AddScoped<IDeleteDishValidation, DeleteDishValidator>();
builder.Services.AddScoped<ICreateService, CreateDishService>();
builder.Services.AddScoped<IUpdateService, UpdateDishService>();
builder.Services.AddScoped<IGetAllDishService, GetAllDishService>();
builder.Services.AddScoped<IGetDishByIdService, GetDishByIdService>();
builder.Services.AddScoped<IDeleteDish, DeleteDishService>();
builder.Services.AddScoped<IDishCommand, DishCommand>();
builder.Services.AddScoped<IDishQuery, DishQuery>();
builder.Services.AddScoped<IDishMapper, DishMapper>();
//EXCEPTIONS
builder.Services.AddScoped<Exceptions>();
//CATEGORY
builder.Services.AddScoped<ICategoryQuery, CategoryQuery>();
builder.Services.AddScoped<IGetAllCategories, GetAllCategoriesService>();
//DELIVERYTYPE
builder.Services.AddScoped<IDeliveryTypeQuery, DeliveryTypeQuery>();
builder.Services.AddScoped<IGetAllDeliveryTypes, GetAllDeliveryTypesService>();
//STATUS
builder.Services.AddScoped<IStatusQuery, StatusQuery>();
builder.Services.AddScoped<IGetAllStatuses, GetAllStatusesService>();
//ORDER
builder.Services.AddScoped<ICreateOrderService, CreateOrderService>();
builder.Services.AddScoped<ICreateOrderValidation, CreateOrderValidator>();
builder.Services.AddScoped<IOrderCommand, OrderCommand>();
builder.Services.AddScoped<IOrderQuery, OrderQuery>();
builder.Services.AddScoped<IGetOrderById, GetOrderByIdService>();
builder.Services.AddScoped<IGetOrderByIdValidation, GetOrderByIdValidator>();
builder.Services.AddScoped<IGetAllOrders, GetAllOrdersService>();
builder.Services.AddScoped<IOrderMapper, OrderMapper>();
builder.Services.AddScoped<IGetAllOrdersValidation, GetAllOrdersValidator>();
//builder.Services.AddScoped<IUpdateOrderStatus, UpdateOrderStatusService>();
//builder.Services.AddScoped<IUpdateOrderStatusValidation, UpdateOrderStatusValidator>();
builder.Services.AddScoped<IUpdateOrderService, UpdateOrderService>();
builder.Services.AddScoped<IUpdateOrderValidation, UpdateOrderValidator>();
//ORDERITEM
builder.Services.AddScoped<IOrderItemCommand, OrderItemCommand>();
builder.Services.AddScoped<IOrderItemQuery, OrderItemQuery>();
builder.Services.AddScoped<ICreateOrderItem, CreateOrderItemService>();


//builder.Services.AddScoped<IOrderMapper, OrderMapper>();
//builder.Services.AddScoped<IOrderItemMapper, OrderItemMapper>();

//END CUSTOM 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Restaurante v1");
    });
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }
