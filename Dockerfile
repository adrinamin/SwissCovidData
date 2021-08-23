#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CovidData.Web2/CovidData.Web2.csproj", "CovidData.Web2/"]
RUN dotnet restore "CovidData.Web2/CovidData.Web2.csproj"
COPY . .
WORKDIR "/src/CovidData.Web2"
RUN dotnet build "CovidData.Web2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CovidData.Web2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CovidData.Web2.dll"]