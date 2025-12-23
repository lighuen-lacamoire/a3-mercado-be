
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
USER $APP_UID
WORKDIR /app

# Expose the port to the outside world
EXPOSE 8080
EXPOSE 8081
EXPOSE 5261
EXPOSE 7232

# Zona horaria
ENV TZ America/Argentina/Buenos_Aires

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-env
WORKDIR /src
# Copia el archivo de solucion .sln
COPY *.sln .

# Copia todos los proyectos .csproj de la carpeta src/
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

RUN dotnet restore "./src/A3.Mercado.API/A3.Mercado.API.csproj"


COPY . ./
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./src/A3.Mercado.API/A3.Mercado.API.csproj" -c $BUILD_CONFIGURATION -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .
ENTRYPOINT ["dotnet", "A3.Mercado.API.dll"]