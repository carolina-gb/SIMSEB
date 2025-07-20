# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos la solución
COPY ["PoyectoCarolina.sln", "./"]

# Copiamos cada csproj (esto permite aprovechar la cache de Docker)
COPY ["SIMSEB/SIMSEB.API.csproj", "SIMSEB/"]
COPY ["SIMSEB.Application/SIMSEB.Application.csproj", "SIMSEB.Application/"]
COPY ["SIMSEB.Domain/SIMSEB.Domain.csproj", "SIMSEB.Domain/"]
COPY ["SIMSEB.Infrastructure/SIMSEB.Infrastructure.csproj", "SIMSEB.Infrastructure/"]

# Restauramos los paquetes
RUN dotnet restore "PoyectoCarolina.sln"

# Ahora sí copiamos todo el código
COPY . .

# Publicamos SOLO el proyecto API
WORKDIR /src/SIMSEB
RUN dotnet publish -c Release -o /app/publish

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Puerto que expondrá Render
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SIMSEB.API.dll"]
