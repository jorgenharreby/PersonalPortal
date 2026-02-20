@echo off
echo Starting Personal Portal...
echo.

echo Starting API on https://localhost:7001...
start "PersonalPortal API" cmd /k "cd /d %~dp0PersonalPortal.API && dotnet run --launch-profile https"

timeout /t 3 /nobreak > nul

echo Starting Web on https://localhost:7000...
start "PersonalPortal Web" cmd /k "cd /d %~dp0PersonalPortal.Web && dotnet run --launch-profile https"

echo.
echo Both applications are starting...
echo - API: https://localhost:7001
echo - Web: https://localhost:7000
echo - Swagger: https://localhost:7001/swagger
echo.
echo You can close this window now.
pause
