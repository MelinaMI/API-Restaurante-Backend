# 🍽️ Dish API - Proyecto de Software

API RESTful desarrollada con ASP.NET Core, utilizando Entity Framework Core para persistencia, y estructurada en tres capas según principios de Clean Arquitecture.

## 🧱 Estructura del Proyecto

| Capa               | Responsabilidad principal                                                     |
| -----------------  | ----------------------------------------------------------------------------- |
| **Domain**         | Entidades y lógica de negocio pura                                            |
| **Application**    | Validadores, interfaces y casos de uso                                        |
| **Infrastructure** | Persistencia con Entity Framework Core y configuración de acceso a datos      |

## 🔍 Tecnologías principales

| Tecnología              | Rol principal                                                              |
|------------------------|-----------------------------------------------------------------------------|
| **ASP.NET Core**       | Framework para construir APIs, manejar rutas, controladores y middleware.  |
| **Entity Framework Core (EFC)** | ORM para acceder y manipular datos en bases SQL desde objetos C#.         |

## 📄 Swagger/OpenAPI

Documentación enriquecida con ejemplos, restricciones, y respuestas de error formateadas.

## 🚀 Ejecución

1. Configurar `appsettings.json`  
2. Ejecutar migraciones (`dotnet ef database update`)  
3. Levantar con `dotnet run`
