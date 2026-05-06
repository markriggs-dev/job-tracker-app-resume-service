FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/ResumeService.Api/ResumeService.Api.csproj", "src/ResumeService.Api/"]
COPY ["src/ResumeService.Core/ResumeService.Core.csproj", "src/ResumeService.Core/"]
COPY ["src/ResumeService.Infrastructure/ResumeService.Infrastructure.csproj", "src/ResumeService.Infrastructure/"]
RUN dotnet restore "src/ResumeService.Api/ResumeService.Api.csproj"

COPY . .
RUN dotnet publish "src/ResumeService.Api/ResumeService.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ResumeService.Api.dll"]
