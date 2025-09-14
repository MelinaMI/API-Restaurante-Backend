using Application.Interfaces.ICategory;
using Application.Interfaces.IDeliveryType;
using Application.Interfaces.IDish;
using Application.Interfaces.IStatus;
using Application.Mapper;
using Application.Models.Response;
using Application.Services.DishService;
using Application.Validators;
using Infrastructure.Commands;
using Infrastructure.Persistence;
using Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


builder.Services.AddScoped<ICreateService, CreateDishService>();
builder.Services.AddScoped<IUpdateService, UpdateDishService>();
builder.Services.AddScoped<ICreateValidation, CreateDishValidator>();
builder.Services.AddScoped<IUpdateValidation, UpdateDishValidator>();
builder.Services.AddScoped<IGetAllDishService, GetAllDishService>();
builder.Services.AddScoped<IDishCommand, DishCommand>();
builder.Services.AddScoped<IDishQuery, DishQuery>();
builder.Services.AddScoped<ICategoryQuery, CategoryQuery>();
builder.Services.AddScoped<Exceptions>();
builder.Services.AddScoped<IGetAllDishValidation, GetAllDishValidator>();
builder.Services.AddScoped<IGetDishByIdService, GetDishByIdService>();
builder.Services.AddScoped<IGetDishByIdValidation, GetDishByIdValidator>();
builder.Services.AddScoped<IDishMapper, DishMapper>();
builder.Services.AddScoped<IDeliveryTypeQuery, DeliveryTypeQuery>();
builder.Services.AddScoped<IStatusQuery, StatusQuery>();


//Manejo de errores de validación de modelos
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        return new BadRequestObjectResult(new ApiError
        {
            Message = "Parámetros de ordenamiento inválidos"
        });
    };
});

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
