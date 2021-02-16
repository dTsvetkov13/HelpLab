#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Microservices.Answers/Microservices.Answers.csproj", "Microservices.Answers/"]
RUN dotnet restore "Microservices.Answers/Microservices.Answers.csproj"
COPY . .
WORKDIR "/src/Microservices.Answers"
RUN dotnet build "Microservices.Answers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservices.Answers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Answers.dll"]