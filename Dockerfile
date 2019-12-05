FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR /src/MTGinator
RUN apk add --no-cache nodejs npm
RUN dotnet publish "MTGinator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
VOLUME /db
ENV DATABASEPATH=/db/db.db
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MTGinator.dll"]
