FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Ecommerce.sln ./
COPY Ecommerce.Domain/*.csproj ./Ecommerce.Domain/
COPY Ecommerce.Application/*.csproj ./Ecommerce.Application/
COPY Ecommerce.Infrastructure/*.csproj ./Ecommerce.Infrastructure/
COPY Ecommerce.WebApi/*.csproj ./Ecommerce.WebApi/
COPY Ecommerce.Web/*.csproj ./Ecommerce.Web/
COPY Ecommerce.Test/*.csproj ./Ecommerce.Test/
RUN dotnet restore Ecommerce.sln

COPY . .

FROM build AS publish-webapi
RUN dotnet publish Ecommerce.WebApi/Ecommerce.WebApi.csproj -c Release -o /app/publish --no-restore

FROM build AS publish-web
RUN dotnet publish Ecommerce.Web/Ecommerce.Web.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS webapi
WORKDIR /app
COPY --from=publish-webapi /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet","Ecommerce.WebApi.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS web
WORKDIR /app
COPY --from=publish-web /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet","Ecommerce.Web.dll"]

