# Personal Portal Database Restore Script
# Restores data from a backup SQL file

param(
    [Parameter(Mandatory=$false)]
    [string]$BackupFile,
    [string]$ServerInstance = "(localdb)\mssqllocaldb",
    [string]$Database = "PersonalPortal"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Personal Portal Database Restore Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if sqlcmd is available
try {
    $null = sqlcmd -?
} catch {
    Write-Host "ERROR: sqlcmd is not installed or not in PATH" -ForegroundColor Red
    Write-Host "Please install SQL Server Command Line Utilities" -ForegroundColor Yellow
    exit 1
}

# If no backup file specified, find the latest one
if (-not $BackupFile) {
    Write-Host "No backup file specified. Looking for latest backup..." -ForegroundColor Yellow
    
    $backupFiles = Get-ChildItem -Path "Database" -Filter "BackupData_*.sql" -ErrorAction SilentlyContinue | Sort-Object CreationTime -Descending
    
    if ($backupFiles.Count -eq 0) {
        Write-Host ""
        Write-Host "ERROR: No backup files found in Database folder" -ForegroundColor Red
        Write-Host ""
        Write-Host "Please run backup-database.bat first to create a backup" -ForegroundColor Yellow
        exit 1
    }
    
    $BackupFile = $backupFiles[0].FullName
    Write-Host "Found latest backup: $($backupFiles[0].Name)" -ForegroundColor Green
}

# Check if backup file exists
if (!(Test-Path $BackupFile)) {
    Write-Host ""
    Write-Host "ERROR: Backup file not found: $BackupFile" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Restore file: $BackupFile" -ForegroundColor Cyan
Write-Host "Server: $ServerInstance" -ForegroundColor Gray
Write-Host "Database: $Database" -ForegroundColor Gray
Write-Host ""

# Confirm restore
Write-Host "??  WARNING: This will add data to the database!" -ForegroundColor Yellow
Write-Host "   Existing data will NOT be deleted, but duplicates may occur." -ForegroundColor Yellow
Write-Host ""
$confirmation = Read-Host "Do you want to continue? (yes/no)"

if ($confirmation -ne "yes") {
    Write-Host ""
    Write-Host "Restore cancelled by user" -ForegroundColor Yellow
    exit 0
}

Write-Host ""
Write-Host "Restoring data..." -ForegroundColor Green

# Run the restore
try {
    sqlcmd -S $ServerInstance -d $Database -i $BackupFile -b
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "========================================" -ForegroundColor Green
        Write-Host "? Restore completed successfully!" -ForegroundColor Green
        Write-Host "========================================" -ForegroundColor Green
        Write-Host ""
    } else {
        Write-Host ""
        Write-Host "? Restore failed! See errors above." -ForegroundColor Red
        Write-Host ""
        exit 1
    }
} catch {
    Write-Host ""
    Write-Host "? Error running restore: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
