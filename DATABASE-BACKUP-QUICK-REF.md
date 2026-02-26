# ?? Quick Reference: Database Backup & Restore

## Backup Your Data

### Quick Backup:
```cmd
backup-database.bat
```

**Output:** `Database/BackupData_YYYYMMDD_HHMMSS.sql`

---

## Restore Your Data

### Restore Latest Backup:
```cmd
restore-database.bat
```

### Restore Specific Backup:
```cmd
restore-database.bat "Database\BackupData_20250221_143052.sql"
```

---

## Common Tasks

### Weekly Backup (Recommended):
```cmd
backup-database.bat
```
Run every Monday morning!

### Before Code Update:
```cmd
# 1. Backup first
backup-database.bat

# 2. Then update
git pull origin main
```

### Moving to New PC:
```cmd
# Old PC:
backup-database.bat
copy Database\BackupData_*.sql "E:\Backup\"

# New PC:
git clone https://github.com/jorgenharreby/PersonalPortal.git
cd PersonalPortal
sqlcmd -S "(localdb)\mssqllocaldb" -i Database\InitializeDatabase.sql
copy "E:\Backup\BackupData_*.sql" Database\
restore-database.bat
```

### Clean Database Restore:
```cmd
# Drop and recreate
sqlcmd -S "(localdb)\mssqllocaldb" -Q "DROP DATABASE PersonalPortal; CREATE DATABASE PersonalPortal;"

# Initialize
sqlcmd -S "(localdb)\mssqllocaldb" -i Database\InitializeDatabase.sql

# Restore data
restore-database.bat
```

---

## What's Included in Backup?

- ? All Text Notes
- ? All Checklists (with groups)
- ? All Recipes (with formatting)
- ? All Pictures (with image data)
- ? Users (without passwords)

---

## Files Created:

### Backup Scripts:
- `backup-database.bat` - Windows batch file
- `backup-database.ps1` - PowerShell script

### Restore Scripts:
- `restore-database.bat` - Windows batch file
- `restore-database.ps1` - PowerShell script

### Documentation:
- `DATABASE-BACKUP-RESTORE-GUIDE.md` - Complete guide
- `DATABASE-BACKUP-QUICK-REF.md` - This file

### Output:
- `Database/BackupData_*.sql` - Your backup files

---

## Troubleshooting

| Problem | Solution |
|---------|----------|
| sqlcmd not found | Install SQL Server Command Line Utilities |
| Cannot connect | Run: `sqllocaldb start MSSQLLocalDB` |
| Backup too large | Normal if you have many pictures |
| Restore fails | Make sure database is initialized first |

---

## Best Practices

1. ? **Backup weekly** - Set a reminder!
2. ? **Backup before updates** - Always safe
3. ? **Test restore** - Once a month
4. ? **Keep multiple backups** - 7-30 days
5. ? **Store securely** - Cloud storage

---

## Example Session

```
> backup-database.bat

========================================
Personal Portal Database Backup Script
========================================

Backing up data from tables...
  ? 1 users backed up
  ? 12 text notes backed up
  ? 5 checklists backed up
  ? 23 checklist items backed up
  ? 8 recipes backed up
  ? 15 pictures backed up

? Backup completed successfully!

Backup file created: Database\BackupData_20250221_143052.sql
File size: 8523.45 KB
```

---

**See DATABASE-BACKUP-RESTORE-GUIDE.md for complete documentation**

Quick? Easy? Powerful? You got it! ???
