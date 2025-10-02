# PowerShell script to generate test coverage report locally

Write-Host "Running tests with coverage..." -ForegroundColor Green
dotnet test --configuration Release --collect:"XPlat Code Coverage" --results-directory ./coverage

Write-Host "`nGenerating coverage report..." -ForegroundColor Green

# Install ReportGenerator if not already installed
if (-not (Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
    Write-Host "Installing ReportGenerator tool..." -ForegroundColor Yellow
    dotnet tool install -g dotnet-reportgenerator-globaltool
}

# Generate HTML report
reportgenerator -reports:./coverage/**/coverage.cobertura.xml -targetdir:./coverage/report -reporttypes:"Html;TextSummary;Cobertura"

Write-Host "`nCoverage report generated!" -ForegroundColor Green
Write-Host "HTML Report: coverage/report/index.html" -ForegroundColor Cyan
Write-Host "`nCoverage Summary:" -ForegroundColor Yellow
Get-Content ./coverage/report/Summary.txt

# Open the report in the default browser
Start-Process ./coverage/report/index.html
