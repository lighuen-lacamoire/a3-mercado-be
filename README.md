# A3 Challenge

El proyecto presenta las siguientes cualidades:

- Arquitectura Domain-Driven Design (DDD)
- API RESTful
- Documentación basada en OpenAPI 3.0
- Test Unitarios
  - Patrón de diseño Ágil: AAA (Arrange/Act/Assert)
  - Herramientas:
    - NSubstitute
    - AutoFixture
    - XUnit
- Docker: posee integración con docker-compose para levantar la aplicación en contenedores.

## Contenido

### Capas

- **API**: expone los servicios de aplicación implementando el diseño REST. Esta capa expone las funcionalidades/recursos implementados en la capa "Application".

- **Application**: Expone las funcionalidades/recursos que se consumen a través de endpoints en la capa "API". Interactúa con la capa de "Infrastructure" y con los objetos de la capa "Domain", orquestando las tareas necesarias para el manejo de la base de datos.

- **Infrastructure**: Implementa las capacidades técnicas requeridas por las capas "Application" y "Domain". Por ejemplo, la implementación de persistencia para la base de datos.

- **Domain**: Contiene los objetos del domino que modelan los datos y lógica requeridos por el negocio. Expone las interfaces de Repositorio de datos y las validaciones sobre estos.

- **UnitTests**: testea todo lo que respecta a la integración de los componentes de la aplicacion ejemplo la exposición de la capa de "API"

## Requisitos

- [x] [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [x] IDE: en caso de usar [Visual Studio](https://visualstudio.microsoft.com/), la versión mínima soportada es [VS Community Edition 2022](https://visualstudio.microsoft.com/vs/community/).
- [x] .NET CLI: se encuentra incluido en la SDK de .NET 8.
- [x] [Docker](https://www.docker.com/): el template ha sido diseñado para ejecutarse como un contenedor. Se puede descargar la herramienta desde el siguiente [link](https://www.docker.com/get-started).

## Desarrollo

- Iniciar utilizando Visual Studio
  Abrir con el visual studio el proyecto y seleccionar la opción **IIS Express**, darle play, y para invocar los endpoints seria con **http://localhost:5261**

- Iniciar utilizando VSCode
  Abrir el vscode en el directorio raiz, y luego en **run and debug** seleccionar la opción **.NET Core Iniciar Api <Play> (web)**, darle play, y se abrira automaticamente el servicio con la url **http://localhost:5261**
