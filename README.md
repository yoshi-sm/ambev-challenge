# DeveloperStore Sales API

A .NET-based API for managing sales records in the DeveloperStore application, built using Domain-Driven Design principles.

## Overview

This API provides complete CRUD operations for sales records management, with support for event publishing for key sales actions. The solution adheres to DDD principles, employing the External Identities pattern for referencing entities from other domains.

The application is built using the CQRS (Command Query Responsibility Segregation) pattern, with:
- **Write operations**: Handled by PostgreSQL for transactional integrity
- **Read operations**: Served from MongoDB for optimized query performance

The entire solution is containerized and designed to run in a Docker cluster environment.

## Features

- **Complete CRUD Operations**: Create, read, update, and delete sales records
- **Sales Information Management**:
  - Sale number
  - Sale date
  - Customer information
  - Total sale amount
  - Branch information
  - Products, quantities, and pricing
  - Discounts
  - Item totals
  - Cancellation status
- **Event Publishing**:
  - SaleCreated
  - SaleModified
  - SaleCancelled
  - ItemCancelled
- **Business Rules Implementation**:
  - 10% discount for purchases of 4+ identical items
  - 20% discount for purchases of 10-20 identical items
  - Maximum limit of 20 identical items per purchase
  - No discounts for purchases below 4 items

## Project Structure

The solution follows a clean architecture approach with distinct layers, incorporating CQRS:

```
DeveloperStore.Sales/
├── DeveloperStore.Sales.API                 # API Controllers, Startup, Configuration
├── DeveloperStore.Sales.Application         # Application Services, DTOs, Validators
│   ├── Commands                             # Write operations (PostgreSQL)
│   └── Queries                              # Read operations (MongoDB)
├── DeveloperStore.Sales.Domain              # Domain Entities, Value Objects, Events, Interfaces
├── DeveloperStore.Sales.Infrastructure      # External Services, Persistence, Event Publishing
│   ├── PostgreSQL                           # Write model persistence
│   ├── MongoDB                              # Read model persistence                    
├── DeveloperStore.Sales.Tests               # Unit and Integration Tests
└── docker                                   # Docker configuration files
    ├── docker-compose.yml                   # Local development environment

```

## Domain Model

The core domain model includes:

- **Sale**: The aggregate root representing a sales transaction
- **SaleItem**: Value object representing individual items in a sale
- **ExternalEntities**: References to Customer, Branch, and Product entities from other domains

## API Endpoints

### Sales

- `GET /api/sales` - List all sales with pagination
- `GET /api/sales/{id}` - Get a specific sale by ID
- `POST /api/sales` - Create a new sale
- `PUT /api/sales/update` - Update an existing sale
- `PUT /api/sales/{id}/cancel` - Mark a sale as cancelled
- `PUT /api/sales/{id}/item/cancel` - Mark a sale as cancelled

## Events

The API publishes the following domain events:

- **SaleCreated**: Triggered when a new sale is created
- **SaleModified**: Triggered when a sale is modified
- **SaleCancelled**: Triggered when a sale is cancelled
- **ItemCancelled**: Triggered when a specific item in a sale is cancelled

Events are currently logged to the application log but can be easily configured to publish to a message broker.

## Business Rules Implementation

The API enforces the following business rules:

1. **Quantity-Based Discounts**:
   - 4+ identical items: 10% discount applied automatically
   - 10-20 identical items: 20% discount applied automatically

2. **Purchase Restrictions**:
   - Maximum limit: 20 items per product
   - No discounts for quantities below 4 items

## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET 8.0 SDK or later (for local development only)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yoshi-sm/ambev-challenge.git
cd ambev-challenge
```

2. Run with Docker Compose (development environment):
```bash
docker-compose -p ambev up -d
```

The API will be available at `http://localhost:8080`.

3. For Kubernetes deployment:
```bash
kubectl apply -f ./docker/kubernetes/
```

### Local Development (Optional)

If you want to develop locally without Docker:

1. Install PostgreSQL and MongoDB
2. Update connection strings in appsettings.json
3. Run the application:
```bash
dotnet run --project Ambev.DeveloperEvaluation.WebAPI.csproj
```

## Configuration

Configuration settings can be adjusted in the `appsettings.json` file or through environment variables:

```json
{
  "ConnectionStrings": {
    "PostgreSQLConnection": "Host=ambev_developer_evaluation_database;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n;"
  },
  "MongoDbSettings": {
    "ConnectionString": "mongodb://developer:ev%40luAt10n@ambev_developer_evaluation_nosql:27017/admin?authSource=admin",
    "DatabaseName": "developer_evaluation"
  },
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
    }
  },
  "AllowedHosts": "*"
}

```

When running in Docker, these settings are configured through environment variables in the docker-compose.yml and Kubernetes manifest files.

## Testing

Run the test suite using:

```bash
dotnet test
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.
