FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5016
EXPOSE 5016

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IntegrityService/App/App.csproj", "IntegrityService/App/"]
COPY ["IntegrityService/Application/Application.csproj", "IntegrityService/Application/"]
COPY ["IntegrityService/Domain/Domain.csproj", "IntegrityService/Domain/"]
COPY ["IntegrityService/Infrastructure/Infrastructure.csproj", "IntegrityService/Infrastructure/"]
COPY ["IntegrityService/Presentation/Presentation.csproj", "IntegrityService/Presentation/"]
RUN dotnet restore "IntegrityService/App/App.csproj"
COPY . .
WORKDIR "/src/IntegrityService/App"
RUN dotnet build "App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5016
ENTRYPOINT ["dotnet", "App.dll"]
