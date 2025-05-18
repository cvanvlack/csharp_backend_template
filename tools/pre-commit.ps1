Write-Host 'dotnet format …'
dotnet format --verify-no-changes || exit $LASTEXITCODE
Write-Host 'dotnet build …'
dotnet build -c Release || exit $LASTEXITCODE
Write-Host 'dotnet test …'
dotnet test -c Release --no-build || exit $LASTEXITCODE
