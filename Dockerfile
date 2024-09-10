FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Social_MVC/Social_MVC.csproj", "Social_MVC/"]
RUN dotnet restore "Social_MVC/Social_MVC.csproj"
COPY . .
WORKDIR "/src/Social_MVC"
RUN dotnet build "Social_MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Social_MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Social_MVC.dll"]



