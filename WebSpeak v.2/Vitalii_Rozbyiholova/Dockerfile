FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /out
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["WebSpeakAngular/WebSpeakAngular.csproj", "WebSpeakAngular/"]
RUN dotnet restore "WebSpeakAngular/WebSpeakAngular.csproj"
COPY . .
WORKDIR /src/WebSpeakAngular
RUN dotnet build "WebSpeakAngular.csproj" -c Release -o /out/build

FROM build AS publish
RUN dotnet publish "WebSpeakAngular.csproj" -c Release -o /out/publish

FROM base AS final
WORKDIR /out
COPY --from=publish /out/publish .
ENTRYPOINT ["dotnet", "WebSpeakAngular.dll"]
