FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FDRWebsite/Server/", "Server/"]
COPY ["FDRWebsite/Client/", "Client/"]
COPY ["FDRWebsite/Shared/", "Shared/"]
RUN dotnet restore "Server/FDRWebsite.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "FDRWebsite.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FDRWebsite.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FDRWebsite.Server.dll"]