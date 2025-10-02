#!/bin/bash
# Bash script to generate test coverage report locally

echo "Running tests with coverage..."
dotnet test --configuration Release --collect:"XPlat Code Coverage" --results-directory ./coverage

echo ""
echo "Generating coverage report..."

# Install ReportGenerator if not already installed
if ! command -v reportgenerator &> /dev/null; then
    echo "Installing ReportGenerator tool..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
fi

# Generate HTML report
reportgenerator -reports:./coverage/**/coverage.cobertura.xml -targetdir:./coverage/report -reporttypes:"Html;TextSummary;Cobertura"

echo ""
echo "Coverage report generated!"
echo "HTML Report: coverage/report/index.html"
echo ""
echo "Coverage Summary:"
cat ./coverage/report/Summary.txt

# Open the report in the default browser (Linux/Mac)
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    xdg-open ./coverage/report/index.html &> /dev/null || true
elif [[ "$OSTYPE" == "darwin"* ]]; then
    open ./coverage/report/index.html
fi
