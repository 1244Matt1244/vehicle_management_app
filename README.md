**`README.md`**
```markdown
# Vehicle Management App

A minimalistic ASP.NET MVC application for managing vehicle makes and models.

## Features
- CRUD operations for Vehicle Makes and Models
- Sorting, Filtering, and Pagination
- Async/Await throughout the stack
- Dependency Injection with Ninject
- AutoMapper for DTO/ViewModel mapping

## Technologies
- **Backend**: ASP.NET MVC, Entity Framework Core
- **Frontend**: Razor Views
- **Testing**: MSTest, Moq
- **Tools**: AutoMapper, Ninject

## Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/1244Matt1244/vehicle_management_app.git
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Configure the database:
   - Update connection string in `appsettings.json` (SQLite is used by default).

4. Run the application:
   ```bash
   dotnet run --project Project.MVC
   ```

5. Run tests:
   ```bash
   dotnet test
   ```

## Project Structure
- **Project.Service**: Contains EF models, repositories, services, and DTOs.
- **Project.MVC**: MVC controllers, views, and Ninject DI configuration.
- **Project.Tests**: Unit tests for services and controllers.
```