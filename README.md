# Vehicle Management App

ASP.NET Core MVC application for managing vehicle makes and models.

## Features
- CRUD operations for Vehicle Makes/Models
- Sorting/Paging/Filtering
- AutoMapper integration
- Ninject DI container

## Setup
1. Clone the repository
2. Update connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=VehicleManagement;Trusted_Connection=True;"
}