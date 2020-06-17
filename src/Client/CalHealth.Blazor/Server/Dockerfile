#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Client/CalHealth.Blazor/Server/CalHealth.Blazor.Server.csproj", "Client/CalHealth.Blazor/Server/"]
COPY ["Client/CalHealth.Blazor/Client/CalHealth.Blazor.Client.csproj", "Client/CalHealth.Blazor/Client/"]
RUN dotnet restore "Client/CalHealth.Blazor/Server/CalHealth.Blazor.Server.csproj"
COPY . .
WORKDIR "/src/Client/CalHealth.Blazor/Server"
RUN dotnet build "CalHealth.Blazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CalHealth.Blazor.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalHealth.Blazor.Server.dll"]