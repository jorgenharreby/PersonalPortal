# Personal Portal Database Backup Script
# Creates SQL INSERT statements for all data in the database

param(
    [string]$OutputFile = "Database\BackupData_$(Get-Date -Format 'yyyyMMdd_HHmmss').sql",
    [string]$ServerInstance = "(localdb)\mssqllocaldb",
    [string]$Database = "PersonalPortal"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Personal Portal Database Backup Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if sqlcmd is available
try {
    $null = sqlcmd -? 2>$null
    if ($LASTEXITCODE -ne 0) { throw }
} catch {
    Write-Host "ERROR: sqlcmd is not installed or not in PATH" -ForegroundColor Red
    Write-Host "Please install SQL Server Command Line Utilities" -ForegroundColor Yellow
    exit 1
}

Write-Host "Creating backup file: $OutputFile" -ForegroundColor Green
Write-Host "Server: $ServerInstance" -ForegroundColor Gray
Write-Host "Database: $Database" -ForegroundColor Gray
Write-Host ""

# Create output directory if it doesn't exist
$outputDir = Split-Path -Parent $OutputFile
if ($outputDir -and !(Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

# Function to execute SQL and get results as objects
function Invoke-SqlQuery {
    param($Query)
    
    $tempFile = [System.IO.Path]::GetTempFileName()
    try {
        # Run sqlcmd and output to temp file with tab-separated values
        sqlcmd -S $ServerInstance -d $Database -Q $Query -s "`t" -W -h -1 -o $tempFile
        
        if ($LASTEXITCODE -ne 0) {
            return @()
        }
        
        # Read and parse results
        $lines = Get-Content $tempFile | Where-Object { $_.Trim() -ne "" }
        if ($lines.Count -lt 2) { return @() }
        
        # First line is headers
        $headers = $lines[0] -split "`t"
        
        # Rest are data rows
        $results = @()
        for ($i = 1; $i -lt $lines.Count; $i++) {
            $values = $lines[$i] -split "`t"
            $obj = New-Object PSObject
            for ($j = 0; $j -lt [Math]::Min($headers.Count, $values.Count); $j++) {
                $obj | Add-Member -MemberType NoteProperty -Name $headers[$j].Trim() -Value $values[$j].Trim()
            }
            $results += $obj
        }
        
        return $results
    } finally {
        if (Test-Path $tempFile) {
            Remove-Item $tempFile -Force
        }
    }
}

# Function to escape single quotes in SQL strings
function Escape-SqlString($value) {
    if ($null -eq $value -or [string]::IsNullOrWhiteSpace($value) -or $value -eq "NULL") { 
        return "NULL" 
    }
    return "N'" + $value.ToString().Trim().Replace("'", "''") + "'"
}

# Start building the backup SQL file
$sqlContent = @"
-- ========================================
-- Personal Portal Database Backup
-- Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')
-- ========================================
-- 
-- This script contains INSERT statements for all data in the PersonalPortal database.
-- Run this script on an initialized database (after running InitializeDatabase.sql)
-- to restore your data.
--
-- IMPORTANT: This will ADD data to existing tables. If you want a clean restore,
-- drop and recreate the database first, then run InitializeDatabase.sql, then this script.
--

USE PersonalPortal;
GO

PRINT 'Starting data restore...';
GO

-- Disable constraints for faster insertion
ALTER TABLE ChecklistItems NOCHECK CONSTRAINT ALL;
ALTER TABLE Pictures NOCHECK CONSTRAINT ALL;
GO

"@

Write-Host "Backing up data from tables..." -ForegroundColor Yellow

# Backup Users table (excluding passwords for security)
try {
    Write-Host "  - Backing up Users table..." -ForegroundColor Cyan
    $users = Invoke-SqlQuery "SELECT Id, Username, Role, DisplayName FROM Users ORDER BY Id"
    
    $sqlContent += "`r`n-- ========================================`r`n"
    $sqlContent += "-- Users Table`r`n"
    $sqlContent += "-- ========================================`r`n"
    $sqlContent += "PRINT 'Restoring Users...';`r`n`r`n"
    
    if ($users.Count -gt 0) {
        $sqlContent += "SET IDENTITY_INSERT Users ON;`r`n"
        foreach ($user in $users) {
            $id = $user.Id
            $username = Escape-SqlString $user.Username
            $role = Escape-SqlString $user.Role
            $displayName = Escape-SqlString $user.DisplayName
            
            $sqlContent += "IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = $id)`r`n"
            $sqlContent += "    INSERT INTO Users (Id, Username, Password, Role, DisplayName)`r`n"
            $sqlContent += "    VALUES ($id, $username, N'CHANGE_ME', $role, $displayName);`r`n"
        }
        $sqlContent += "SET IDENTITY_INSERT Users OFF;`r`n"
        $sqlContent += "GO`r`n"
        Write-Host "    ? $($users.Count) users backed up" -ForegroundColor Green
    } else {
        Write-Host "    ? No users to backup" -ForegroundColor Gray
    }
} catch {
    Write-Host "    ? Error backing up Users: $($_.Exception.Message)" -ForegroundColor Red
}

# Backup TextNotes table
try {
    Write-Host "  - Backing up TextNotes table..." -ForegroundColor Cyan
    $notes = Invoke-SqlQuery "SELECT CAST(Id AS NVARCHAR(50)) as Id, Name, Content, CONVERT(VARCHAR(23), Created, 121) as Created, CONVERT(VARCHAR(23), Updated, 121) as Updated FROM TextNotes ORDER BY Created"
    
    $sqlContent += "`r`n-- ========================================`r`n"
    $sqlContent += "-- TextNotes Table`r`n"
    $sqlContent += "-- ========================================`r`n"
    $sqlContent += "PRINT 'Restoring TextNotes...';`r`n`r`n"
    
    if ($notes.Count -gt 0) {
        foreach ($note in $notes) {
            $id = "'" + $note.Id + "'"
            $name = Escape-SqlString $note.Name
            $content = Escape-SqlString $note.Content
            $created = "'" + $note.Created + "'"
            $updated = "'" + $note.Updated + "'"
            
            $sqlContent += "IF NOT EXISTS (SELECT 1 FROM TextNotes WHERE Id = $id)`r`n"
            $sqlContent += "    INSERT INTO TextNotes (Id, Name, Content, Created, Updated)`r`n"
            $sqlContent += "    VALUES ($id, $name, $content, $created, $updated);`r`n"
        }
        $sqlContent += "GO`r`n"
        Write-Host "    ? $($notes.Count) text notes backed up" -ForegroundColor Green
    } else {
        Write-Host "    ? No text notes to backup" -ForegroundColor Gray
    }
} catch {
    Write-Host "    ? Error backing up TextNotes: $($_.Exception.Message)" -ForegroundColor Red
}

# Backup Checklists
try {
    Write-Host "  - Backing up Checklists table..." -ForegroundColor Cyan
    $checklists = Invoke-SqlQuery "SELECT CAST(Id AS NVARCHAR(50)) as Id, Name, Type, CONVERT(VARCHAR(23), Created, 121) as Created, CONVERT(VARCHAR(23), Updated, 121) as Updated FROM Checklists ORDER BY Created"
    
    $sqlContent += "`r`n-- ========================================`r`n"
    $sqlContent += "-- Checklists Table`r`n"
    $sqlContent += "-- ========================================`r`n"
    $sqlContent += "PRINT 'Restoring Checklists...';`r`n`r`n"
    
    if ($checklists.Count -gt 0) {
        foreach ($checklist in $checklists) {
            $id = "'" + $checklist.Id + "'"
            $name = Escape-SqlString $checklist.Name
            $type = Escape-SqlString $checklist.Type
            $created = "'" + $checklist.Created + "'"
            $updated = "'" + $checklist.Updated + "'"
            
            $sqlContent += "IF NOT EXISTS (SELECT 1 FROM Checklists WHERE Id = $id)`r`n"
            $sqlContent += "    INSERT INTO Checklists (Id, Name, Type, Created, Updated)`r`n"
            $sqlContent += "    VALUES ($id, $name, $type, $created, $updated);`r`n"
        }
        $sqlContent += "GO`r`n"
        Write-Host "    ? $($checklists.Count) checklists backed up" -ForegroundColor Green
    } else {
        Write-Host "    ? No checklists to backup" -ForegroundColor Gray
    }
} catch {
    Write-Host "    ? Error backing up Checklists: $($_.Exception.Message)" -ForegroundColor Red
}

# Backup ChecklistItems
try {
    Write-Host "  - Backing up ChecklistItems table..." -ForegroundColor Cyan
    $itemsQuery = "SELECT Id, CAST(ChecklistId AS NVARCHAR(50)) as ChecklistId, ISNULL(ItemName, '(No Name)') as ItemName, ISNULL(Description, '') as Description, ISNULL(ItemGroup, '') as ItemGroup FROM ChecklistItems ORDER BY ChecklistId, Id"
    $items = Invoke-SqlQuery $itemsQuery
    
    $sqlContent += "`r`n-- ========================================`r`n"
    $sqlContent += "-- ChecklistItems Table`r`n"
    $sqlContent += "-- ========================================`r`n"
    $sqlContent += "PRINT 'Restoring ChecklistItems...';`r`n`r`n"
    
    if ($items -and $items.Count -gt 0) {
        $sqlContent += "SET IDENTITY_INSERT ChecklistItems ON;`r`n"
        $itemCount = 0
        foreach ($item in $items) {
            try {
                $id = $item.Id
                $checklistId = "'" + $item.ChecklistId.ToString().Trim() + "'"
                
                # Get string values safely
                $itemNameStr = $item.ItemName.ToString().Trim()
                $descStr = $item.Description.ToString().Trim()
                $groupStr = $item.ItemGroup.ToString().Trim()
                
                # Build SQL values - don't call function on empty strings
                if ([string]::IsNullOrWhiteSpace($itemNameStr)) {
                    $itemName = "N'(No Name)'"
                } else {
                    $itemName = "N'" + $itemNameStr.Replace("'", "''") + "'"
                }
                
                if ([string]::IsNullOrWhiteSpace($descStr)) {
                    $description = "N''"
                } else {
                    $description = "N'" + $descStr.Replace("'", "''") + "'"
                }
                
                if ([string]::IsNullOrWhiteSpace($groupStr)) {
                    $itemGroup = "NULL"
                } else {
                    $itemGroup = "N'" + $groupStr.Replace("'", "''") + "'"
                }
                
                $sqlContent += "IF NOT EXISTS (SELECT 1 FROM ChecklistItems WHERE Id = $id)`r`n"
                $sqlContent += "    INSERT INTO ChecklistItems (Id, ChecklistId, ItemName, Description, ItemGroup)`r`n"
                $sqlContent += "    VALUES ($id, $checklistId, $itemName, $description, $itemGroup);`r`n"
                $itemCount++
            } catch {
                Write-Host "    ? Warning: Skipping item $($item.Id): $($_.Exception.Message)" -ForegroundColor Yellow
            }
        }
        $sqlContent += "SET IDENTITY_INSERT ChecklistItems OFF;`r`n"
        $sqlContent += "GO`r`n"
        Write-Host "    ? $itemCount checklist items backed up" -ForegroundColor Green
    } else {
        Write-Host "    ? No checklist items to backup" -ForegroundColor Gray
    }
} catch {
    Write-Host "    ? Error backing up ChecklistItems: $($_.Exception.Message)" -ForegroundColor Red
}

# Backup Recipes
try {
    Write-Host "  - Backing up Recipes table..." -ForegroundColor Cyan
    $recipes = Invoke-SqlQuery "SELECT CAST(Id AS NVARCHAR(50)) as Id, Name, Type, RecipeText, CONVERT(VARCHAR(23), Created, 121) as Created, CONVERT(VARCHAR(23), Updated, 121) as Updated FROM Recipes ORDER BY Created"
    
    $sqlContent += "`r`n-- ========================================`r`n"
    $sqlContent += "-- Recipes Table`r`n"
    $sqlContent += "-- ========================================`r`n"
    $sqlContent += "PRINT 'Restoring Recipes...';`r`n`r`n"
    
    if ($recipes.Count -gt 0) {
        foreach ($recipe in $recipes) {
            $id = "'" + $recipe.Id + "'"
            $name = Escape-SqlString $recipe.Name
            $type = Escape-SqlString $recipe.Type
            $recipeText = Escape-SqlString $recipe.RecipeText
            $created = "'" + $recipe.Created + "'"
            $updated = "'" + $recipe.Updated + "'"
            
            $sqlContent += "IF NOT EXISTS (SELECT 1 FROM Recipes WHERE Id = $id)`r`n"
            $sqlContent += "    INSERT INTO Recipes (Id, Name, Type, RecipeText, Created, Updated)`r`n"
            $sqlContent += "    VALUES ($id, $name, $type, $recipeText, $created, $updated);`r`n"
        }
        $sqlContent += "GO`r`n"
        Write-Host "    ? $($recipes.Count) recipes backed up" -ForegroundColor Green
    } else {
        Write-Host "    ? No recipes to backup" -ForegroundColor Gray
    }
} catch {
    Write-Host "    ? Error backing up Recipes: $($_.Exception.Message)" -ForegroundColor Red
}

# Note: Pictures with binary data are skipped in basic version
# For full backup including images, use SQL Server backup tools

# Re-enable constraints
$sqlContent += "`r`n-- ========================================`r`n"
$sqlContent += "-- Re-enable constraints`r`n"
$sqlContent += "-- ========================================`r`n"
$sqlContent += "ALTER TABLE ChecklistItems CHECK CONSTRAINT ALL;`r`n"
$sqlContent += "ALTER TABLE Pictures CHECK CONSTRAINT ALL;`r`n"
$sqlContent += "GO`r`n`r`n"
$sqlContent += "PRINT 'Data restore completed successfully!';`r`n"
$sqlContent += "GO`r`n"

# Write to file
try {
    $sqlContent | Out-File -FilePath $OutputFile -Encoding UTF8
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "? Backup completed successfully!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Backup file created: $OutputFile" -ForegroundColor Cyan
    Write-Host "File size: $([math]::Round((Get-Item $OutputFile).Length / 1KB, 2)) KB" -ForegroundColor Gray
    Write-Host ""
    Write-Host "To restore this backup:" -ForegroundColor Yellow
    Write-Host "  1. Run InitializeDatabase.sql to create tables" -ForegroundColor Gray
    Write-Host "  2. Run this backup file: sqlcmd -S (localdb)\mssqllocaldb -i `"$OutputFile`"" -ForegroundColor Gray
    Write-Host ""
    Write-Host "NOTE: Pictures are not included in this backup." -ForegroundColor Yellow
    Write-Host "      Use SQL Server backup tools for full backup with images." -ForegroundColor Yellow
    Write-Host ""
} catch {
    Write-Host ""
    Write-Host "? Error writing backup file: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
