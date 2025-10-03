# Usufruct Calculator

A full-stack application for calculating usufruct (vruchtgebruik) values based on age, gender, and property value.

## Project Structure

```
UsufructCalculator/
├── src/
│   ├── UsufructCalculator.Api/          # .NET 8 API
│   │   ├── Controllers/                 # API endpoints
│   │   ├── Services/                    # Business logic
│   │   │   └── CalculationStrategies/   # Strategy pattern for calculations
│   │   ├── Models/                      # Request/Response models
│   │   ├── Middleware/                  # API key authentication
│   │   └── Extensions/                  # Service/middleware extensions
│   │
│   └── UsufructCalculator.Web/          # Angular 20 Frontend
│       └── src/
│           └── app/
│               ├── app.component.*      # Shell component
│               ├── features/            # Feature modules
│               │   └── calculator/      # Calculator feature
│               ├── core/                # Singleton services
│               │   └── interceptors/    # HTTP interceptors
│               └── shared/              # Shared resources
│                   ├── models/          # TypeScript models
│                   └── services/        # Shared services
│
├── tests/
│   └── UsufructCalculator.Api.Tests/   # xUnit tests
│
└── UsufructCalculator.sln               # Solution file
```

## Dependencies

### Backend (.NET 8)

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.AspNetCore.OpenApi | 8.0.20 | OpenAPI/Swagger support |
| Swashbuckle.AspNetCore | 6.6.2 | API documentation |
| xUnit | 2.5.3 | Testing framework |
| Moq | 4.20.72 | Mocking framework |
| FluentAssertions | 6.12.2 | Assertion library |

### Frontend (Angular 20)

| Package | Version | Purpose |
|---------|---------|---------|
| @angular/core | 20.3.2 | Angular framework |
| @angular/forms | 20.3.2 | Form handling |
| @angular/ssr | 20.3.3 | Server-side rendering |
| @types/node | 20.19.0 | Node.js type definitions |
| rxjs | 7.8.0 | Reactive programming |
| angular-eslint | 20.3.0 | Code linting |

**Note:** All dependencies are pinned to exact versions for reproducible builds.

## Running the Application Locally

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/)
- npm (comes with Node.js)

### Backend API

```bash
# From repository root
dotnet run --project src/UsufructCalculator.Api/UsufructCalculator.Api.csproj
```

The API will start at:
- **HTTPS:** https://localhost:5184
- **HTTP:** http://localhost:5184
- **Swagger UI:** https://localhost:5184/swagger (development only)

### Frontend (Angular)

```bash
# Navigate to frontend directory
cd src/UsufructCalculator.Web

# Install dependencies (first time only)
npm install

# Start development server
npm start
```

The application will be available at: **http://localhost:4200**

### API Key Configuration

The API requires an API key for authentication. Configure it in:

**Development:** `src/UsufructCalculator.Api/appsettings.Development.json`
```json
{
  "ApiKey": "dev-api-key-replace-in-production"
}
```

**Frontend:** `src/UsufructCalculator.Web/src/environments/environment.ts`
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5184/api',
  apiKey: 'dev-api-key-replace-in-production'
};
```

## Running Tests

### Backend Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/UsufructCalculator.Api.Tests/UsufructCalculator.Api.Tests.csproj

# Run with code coverage (windows)
'./generate-coverage.ps1' 

# Run with code coverage (Mac/Linux)
'./generate-coverage.sh'
```

### Frontend Tests

```bash
# Navigate to frontend directory
cd src/UsufructCalculator.Web

# Run tests (single run)
npm test -- --no-watch --browsers=ChromeHeadless

# Run tests with code coverage
npm test -- --no-watch --code-coverage --browsers=ChromeHeadless

# Run tests in watch mode (development)
npm test
```

### Linting

```bash
# Frontend linting
cd src/UsufructCalculator.Web
npm run lint

# Auto-fix linting issues
npm run lint -- --fix
```

## Updating Dependencies

### Frontend Dependencies

```bash
cd src/UsufructCalculator.Web

# Check for outdated packages
npm outdated

# Update specific package
npm install @angular/core@20.4.0

# Update all Angular packages to latest compatible version
ng update @angular/core @angular/cli
```

### Backend Dependencies

```bash
# Check for outdated packages
dotnet list package --outdated

# Update specific package
dotnet add src/UsufructCalculator.Api package Microsoft.AspNetCore.OpenApi --version 8.0.21

# Update all packages in solution
dotnet restore
```

**Important:** After updating dependencies, ensure to:
1. Update version in `package.json` or `.csproj` to the exact version (no `^` or `~`)
2. Run all tests to verify compatibility
3. Update this README if major versions change

## Adding a New Calculation Strategy

The API uses the Strategy Pattern for different calculation methods. Here's how to add a new one:

### Step 1: Create the Strategy Class

Create a new file in `src/UsufructCalculator.Api/Services/CalculationStrategies/`:

```csharp
using UsufructCalculator.Api.Models;
using UsufructCalculator.Api.Services.CalculationStrategies.Models;

namespace UsufructCalculator.Api.Services.CalculationStrategies;

/// <summary>
/// Strategy for [Your Method Name] calculation.
/// </summary>
public class YourNewCalculationStrategy : ICalculationStrategy
{
    public CalculationMethod SupportedMethod => CalculationMethod.YourNewMethod;

    public UsufructResult Calculate(UsufructRequest request)
    {
        // Implement your calculation logic here
        decimal factor = CalculateYourFactor(request.Age, request.Gender);
        decimal calculatedValue = request.Amount * factor;

        return new UsufructResult
        {
            OriginalValue = request.Amount,
            Factor = factor,
            CalculatedValue = calculatedValue
        };
    }

    private decimal CalculateYourFactor(int age, Gender gender)
    {
        // Your factor calculation logic
        return 0.5m; // Example
    }
}
```

### Step 2: Add to Calculation Method Enum

Update `src/UsufructCalculator.Api/Models/Enums/CalculationMethod.cs`:

```csharp
public enum CalculationMethod
{
    EenLeven = 0,
    YourNewMethod = 1  // Add your new method
}
```

### Step 3: Register the Strategy

Update `src/UsufructCalculator.Api/Extensions/ServiceCollectionExtensions.cs`:

```csharp
public static IServiceCollection AddApiServices(this IServiceCollection services)
{
    // ... existing code ...

    // Register calculation strategies
    services.AddTransient<ICalculationStrategy, EenLevenCalculationStrategy>();
    services.AddTransient<ICalculationStrategy, YourNewCalculationStrategy>(); // Add this

    // ... rest of code ...
}
```

### Step 4: Update Frontend

Add the new method to `src/UsufructCalculator.Web/src/app/shared/models/usufruct.models.ts`:

```typescript
export enum CalculationMethod {
  EenLeven = 0,
  YourNewMethod = 1,  // Add your new method
}
```

Update the dropdown in `src/UsufructCalculator.Web/src/app/features/calculator/calculator.component.ts`:

```typescript
protected readonly calculationMethods = [
  { value: CalculationMethod.EenLeven, label: 'Eén Leven' },
  { value: CalculationMethod.YourNewMethod, label: 'Your Method Name' } // Add this
];
```

### Step 5: Write Tests

Create tests in `tests/UsufructCalculator.Api.Tests/Services/CalculationStrategies/`:

```csharp
public class YourNewCalculationStrategyTests
{
    [Fact]
    public void Calculate_ShouldReturnCorrectResult()
    {
        // Arrange
        var strategy = new YourNewCalculationStrategy();
        var request = new UsufructRequest
        {
            Amount = 100000,
            Age = 50,
            Gender = Gender.Male,
            CalculationMethod = CalculationMethod.YourNewMethod
        };

        // Act
        var result = strategy.Calculate(request);

        // Assert
        result.Should().NotBeNull();
        result.OriginalValue.Should().Be(100000);
        result.Factor.Should().BeGreaterThan(0);
    }
}
```

## Additional Information

### Code Coverage

The project maintains high code coverage:
- **Backend:** Check coverage with `dotnet test` and view reports in `coverage/`
- **Frontend:** 100% statement/function/line coverage, 90.9% branch coverage

### Architecture Patterns

- **Backend:** Strategy Pattern for calculations, Dependency Injection, Middleware pipeline
- **Frontend:** Feature modules, Reactive forms, Signal-based state management

### API Documentation

When running in development mode, access the Swagger UI at:
- https://localhost:5184/swagger

### Contributing

1. Create a feature branch
2. Make your changes
3. Ensure all tests pass
4. Run linting
5. Update documentation if needed
6. Create a pull request

---

**Built with .NET 8 and Angular 20**
