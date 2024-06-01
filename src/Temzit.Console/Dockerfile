FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Temzit.Console/Temzit.Console.csproj", "Temzit.Console/"]
RUN dotnet restore "Temzit.Console/Temzit.Console.csproj"
COPY . .
WORKDIR "/src/Temzit.Console"
RUN dotnet build "Temzit.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Temzit.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Temzit.Console.dll"]
