# ?? Database Backup & Restore Guide

## Overview

The Personal Portal includes scripts to **backup** and **restore** your database content to/from SQL files with INSERT statements.

### What Gets Backed Up:
- ? **Users** (without passwords for security)
- ? **Text Notes** (all notes with content)
- ? **Checklists** (all checklists and items with groups)
- ? **Recipes** (recipes with formatted text)
- ? **Pictures** (images with binary data)

### What's Preserved:
- ? All GUIDs (IDs)
- ? Timestamps (Created, Updated)
- ? Relationships (Recipe-Picture links)
- ? Binary data (images)

---

## ?? Backup Scripts

### Files:
- `backup-database.bat` - Windows batch file (easy to run)
- `backup-database.ps1` - PowerShell script (does the work)

### Quick Usage:

**Option 1: Double-click**
- Just double-click `backup-database.bat`
- Backup file is created in `Database/` folder

**Option 2: Command Line**
```cmd
backup-database.bat
```

**Option 3: PowerShell with Options**
```powershell
.\backup-database.ps1 -OutputFile "MyBackup.sql"
```

### Output:
Creates a file like: `Database/BackupData_20250221_143052.sql`

### What Happens:
1. Connects to LocalDB
2. Reads all data from tables
3. Generates INSERT statements
4. Saves to SQL file
5. Shows summary of what was backed up

---

## ?? Restore Scripts

### Files:
- `restore-database.bat` - Windows batch file
- `restore-database.ps1` - PowerShell script

### Quick Usage:

**Option 1: Restore Latest Backup**
```cmd
restore-database.bat
```
Automatically finds and uses the most recent backup file.

**Option 2: Restore Specific Backup**
```cmd
restore-database.bat "Database\BackupData_20250221_143052.sql"
```

**Option 3: PowerShell**
```powershell
.\restore-database.ps1 -BackupFile "Database\BackupData_20250221_143052.sql"
```

### What Happens:
1. Finds backup file (latest or specified)
2. Asks for confirmation
3. Runs the SQL INSERT statements
4. Shows success/failure message

### ?? Important Notes:
- **Does NOT delete existing data**
- **Adds new data** alongside existing
- Use `IF NOT EXISTS` checks to prevent duplicates
- For clean restore, drop & recreate database first

---

## ?? Common Scenarios

### Scenario 1: Regular Backup (Recommended Weekly)

```cmd
backup-database.bat
```

**Result:** Creates timestamped backup file

**Best Practice:**
- Run weekly or before major changes
- Keep multiple backups
- Test restore occasionally

---

### Scenario 2: Moving to Another PC

**On Original PC:**
```cmd
# 1. Create backup
backup-database.bat

# 2. Copy backup file
copy Database\BackupData_*.sql "E:\MyBackups\"
```

**On New PC:**
```cmd
# 1. Clone repository
git clone https://github.com/jorgenharreby/PersonalPortal.git

# 2. Initialize database
sqlcmd -S "(localdb)\mssqllocaldb" -i Database\InitializeDatabase.sql

# 3. Copy backup file to Database folder
copy "E:\MyBackups\BackupData_*.sql" Database\

# 4. Restore data
restore-database.bat
```

---

### Scenario 3: Clean Restore (Start Fresh)

When you want to **completely replace** all data:

```cmd
# 1. Drop and recreate database
sqlcmd -S "(localdb)\mssqllocaldb" -Q "DROP DATABASE PersonalPortal; CREATE DATABASE PersonalPortal;"

# 2. Initialize schema
sqlcmd -S "(localdb)\mssqllocaldb" -i Database\InitializeDatabase.sql

# 3. Restore data
restore-database.bat "Database\BackupData_20250221_143052.sql"
```

---

### Scenario 4: Before Major Update

**Before updating code that changes the database:**

```cmd
# 1. Backup current data
backup-database.bat

# 2. Pull new code
git pull origin main

# 3. Run migrations
sqlcmd -S "(localdb)\mssqllocaldb" -i Database\Migration_*.sql

# 4. If something goes wrong:
#    - Restore database from backup
#    - Roll back code changes
```

---

### Scenario 5: Testing on Fresh Database

**Create test environment:**

```cmd
# 1. Create test database
sqlcmd -S "(localdb)\mssqllocaldb" -Q "CREATE DATABASE PersonalPortal_Test"

# 2. Initialize test database
sqlcmd -S "(localdb)\mssqllocaldb" -d PersonalPortal_Test -i Database\InitializeDatabase.sql

# 3. Restore production data to test
.\restore-database.ps1 -Database "PersonalPortal_Test" -BackupFile "Database\BackupData_Latest.sql"
```

---

## ?? Advanced Usage

### Custom Backup Location

```powershell
.\backup-database.ps1 -OutputFile "C:\Backups\MyPortal_$(Get-Date -Format 'yyyyMMdd').sql"
```

### Backup Different Database

```powershell
.\backup-database.ps1 -Database "PersonalPortal_Test" -OutputFile "Test_Backup.sql"
```

### Restore to Different Database

```powershell
.\restore-database.ps1 -Database "PersonalPortal_Test" -BackupFile "Database\BackupData_20250221.sql"
```

### Different SQL Server Instance

```powershell
.\backup-database.ps1 -ServerInstance "localhost\SQLEXPRESS"
.\restore-database.ps1 -ServerInstance "localhost\SQLEXPRESS"
```

---

## ?? Backup File Format

### Structure:

```sql
-- ========================================
-- Personal Portal Database Backup
-- Generated: 2025-02-21 14:30:52
-- ========================================

USE PersonalPortal;
GO

-- Disable constraints for faster insertion
ALTER TABLE ChecklistItems NOCHECK CONSTRAINT ALL;
GO

-- ========================================
-- Users Table
-- ========================================
SET IDENTITY_INSERT Users ON;
IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = 1)
    INSERT INTO Users (Id, Username, Password, Role, DisplayName)
    VALUES (1, 'harreby', 'CHANGE_ME', 'Admin', 'Jørgen');
SET IDENTITY_INSERT Users OFF;
GO

-- ========================================
-- TextNotes Table
-- ========================================
IF NOT EXISTS (SELECT 1 FROM TextNotes WHERE Id = '...')
    INSERT INTO TextNotes (Id, Name, Content, Created, Updated)
    VALUES ('...', 'My Note', 'Content here', '2025-02-21 10:00:00', '2025-02-21 10:00:00');
GO

-- [... more tables ...]

-- Re-enable constraints
ALTER TABLE ChecklistItems CHECK CONSTRAINT ALL;
GO

PRINT 'Data restore completed successfully!';
GO
```

### Features:
- **IF NOT EXISTS checks** - Prevents duplicate data
- **IDENTITY_INSERT** - Preserves original IDs
- **Binary data as hex** - Images included as 0x... hex strings
- **Proper escaping** - Handles special characters
- **Transaction safety** - Can be wrapped in transaction if needed

---

## ?? Troubleshooting

### "sqlcmd not found"

**Problem:** sqlcmd command not available

**Solution:**
1. Install SQL Server Command Line Utilities
2. Or use SQL Server Management Studio to run the scripts
3. Or use Azure Data Studio

### "Cannot connect to LocalDB"

**Problem:** Database connection fails

**Solution:**
```cmd
# Check LocalDB instances
sqllocaldb info

# Start instance
sqllocaldb start MSSQLLocalDB

# Try again
backup-database.bat
```

### "Backup file is huge"

**Problem:** Backup file is very large (>100MB)

**Cause:** Many pictures with large images

**Solutions:**
1. This is normal for image data
2. Compress the .sql file (ZIP reduces size significantly)
3. Consider separate picture backup if needed

### "Duplicate key errors on restore"

**Problem:** Getting primary key violations

**Cause:** Data already exists in database

**Solutions:**
1. Use clean restore (drop/recreate database first)
2. Or edit backup file to remove duplicates
3. Or restore to empty database

### "Images not restoring correctly"

**Problem:** Pictures appear corrupted after restore

**Solution:**
1. Make sure backup file wasn't edited in text editor
2. Use UTF-8 encoding when viewing
3. Binary data is sensitive to corruption

### "Restore is very slow"

**Problem:** Restore takes a long time

**Causes:**
- Large images
- Many records
- Constraints being checked

**Solutions:**
- Be patient (images are large)
- Constraints are disabled during restore for speed
- Can take 5-10 minutes for large databases

---

## ?? Backup File Management

### Naming Convention:
```
BackupData_YYYYMMDD_HHMMSS.sql
Example: BackupData_20250221_143052.sql
```

### Recommended Retention:
- **Daily backups:** Keep 7 days
- **Weekly backups:** Keep 4 weeks
- **Monthly backups:** Keep 12 months
- **Before updates:** Keep indefinitely

### File Sizes (Approximate):
- Empty database: ~5 KB
- 10 notes, 5 checklists: ~20 KB
- 50 notes, 20 checklists, 5 recipes: ~100 KB
- + 10 pictures: +5-10 MB
- + 100 pictures: +50-100 MB

### Storage:
- Store backups outside repository
- Use cloud storage (OneDrive, Dropbox, Google Drive)
- Compress older backups (ZIP)
- Don't commit large backups to Git

---

## ?? Security Notes

### Password Handling:
- **User passwords are NOT backed up** for security
- Backup file shows `'CHANGE_ME'` for passwords
- After restore, users must reset passwords
- Or manually edit backup file to include passwords

### Sensitive Data:
- Backup files contain all your data in plain text
- Store backups securely
- Don't share backup files publicly
- Consider encrypting backup files

### Best Practices:
1. Run backups regularly
2. Store in secure location
3. Test restores occasionally
4. Keep multiple versions
5. Document what's in each backup

---

## ?? Example Backup Session

```
========================================
Personal Portal Database Backup Script
========================================

Creating backup file: Database\BackupData_20250221_143052.sql
Server: (localdb)\mssqllocaldb
Database: PersonalPortal

Backing up data from tables...
  - Backing up Users table...
    ? 1 users backed up
  - Backing up TextNotes table...
    ? 12 text notes backed up
  - Backing up Checklists table...
    ? 5 checklists backed up
  - Backing up ChecklistItems table...
    ? 23 checklist items backed up
  - Backing up Recipes table...
    ? 8 recipes backed up
  - Backing up Pictures table...
    ? 15 pictures backed up

========================================
? Backup completed successfully!
========================================

Backup file created: Database\BackupData_20250221_143052.sql
File size: 8523.45 KB

To restore this backup:
  1. Run InitializeDatabase.sql to create tables
  2. Run this backup file: sqlcmd -S (localdb)\mssqllocaldb -i "Database\BackupData_20250221_143052.sql"
```

---

## ? Summary

### Backup:
```cmd
backup-database.bat
```
- Creates timestamped SQL file
- Includes all data
- Safe to run anytime

### Restore:
```cmd
restore-database.bat
```
- Uses latest backup by default
- Asks for confirmation
- Preserves existing data (adds new)

### Best Practices:
1. ? Backup before major changes
2. ? Backup weekly
3. ? Test restores occasionally
4. ? Keep multiple backup versions
5. ? Store backups securely

---

**Your data is now protected with easy backup and restore! ???**
