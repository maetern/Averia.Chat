#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app
EXPOSE 1090

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Averia.Chat.Server/Averia.Chat.Server.csproj", "Averia.Chat.Server/"]
COPY ["../Core/Averia.Core.Domain/Averia.Core.Domain.csproj", "../Core/Averia.Core.Domain/"]
COPY ["../Storage/Averia.Storage.Entity/Averia.Storage.Entity.csproj", "../Storage/Averia.Storage.Entity/"]
COPY ["../Core/Averia.Core.Domain.Server/Averia.Core.Domain.Server.csproj", "../Core/Averia.Core.Domain.Server/"]
COPY ["../Transport/Averia.Core.Server/Averia.Transport.Server.csproj", "../Transport/Averia.Core.Server/"]
RUN dotnet restore "Averia.Chat.Server/Averia.Chat.Server.csproj"
COPY . .
WORKDIR "/src/Averia.Chat.Server"
RUN dotnet build "Averia.Chat.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Averia.Chat.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Averia.Chat.Server.dll"]