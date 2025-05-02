FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OnlineShop/OnlineShop.csproj", "OnlineShop/"]
RUN dotnet restore "OnlineShop/OnlineShop.csproj"
COPY . .
WORKDIR "/src/OnlineShop"
RUN dotnet publish "OnlineShop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.dll"]
