FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ResumeService.Api/ResumeService.Api.csproj", "src/ResumeService.Api/"]
COPY ["src/ResumeService.Core/ResumeService.Core.csproj", "src/ResumeService.Core/"]
COPY ["src/ResumeService.Infrastructure/ResumeService.Infrastructure.csproj", "src/ResumeService.Infrastructure/"]
RUN dotnet restore "src/ResumeService.Api/ResumeService.Api.csproj"
COPY . .
RUN dotnet build "src/ResumeService.Api/ResumeService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/ResumeService.Api/ResumeService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResumeService.Api.dll"]
