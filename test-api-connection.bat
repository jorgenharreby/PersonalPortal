@echo off
echo Testing API Connection...
echo.

powershell -Command "try { $response = Invoke-WebRequest -Uri 'https://localhost:7001/swagger/index.html' -UseBasicParsing -SkipCertificateCheck 2>&1; if ($response.StatusCode -eq 200) { Write-Host 'SUCCESS: API is running!' -ForegroundColor Green; Write-Host 'Swagger UI is accessible at https://localhost:7001/swagger' -ForegroundColor Cyan } else { Write-Host 'WARNING: API returned status code:' $response.StatusCode -ForegroundColor Yellow } } catch { Write-Host 'ERROR: Cannot connect to API' -ForegroundColor Red; Write-Host 'Make sure the API is running. Start it with:' -ForegroundColor Yellow; Write-Host '  cd PersonalPortal.API' -ForegroundColor White; Write-Host '  dotnet run' -ForegroundColor White; Write-Host ''; Write-Host 'Error details:' $_.Exception.Message -ForegroundColor Gray }"

echo.
pause
