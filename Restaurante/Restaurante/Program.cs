using Application.Interfaces.ICategory;
using Application.Interfaces.IDeliveryType;
using Application.Interfaces.IDish;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrderItem;
using Application.Interfaces.IStatus;
using Application.Mapper;
using Application.Models.Response;
using Application.UseCase.CategoryService;
using Application.UseCase.DeliveryTypeService;
using Application.UseCase.OrderItemService;
using Application.UseCase.OrderService;
using Application.UseCase.Services.DishService;
using Application.UseCase.StatusService;
using Application.Validators;
using Application.Validators.DishValidator;
using Application.Validators.OrderItemValidator;
using Application.Validators.OrderValidator;
using Infrastructure.Command;
using Infrastructure.Commands;
using Infrastructure.Persistence;
using Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//CUSTOM
//Inyección de dependencias
var connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Restaurante API", Version = "v1" });
   
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
builder.Services.AddScoped<IGetOrderByIdService, GetOrderByIdService>();
builder.Services.AddScoped<IGetOrderByIdValidation, GetOrderByIdValidator>();
builder.Services.AddScoped<IGetAllOrdersService, GetAllOrdersService>();
builder.Services.AddScoped<IOrderMapper, OrderMapper>();
builder.Services.AddScoped<IGetAllOrdersValidation, GetAllOrdersValidator>();
builder.Services.AddScoped<IUpdateOrderStatusService, UpdateOrderStatusService>();
builder.Services.AddScoped<IUpdateOrderService, UpdateOrderService>();
builder.Services.AddScoped<IUpdateOrderValidation, UpdateOrderValidator>();
//ORDERITEM
builder.Services.AddScoped<IOrderItemCommand, OrderItemCommand>();
builder.Services.AddScoped<IOrderItemQuery, OrderItemQuery>();
builder.Services.AddScoped<ICreateOrderItemService, CreateOrderItemService>();
builder.Services.AddScoped<IUpdateOrderItemStatusService, UpdateOrderItemStatusService>();
builder.Services.AddScoped<IUpdateOrderItemStatusValidation, UpdateOrderItemStatusValidator>();
//END CUSTOM 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }
