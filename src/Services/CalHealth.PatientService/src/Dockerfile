FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update && apt-get install -y wget
ENV DOCKERIZE_VERSION v0.6.1
RUN wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz
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
CMD dockerize \
    -wait http://rabbitmq:15672 -wait-retry-interval 30s -timeout 600s \
    -wait tcp://patient-db:1433 -wait-retry-interval 30s -timeout 600s \
    dotnet CalHealth.PatientService.dll