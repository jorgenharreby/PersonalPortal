@echo off
echo ========================================
echo  Personal Portal Startup
echo ========================================
echo.

echo [1/3] Starting API on https://localhost:7001...
start "PersonalPortal API" cmd /k "cd /d %~dp0PersonalPortal.API && echo Starting API... && dotnet run --launch-profile https"

echo [2/3] Waiting for API to start (10 seconds)...
timeout /t 10 /nobreak > nul

echo [3/3] Starting Web on https://localhost:7000...
start "PersonalPortal Web" cmd /k "cd /d %~dp0PersonalPortal.Web && echo Starting Web App... && dotnet run --launch-profile https"

echo.
echo ========================================
echo  Both applications are starting!
echo ========================================
echo.
echo  - API:     https://localhost:7001
echo  - Swagger: https://localhost:7001/swagger
echo  - Web:     https://localhost:7000
echo.
echo  Default Login:
echo  - Username: harreby
echo  - Password: fishalot
echo.
echo  Wait about 10-15 seconds for both to fully start,
echo  then open your browser to https://localhost:7000
echo.
echo ========================================
echo  You can close this window now.
echo  To stop the applications, close the
echo  "PersonalPortal API" and "PersonalPortal Web" windows.
echo ========================================
echo.
pause
