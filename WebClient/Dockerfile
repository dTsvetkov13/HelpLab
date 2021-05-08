FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WebClient/WebClient.csproj", "WebClient/"]

RUN dotnet restore "WebClient/WebClient.csproj"
COPY . .
WORKDIR "/src/WebClient"
RUN dotnet build "WebClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebClient.dll"]