@echo off
REM ========================================
REM Personal Portal Database Backup Script
REM ========================================

echo.
echo ========================================
echo Personal Portal Database Backup
echo ========================================
echo.

REM Check if PowerShell is available
where powershell >nul 2>nul
if %errorlevel% neq 0 (
    echo ERROR: PowerShell is not installed or not in PATH
    echo Please install PowerShell to use this script
    pause
    exit /b 1
)

REM Run the PowerShell backup script
powershell -ExecutionPolicy Bypass -File "%~dp0backup-database.ps1"

if %errorlevel% equ 0 (
    echo.
    echo Backup completed successfully!
    echo.
) else (
    echo.
    echo Backup failed! See errors above.
    echo.
)

pause
