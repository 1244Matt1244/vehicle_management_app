# Optimized Dockerfile
cat << EOF > Project.MVC/Dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Project.Service/Project.Service.csproj", "Project.Service/"]
COPY ["Project.MVC/Project.MVC.csproj", "Project.MVC/"]
RUN dotnet restore "Project.MVC/Project.MVC.csproj"
COPY . .
RUN dotnet publish "Project.MVC/Project.MVC.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection="Server=sql-server;Database=VehicleManagement;User=sa;Password=YourStrongPassword!;"
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Project.MVC.dll"]
EOF