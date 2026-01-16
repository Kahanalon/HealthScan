# HealthScan Israel

A mobile app that scans food products and returns a health score based on nutrition facts and ingredients analysis. Built with maximum decoupling for easy component swapping.

## Architecture

```
HealthScan/
├── src/
│   ├── HealthScan.Domain/           # Entities, interfaces (no dependencies)
│   ├── HealthScan.Application/      # Business logic, scoring engine
│   ├── HealthScan.Infrastructure/   # EF Core, external services
│   └── HealthScan.Api/              # Minimal API endpoints
├── tests/
│   └── HealthScan.Tests/            # Unit tests
├── mobile/                          # React Native app (TODO)
├── scripts/
│   └── init.sql                     # Database seed with 20 products
└── docker-compose.yml
```

## Quick Start

### Prerequisites
- .NET 8 SDK
- Docker & Docker Compose
- Node.js 18+ (for mobile)

### Run with Docker

```bash
docker compose up
```

API will be available at:
- Swagger UI: http://localhost:5000/swagger
- Health check: http://localhost:5000/health

### Run locally (development)

1. Start PostgreSQL:
```bash
docker compose up db
```

2. Run the API:
```bash
cd src/HealthScan.Api
dotnet run
```

3. Run tests:
```bash
dotnet test
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/products/{barcode}` | Get product by barcode |
| GET | `/api/v1/products/search?q=` | Search products |
| POST | `/api/v1/products/{barcode}/contribute` | Submit product data |
| POST | `/api/v1/ocr/nutrition` | Process nutrition label image |
| POST | `/api/v1/ocr/ingredients` | Process ingredients image |
| GET | `/health` | Health check |

## Decoupled Components

All major components can be swapped via interfaces:

| Interface | Default | Can swap to |
|-----------|---------|-------------|
| `IScoringEngine` | CustomScoringEngine | Any scoring algorithm |
| `IProductDataSource` | OpenFoodFactsAdapter | Any product API |
| `IOcrService` | StubOcrService | Azure Vision, Google Vision, Tesseract |
| `IProductRepository` | EfProductRepository | Any database |
| `ICacheService` | MemoryCacheService | Redis, distributed cache |
| `IIngredientAnalyzer` | RegexIngredientAnalyzer | ML-based analyzer |

## Scoring Engine

The scoring engine is pluggable. Current placeholder implementation:

- **Base score**: 100
- **Deductions**: High sugar (-20), High sodium (-20), High saturated fat (-20), Artificial sweeteners (-10), Palm oil (-5)
- **Bonuses**: High fiber (+10), High protein (+10), Low sugar (+5)
- **Grade mapping**: A (80-100), B (60-79), C (40-59), D (20-39), E (0-19)

Scoring rules can also be defined in the database (`scoring_rules` table).

## Configuration

Copy `.env.example` to `.env` and configure:

```bash
cp .env.example .env
```

## License

MIT
