# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Architecture

This is a full-stack usufruct calculator application with:

### Backend (.NET 8 API)
- **Location**: `src/UsufructCalculator.Api/`
- **Framework**: ASP.NET Core 8.0 Minimal API
- **Features**:
  - Swagger/OpenAPI documentation enabled in development
  - Currently contains a placeholder WeatherForecast endpoint

### Frontend (Angular)
- **Location**: `src/UsufructCalculator.Web/`
- **Framework**: Angular 20.3 with SSR (Server-Side Rendering)
- **Entry Points**:
  - Browser: `src/main.ts`
  - Server: `src/main.server.ts`
  - SSR: `src/server.ts`
- **Architecture**: Signal-based components (new Angular pattern)

### Test Projects
- **API Tests**: `tests/UsufructCalculator.Api.Tests/` using xUnit
- **Web Tests**: Angular Karma/Jasmine tests in `src/UsufructCalculator.Web/src/`

## Common Commands

### .NET API (run from repository root)
```bash
# Build the solution
dotnet build

# Run the API (starts on https://localhost:5001 by default)
dotnet run --project src/UsufructCalculator.Api/UsufructCalculator.Api.csproj

# Run API tests
dotnet test tests/UsufructCalculator.Api.Tests/UsufructCalculator.Api.Tests.csproj

# Run all tests
dotnet test
```

### Angular Frontend (run from src/UsufructCalculator.Web/)
```bash
# Install dependencies
npm install

# Start development server
npm start
# or
ng serve

# Build for production
npm run build
# or
ng build

# Run tests
npm test
# or
ng test

# Run tests with code coverage
ng test --code-coverage

# Watch mode for development
npm run watch
# or
ng build --watch --configuration development

# Run SSR server (after build)
npm run serve:ssr:UsufructCalculator.Web
```

## Code Style

### Angular
- Uses Prettier with 100 character line width
- Single quotes for strings
- Angular parser for HTML templates

## Project Structure

- Solution file: `UsufructCalculator.sln`
- Source code in `src/` folder
- Tests in `tests/` folder
- Angular app is self-contained with its own `package.json` and `node_modules`
