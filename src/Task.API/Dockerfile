FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
RUN mkdir -p /home/app/.aspnet/https
COPY ./certs/.aspnet/https/Task.API.pfx /home/app/.aspnet/https/Task.API.pfx 
RUN chown -R app:app /home/app/.aspnet/https
USER app
WORKDIR /app
EXPOSE 7080
EXPOSE 7081

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Task.API/Task.API.csproj", "src/Task.API/"]
RUN dotnet restore "src/Task.API/Task.API.csproj"
COPY . .
WORKDIR "/src/src/Task.API"
RUN dotnet build "Task.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Task.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Task.API.dll"]
