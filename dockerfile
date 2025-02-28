# Fase 1: Build dell'applicazione

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./

# ENV CONNECTION_STRING="Server=database;Database=cdciweb;User=sa;Password=Password"

# COPY --from=build /out .

# RUN dotnet publish -c Release -o out --no-restore

# 
COPY . ./

# oppure con restore assieme al run concatenato con &&
RUN dotnet restore && dotnet publish -c Release -o out

# Fase 2: Creazione dell'immagine finale leggera

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build app/out ./

# Definizione della variabile d'ambiente per il fuso orario
ENV TZ=Europe/Rome
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# Imposta la cultura e la valuta per il sistema
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0
RUN apt-get update && apt-get install -y locales && rm -rf /var/lib/apt/lists/*
RUN locale-gen it_IT.UTF-8
ENV LANG=it_IT.UTF-8
ENV LC_ALL=it_IT.UTF-8

# Imposta la valuta come EUR
ENV DOTNET_CURRENCY="€"

# Definizione della variabile d'ambiente per l'ambiente di esecuzione Development, Staging, Production
ENV DOTNET_ENVIRONMENT="Production"

# ENV CONNECTION_STRING="Server=database;Database=cdciweb;User=sa;Password=Password"

# Evita problemi con le impostazioni di localizzazione nel container
# 1 = Invariant, 0 = Current
# ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

# Definizione della variabile d'ambiente per il path del file json locale
# ENV PRODOTTI_JSON_PATH=./wwwroot/data/prodotti.json

# Creazione della cartella per i files json
# l attributo -p permette di creare anche le cartelle genitore cioe la cartella database
RUN mkdir -p /app/data

# Definizione della variabile d'ambiente per il path del file json
ENV PRODOTTI_APP_PATH=/app/data/prodotti.json

# copia il files json di esempio
COPY ./wwwroot/data/prodotti.json /app/data/prodotti.json

# Definizione del volume per i files json
VOLUME ["/app/data"]

EXPOSE 8080

ENTRYPOINT ["dotnet", "cd-ci-web.dll"]