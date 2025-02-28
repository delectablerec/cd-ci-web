# Fase 1: Build dell'applicazione

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY *.csproj ./

# RUN dotnet restore
COPY . ./

# ENV APP_DATA_PATH=/app/data

# Definizione della variabile d'ambiente per il path del file json
# ENV PRODOTTI_JSON_PATH=/app/data/prodotti.json

# Definizione della variabile d'ambiente per l'ambiente di esecuzione Development, Staging, Production
# ENV DOTNET_ENVIRONMENT="Production"

# ENV CONNECTION_STRING="Server=database;Database=cdciweb;User=sa;Password=Password"

# Evita problemi con le impostazioni di localizzazione nel container
# 1 = Invariant, 0 = Current
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# RUN dotnet publish -c Release -o out --no-restore

# oppure con restore assieme al run concatenato con &&
RUN dotnet restore && dotnet publish -c Release -o out

# Fase 2: Creazione dell'immagine finale leggera

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build app/out ./

# Creazione della cartella per i files json
# l attributo -p permette di creare anche le cartelle genitore cioe la cartella database
RUN mkdir -p /app/data

# Definizione del volume per i files json
VOLUME ["/app/data"]

EXPOSE 8080

ENTRYPOINT ["dotnet", "cd-ci-web.dll"]