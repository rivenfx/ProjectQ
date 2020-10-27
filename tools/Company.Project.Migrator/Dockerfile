#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Company.Project.Migrator/Company.Project.Migrator.csproj", "src/Company.Project.Migrator/"]
COPY ["src/Company.Project.EntityFrameworkCore/Company.Project.EntityFrameworkCore.csproj", "src/Company.Project.EntityFrameworkCore/"]
COPY ["src/Company.Project.Domain/Company.Project.Domain.csproj", "src/Company.Project.Domain/"]
RUN dotnet restore "src/Company.Project.Migrator/Company.Project.Migrator.csproj"
COPY . .
WORKDIR "/src/src/Company.Project.Migrator"
RUN dotnet build "Company.Project.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Company.Project.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.Project.Migrator.dll"]