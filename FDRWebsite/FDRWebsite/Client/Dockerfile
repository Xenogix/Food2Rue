FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
COPY . ./
RUN dotnet publish -c Release -o output

FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build-env /app/output/wwwroot .
COPY FDRWebsite/Client/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
EXPOSE 443