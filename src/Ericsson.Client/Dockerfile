FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Ericsson.Client/Ericsson.Client.csproj", "Ericsson.Client/"]
RUN dotnet restore "Ericsson.Client/Ericsson.Client.csproj"
COPY . .
WORKDIR "/src/Ericsson.Client"
RUN dotnet build "Ericsson.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ericsson.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ericsson.Client.dll"]
