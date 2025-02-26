# Dockerfile for Project.MVC
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY ./bin/Debug/net8.0/publish/ .
ENTRYPOINT ["dotnet", "Project.MVC.dll"]