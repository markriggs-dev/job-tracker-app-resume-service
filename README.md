# job-tracker-app-resume-service

Manages versioned resume uploads linked to job requisitions with blob storage integration.

## Technology
- .NET 8 Web API
- C#
- PostgreSQL
- Docker

## Getting started

```bash
dotnet restore
dotnet build
dotnet run --project src/ResumeService.Api
```

## Running with Docker

```bash
docker build -t job-tracker-app-resume-service .
docker run -p 5004:5004 job-tracker-app-resume-service
```

## Environment variables

| Variable | Description |
|----------|-------------|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string |
| `Auth0__Domain` | Auth0 domain |
| `Auth0__Audience` | Auth0 API audience |
| `Kafka__BootstrapServers` | Kafka broker address |

## Project structure

```
src/
  ResumeService.Api/          # Web API entry point, controllers, middleware
  ResumeService.Core/         # Domain models, interfaces, business logic
  ResumeService.Infrastructure/ # Data access, Kafka, external integrations
tests/
  ResumeService.UnitTests/
  ResumeService.IntegrationTests/
```
