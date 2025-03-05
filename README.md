```markdown
# Vehicle Management System 🚗

[![.NET Build](https://github.com/1244Matt1244/vehicle_management_app/actions/workflows/dotnet.yml/badge.svg)](https://github.com/1244Matt1244/vehicle_management_app/actions)
[![License](https://img.shields.io/github/license/1244Matt1244/vehicle_management_app)](LICENSE)

**Modern Vehicle Inventory System** built on the .NET 9 ecosystem with a clean, layered architecture. This application provides complete CRUD operations for vehicle makes and models—with advanced filtering, sorting, and paging—enforced with async/await, dependency injection via Ninject, and mapping via AutoMapper.

---

## Table of Contents

- [Overview](#overview)
- [Architecture & Workflow](#architecture--workflow)
- [Features](#features)
- [Requirements & Roadmap](#requirements--roadmap)
- [Database Migrations & Build Process](#database-migrations--build-process)
- [Getting Started](#getting-started)
- [Testing](#testing)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [License](#license)
- [Documentation](#documentation)

---

## Overview

- **Project.MVC**: Presentation layer with controllers, views, and view models.
- **Project.Service**: Business logic layer with EF Core models, the `VehicleService` class (supporting sorting, filtering, and paging), and AutoMapper integration.
- **Project.Tests**: Automated tests using xUnit and Moq.

---

## Architecture & Workflow

The system is built with a clear separation of concerns between the presentation (MVC), business logic (Service), and data access (EF Core) layers. The diagram below illustrates the flow from development in Visual Studio to database management via SQL Server Management Studio (SSMS).

```mermaid
flowchart TD
    subgraph DEV[Development Environment]
        VS[Visual Studio]
    end

    subgraph MVC[Project.MVC (Presentation Layer)]
        Controllers[Controllers]
        Views[Views & ViewModels]
    end

    subgraph SERVICE[Project.Service (Business Logic Layer)]
        VehicleService[VehicleService]
        AutoMapper[AutoMapper]
    end

    subgraph DATA[Data Access Layer]
        EFCore[Entity Framework Core]
        DbContext[ApplicationDbContext]
        SQLDB[SQL Server Database]
    end

    subgraph TOOLS[Database Tools]
        SSMS[SQL Server Management Studio]
    end

    VS -->|Develops & Edits Code| Controllers
    VS -->|Develops & Edits Code| VehicleService
    Controllers -->|Calls| VehicleService
    VehicleService -->|Maps DTOs| AutoMapper
    VehicleService -->|Queries/Updates| DbContext
    DbContext -->|Executes EF Core Commands| SQLDB
    SSMS --- SQLDB

    Controllers --- Views

    style VS fill:#f9f,stroke:#333,stroke-width:2px,stroke-dasharray: 5 5
    style SSMS fill:#bbf,stroke:#333,stroke-width:2px
```

---

## Features

- **Complete Vehicle Management:**  
  Full CRUD operations for vehicle makes and models.
- **Advanced Filtering & Sorting:**  
  Dynamic pagination, sorting, and search functionality.
- **Layered Architecture:**  
  Strict separation between MVC, Service, and Data layers.
- **Asynchronous Programming:**  
  Async/await enforced across all layers.
- **Dependency Injection & IoC:**  
  Ninject is used for DI to promote testability and loose coupling.
- **Object Mapping:**  
  AutoMapper converts EF models to DTOs/view models.
- **Global Error Handling:**  
  Custom middleware returns structured JSON error responses.
- **CI/CD & Docker Support:**  
  Automated builds, tests, and containerized deployments.
- **HTTPS Security:**  
  Enforced HTTPS using development certificates.

---

## Requirements & Roadmap

### Requirements

- **Database Setup:**  
  - **VehicleMake:** Columns: `Id`, `Name`, `Abrv` (e.g., BMW, Ford, Volkswagen)  
  - **VehicleModel:** Columns: `Id`, `MakeId`, `Name`, `Abrv` (e.g., 128, 325, X5 for BMW)
- **Solution Structure:**  
  - **Project.Service:** EF Core models and `VehicleService` with CRUD operations (including sorting, filtering, and paging).  
  - **Project.MVC:** Administration views for vehicle makes and models (with filtering by make).
- **Implementation Details:**  
  - Use async/await throughout.
  - Abstract classes with interfaces for unit testing.
  - Enforce IoC/DI using Ninject (constructor injection preferred).
  - Use AutoMapper for mapping.
  - Use EF Core (Code First) for database access.
  - Return view models (not EF models) in MVC.
  - Return proper HTTP status codes.

### Roadmap

#### 2025 Priorities
- [x] Implement Core CRUD Functionality for Vehicle Makes/Models  
- [x] Enhance Pagination, Sorting & Filtering  
- [ ] Integrate additional security measures  
- [ ] Refine global error handling and logging  

#### Quality Goals
- **High Test Coverage:** Robust automated testing.
- **Global Error Handling:** Precise and structured exception reporting.
- **Clean Architecture:** Strict separation of concerns across layers.

---

## Database Migrations & Build Process

### Database Migrations

Clean existing migrations and create a new migration to fix foreign key conflicts:

```bash
# Clean existing migrations
rm -rf Project.Service/Migrations/

# Create a new migration
dotnet ef migrations add FixForeignKeyConflict --project Project.Service --startup-project Project.MVC

# Apply the new migration
dotnet ef database update --project Project.Service --startup-project Project.MVC --verbose
```

### Build Process

Perform a full clean build and run the application:

```bash
dotnet clean && dotnet restore && dotnet build
dotnet run --project Project.MVC
```

### Deployment Workflow Diagram

```mermaid
flowchart TD
    A[Clean Existing Migrations]
    B[Create New Migration]
    C[Update Database]
    D[Clean, Restore, & Build]
    E[Run Application]
    
    A --> B
    B --> C
    C --> D
    D --> E
```

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB is included)
- Visual Studio

### Setup Instructions

1. **Clone the Repository:**
   ```bash
   git clone --recurse-submodules https://github.com/1244Matt1244/vehicle_management_app.git
   cd vehicle_management_app
   ```
2. **Initialize Development Certificates:**
   ```bash
   dotnet dev-certs https --clean
   dotnet dev-certs https --trust -ep ${HOME}/.aspnet/https/VehicleManagement.pfx -p "SecurePassword123!"
   ```
3. **Configure Local Secrets:**
   ```bash
   dotnet user-secrets init --project Project.MVC
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\MSSQLLocalDB;Database=VehicleManagement;Trusted_Connection=True;"
   ```
4. **Restore Dependencies and Build:**
   ```bash
   dotnet restore
   dotnet build
   ```
5. **Run the Application:**
   ```bash
   dotnet run --project Project.MVC
   ```
6. **Access the Application:**
   - Open [https://localhost:7266](https://localhost:7266) in your browser.

---

## Testing

Run automated tests using xUnit and Moq:

```bash
dotnet test
```

Test results, coverage, and performance metrics will be displayed in the terminal.

---

## Deployment

### Docker Deployment

1. **Build the Docker Image:**
   ```bash
   docker build -t vehicle-mgmt -f Project.MVC/Dockerfile .
   ```
2. **Run the Docker Container:**
   ```bash
   docker run -p 8080:80 vehicle-mgmt
   ```
3. **Access the Application:**
   - Visit [http://localhost:8080](http://localhost:8080).

---

## Contributing

Contributions are welcome! Please follow these guidelines:

1. **Fork the Repository**
2. **Create a Feature Branch:**
   ```bash
   git checkout -b feature/YourFeature
   ```
3. **Implement Your Changes:**  
   Ensure that all tests pass.
4. **Submit a Pull Request:**  
   Provide detailed commit messages and update documentation as needed.

For open issues or feature requests, please refer to our [Issue Tracker](https://github.com/1244Matt1244/vehicle_management_app/issues).

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Documentation

For further details on API endpoints, design decisions, and more, refer to our [docs/README.md](docs/README.md).

---

**Production Ready | Clean Architecture | CI/CD Enabled | Global Error Handling**
```
