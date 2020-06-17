#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update
RUN ["apt-get","install","-y","dos2unix"]
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/CalHealth.CalendarService/src/CalHealth.CalendarService.csproj", "Services/CalHealth.CalendarService/src/"]
RUN dotnet restore "Services/CalHealth.CalendarService/src/CalHealth.CalendarService.csproj"
COPY . .
WORKDIR "/src/Services/CalHealth.CalendarService/src"
RUN dotnet build "CalHealth.CalendarService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CalHealth.CalendarService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Services/CalHealth.CalendarService/src/wait-for-it.sh ./wait-for-it.sh
RUN dos2unix ./wait-for-it.sh && apt-get --purge remove -y dos2unix && rm -rf /var/lib/apt/lists/*
RUN chmod +x ./wait-for-it.sh
ENTRYPOINT ["./wait-for-it.sh", "calendar-db:1434", "-t", "40", "--", "dotnet", "CalHealth.CalendarService.dll"]

