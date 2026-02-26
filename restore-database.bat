@echo off
REM ========================================
REM Personal Portal Database Restore Script
REM ========================================

echo.
echo ========================================
echo Personal Portal Database Restore
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

REM Check if a backup file was specified as argument
if "%~1"=="" (
    echo No backup file specified. Will use latest backup.
    echo.
    powershell -ExecutionPolicy Bypass -File "%~dp0restore-database.ps1"
) else (
    echo Using backup file: %~1
    echo.
    powershell -ExecutionPolicy Bypass -File "%~dp0restore-database.ps1" -BackupFile "%~1"
)

if %errorlevel% equ 0 (
    echo.
    echo Restore completed successfully!
    echo.
) else (
    echo.
    echo Restore failed! See errors above.
    echo.
)

pause
