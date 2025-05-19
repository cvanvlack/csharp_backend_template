Write-Host "dotnet format to fix formatting issues"
dotnet format style
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "dotnet format to verify nothing left to format"
dotnet format CsharpBackendService.sln --verify-no-changes
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "dotnet build"
dotnet build -c Release
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "dotnet test"
dotnet test -c Release --no-build
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
