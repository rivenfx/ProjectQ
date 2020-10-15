#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Company.Project.Host/Company.Project.Host.csproj", "src/Company.Project.Host/"]
COPY ["src/Company.Project.Domain/Company.Project.Domain.csproj", "src/Company.Project.Domain/"]
COPY ["src/Company.Project.Application/Company.Project.Application.csproj", "src/Company.Project.Application/"]
COPY ["src/Company.Project.EntityFrameworkCore/Company.Project.EntityFrameworkCore.csproj", "src/Company.Project.EntityFrameworkCore/"]

RUN dotnet restore "src/Company.Project.Host/Company.Project.Host.csproj"
COPY . .
WORKDIR "/src/src/Company.Project.Host"
RUN dotnet build "Company.Project.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Company.Project.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.Project.Host.dll"]