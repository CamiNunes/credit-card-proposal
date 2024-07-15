FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/RequestCrediCard.Worker/RequestCrediCard.Worker.csproj", "Services/RequestCrediCard.Worker/"]
RUN dotnet restore "Services/RequestCrediCard.Worker/RequestCrediCard.Worker.csproj"
COPY . .
WORKDIR "/src/Services/RequestCrediCard.Worker"
RUN dotnet build "RequestCrediCard.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RequestCrediCard.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RequestCrediCard.Worker.dll"]
