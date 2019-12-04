FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
WORKDIR /app/MTGinator
RUN apk add --no-cache nodejs npm
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS runtime
WORKDIR /app
VOLUME /db
ENV DATABASEPATH=/db/db.db
COPY --from=build /app/MTGinator/out ./
ENTRYPOINT ["dotnet", "MTGinator.dll"]
