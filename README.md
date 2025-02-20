```markdown
# Vehicle Management Application

[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

A complete vehicle make/model management system implementing all specified requirements.

## ✅ Verified Requirements

### Database Implementation
- [x] SQLite database with `VehicleMake` and `VehicleModel` tables (EF Core Code First)

### Back-End Solution
- [x] `Project.Service` 
  - Entity Framework models
  - `VehicleService` with full CRUD operations
  - Sorting/Filtering/Paging implementation
- [x] `Project.MVC`
  - Administration views for Makes/Models
  - Filtering by Make
  - Proper HTTP status codes

### Technical Implementation
- [x] Full async/await pipeline
- [x] Interface abstractions for unit testing
- [x] Ninject DI container configuration
- [x] AutoMapper integration
- [x] EF Core 8 Code First migrations
- [x] ViewModel/DTO separation

## 🚀 Features

**Vehicle Makes Management**
- Paginated listing (10 items/page)
- Sort by Name/Abrv (ASC/DESC)
- Search by name/abbreviation
- Create/Edit/Delete operations

**Vehicle Models Management**
- Filter by Make
- Search across model/make names
- Full CRUD operations
- Automatic Make relationship handling

## ⚙️ Installation

```bash
# Clone repository
git clone https://github.com/1244Matt1244/vehicle_management_app.git
cd vehicle_management_app

# Restore packages
dotnet restore

# Run migrations
dotnet ef database update --project src/Project.Service --startup-project src/Project.MVC

# Start application
dotnet run --project src/Project.MVC
```

## 🛠️ Project Structure

```
src/
├── Project.Service/        # Core business logic
│   ├── Data/               # EF Models/Migrations
│   ├── Services/           # VehicleService implementation
│   └── Interfaces/         # Service contracts
│
└── Project.MVC/            # Web interface
    ├── Controllers/        # MVC Controllers
    ├── ViewModels/         # Presentation models
    └── Views/              # Razor templates
```

## 🔍 Verification

1. **Database Configuration**
   - Check `ApplicationDbContext.cs` for model definitions
   - Verify migrations in `Project.Service/Migrations/`

2. **Dependency Injection**
   - Ninject config in `Program.cs`
   - Interface implementations in `Project.Service/Services/`

3. **Async Implementation**
   - All service methods use `async/await`
   - Controllers use asynchronous action methods

4. **Unit Testability**
   - Interfaces provided for all services
   - Constructor injection used throughout

## 📄 License
MIT License - See [LICENSE](LICENSE) for details
```