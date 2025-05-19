# 1. Verify that everything is already formatted

Write-Host "Verifying code formatting…"
dotnet format CsharpBackendService.sln --verify-no-changes
if ($LASTEXITCODE -ne 0) {
    # 2a. If formatting violations exist, fix them…
    Write-Warning "Formatting violations detected. Applying auto-format…"
    dotnet format CsharpBackendService.sln

    # 2b. Then abort the commit so the user can review & stage the changes
    Write-Error "Formatting changes have been applied. Please review and 'git add' them before committing."
    exit 1
}

Write-Host "dotnet build"
dotnet build -c Release
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "dotnet test"
dotnet test -c Release --no-build
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
