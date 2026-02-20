#!/usr/bin/env pwsh

Write-Host "Starting Personal Portal..." -ForegroundColor Green
Write-Host ""

$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path

# Determine which PowerShell to use
$psCommand = if (Get-Command pwsh -ErrorAction SilentlyContinue) { "pwsh" } else { "powershell" }

# Start API in a new window
Write-Host "Starting API on https://localhost:7001..." -ForegroundColor Cyan
Start-Process $psCommand -ArgumentList "-NoExit", "-Command", "cd '$scriptPath\PersonalPortal.API'; dotnet run"

# Wait a bit for API to start
Start-Sleep -Seconds 3

# Start Web in a new window
Write-Host "Starting Web on https://localhost:7000..." -ForegroundColor Cyan
Start-Process $psCommand -ArgumentList "-NoExit", "-Command", "cd '$scriptPath\PersonalPortal.Web'; dotnet run"

Write-Host ""
Write-Host "Both applications are starting..." -ForegroundColor Green
Write-Host "- API: https://localhost:7001" -ForegroundColor Yellow
Write-Host "- Web: https://localhost:7000" -ForegroundColor Yellow
Write-Host "- Swagger: https://localhost:7001/swagger" -ForegroundColor Yellow
Write-Host ""
Write-Host "Press any key to exit (this won't stop the applications)..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
