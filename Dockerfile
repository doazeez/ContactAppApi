#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ContactBookApp/ContactBookApp.csproj", "ContactBookApp/"]
COPY ["ContactBusinessLogic/ContactBusinessLogic.csproj", "ContactBusinessLogic/"]
COPY ["ContactDatabase/DatabaseAndModel.csproj", "ContactDatabase/"]
RUN dotnet restore "ContactBookApp/ContactBookApp.csproj"
COPY . .
WORKDIR "/src/ContactBookApp"
RUN dotnet build "ContactBookApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContactBookApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContactBookApp.dll"]