# Usa una imagen base de .NET SDK para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar los archivos de la solución al contenedor
COPY . .

# 🔄 Especificar el archivo .csproj correcto (confirma la ruta)
RUN dotnet restore "EcommerceBackend.csproj"


# Publicar la aplicación en una carpeta para producción
RUN dotnet publish "EcommerceBackend.csproj" -c Release -o out

# Crear una imagen para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Establecer el directorio de trabajo en el contenedor
WORKDIR /app

# Copiar los archivos publicados desde la etapa de compilación
COPY --from=build /app/out .

# Exponer el puerto que la aplicación utilizará (por defecto, 80 para HTTP)
EXPOSE 80


# Definir el comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "EcommerceBackend.dll"]
