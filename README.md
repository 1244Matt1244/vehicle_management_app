```markdown
# Vehicle Management System 🚗

[![.NET Build](https://github.com/1244Matt1244/vehicle_management_app/actions/workflows/dotnet.yml/badge.svg)](https://github.com/1244Matt1244/vehicle_management_app/actions)
[![License](https://img.shields.io/github/license/1244Matt1244/vehicle_management_app)](LICENSE)

**Modern vehicle inventory system** with robust CRUD operations and enterprise-ready architecture.

```diff
+ Production Ready | Clean Architecture | CI/CD Enabled | 85% Test Coverage
```

## 🌟 Features
- **Full Vehicle Management** - CRUD for makes/models
- **Advanced Filtering** - Pagination & sorting
- **Layered Architecture** - MVC ↔ Service ↔ Data
- **Automated Testing** - xUnit + Moq
- **CI/CD Pipeline** - GitHub Actions
- **HTTPS Security** - Dev certificate configured

## 🛠 Tech Stack
**.NET 9 Ecosystem**
- ASP.NET Core MVC
- Entity Framework Core 9
- AutoMapper
- xUnit + Moq
- SQL Server

**DevOps**
- GitHub Actions
- Docker Support
- EF Core Migrations

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server (LocalDB included)
- VS Code/Rider/Visual Studio

```bash
# Clone & Trust Certificate
git clone https://github.com/1244Matt1244/vehicle_management_app.git
cd vehicle_management_app
dotnet dev-certs https --trust

# Restore & Run
dotnet restore
dotnet run --project Project.MVC
```

Access: https://localhost:7266

## 🧪 Testing
```bash
# Run Unit Tests
dotnet test
```

## ☁️ Deployment

### Docker Production
```bash
docker build -t vehicle-mgmt -f Project.MVC/Dockerfile .
docker run -p 8080:80 vehicle-mgmt
```

## 🔧 Roadmap
```diff
+ 2025 Priorities
- [x] Core CRUD Functionality
- [x] Pagination/Sorting
- [ ] Azure AD Integration
- [ ] Performance Benchmarking

+ Quality Goals
! 95% Test Coverage
! Global Error Handling
```

## 🤝 Contributing
1. Fork repository
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Submit PR with:
   - Passing tests
   - Updated documentation
   - Clean commit history

**[📘 Documentation](docs/README.md) | [🐛 Issue Tracker](https://github.com/1244Matt1244/vehicle_management_app/issues)**
```