FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SecretsSharing.Api/SecretsSharing.Api.csproj", "SecretsSharing.Api/"]
COPY ["Logic/Logic.csproj", "Logic/"]
COPY ["Dal/Dal.csproj", "Dal/"]
RUN dotnet restore "SecretsSharing.Api/SecretsSharing.Api.csproj"
COPY . .
WORKDIR "/src/SecretsSharing.Api"
RUN dotnet build "SecretsSharing.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SecretsSharing.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecretsSharing.Api.dll"]
