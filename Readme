# Banca_Api

Este proyecto fue desarrollado en .NET Core 8 con SQLite. Incluye una colección de Postman para facilitar las pruebas de la API.

## Requisitos

- .NET Core 8
- SQLite (usado como base de datos)
  
### Instalación

1. Clona el repositorio:

    ```
    git clone <URL del repositorio>
    cd BancaApi
    ```

2. Limpia y restaura los paquetes del proyecto:

    ```
    dotnet clean
    dotnet restore
    dotnet build
    ```

### Ejecución del Proyecto
    ```
    dotnet run
    ```
Para ejecutar el proyecto localmente:

La API estará disponible en `http://localhost:5076`.

Para probar la API, se recomienda importar la colección de Postman `BancaApi.postman_collection`.

### Ejecución de Pruebas de Integración y Unitarias

Para ejecutar las pruebas unitarias e integración, usa el siguiente comando:


### Instalación y Ejecución con Docker

1. **Construir la imagen de Docker**:

    ```
    docker build -t bancaapi .
    ```

2. **Ejecutar el contenedor**:

    ```
    docker run -d -p 5076:5076 --name bancaapi_container bancaapi
    ```

### Base de Datos (SQLite)

Este proyecto usa SQLite para persistencia de datos. Asegúrate de que el archivo `BancaDB.db` esté disponible en el directorio adecuado.

Cuando ejecutes el contenedor de Docker, la base de datos se copiará en el contenedor como parte del proceso de construcción:


Si se necesita aplicar migraciones, el contenedor intentará ejecutarlas durante el arranque, usando la cadena de conexión configurada a través de las variables de entorno.

### Variables de Entorno

En el contenedor de Docker, la cadena de conexión se establece a través de la variable de entorno `DB_CONNECTION_STRING`. Esta es la ruta que se usará para conectar con la base de datos SQLite.

- **En local**: El proyecto usará la cadena de conexión de `appsettings.json` (definida como `Data Source=BancaDB.db`).
- **En Docker**: La cadena de conexión se ajustará a `Data Source=/app/data/BancaDB.db` dentro del contenedor.

### Configuración Adicional

Si tienes algún requisito específico para tu entorno de desarrollo o pruebas, por favor asegúrate de configurarlo antes de ejecutar el proyecto.


