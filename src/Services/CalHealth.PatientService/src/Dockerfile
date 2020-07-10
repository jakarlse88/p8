#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update
RUN ["apt-get","install","-y","dos2unix"]
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/CalHealth.PatientService/src/CalHealth.PatientService.csproj", "Services/CalHealth.PatientService/src/"]
RUN dotnet restore "Services/CalHealth.PatientService/src/CalHealth.PatientService.csproj"
COPY . .
WORKDIR "/src/Services/CalHealth.PatientService/src"
RUN dotnet build "CalHealth.PatientService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CalHealth.PatientService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Services/CalHealth.PatientService/src/wait-for-it.sh ./wait-for-it.sh
RUN dos2unix ./wait-for-it.sh && apt-get --purge remove -y dos2unix && rm -rf /var/lib/apt/lists/*
RUN chmod +x ./wait-for-it.sh
ENTRYPOINT ["./wait-for-it.sh", "booking-db:1435", "-t", "40", "--", "dotnet", "CalHealth.PatientService.dll"]

