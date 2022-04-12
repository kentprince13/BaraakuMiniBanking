#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MiniBanking/MiniBanking.API/MiniBanking.API.csproj", "MiniBanking/MiniBanking.API/"]
COPY ["MiniBanking/MiniBanking.Core/MiniBanking.Core.csproj", "MiniBanking/MiniBanking.Core/"]
COPY ["MiniBanking/MiniBanking.Infrastructure/MiniBanking.Infrastructure.csproj", "MiniBanking/MiniBanking.Infrastructure/"]
COPY ["MiniBanking/MiniBanking.Domain/MiniBanking.Domain.csproj", "MiniBanking/MiniBanking.Domain/"]
RUN dotnet restore "MiniBanking/MiniBanking.API/MiniBanking.API.csproj"
COPY . .
WORKDIR "/src/MiniBanking/MiniBanking.API"
RUN dotnet build "MiniBanking.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiniBanking.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniBanking.API.dll"]
