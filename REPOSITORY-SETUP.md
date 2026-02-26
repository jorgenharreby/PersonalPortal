# ?? Repository Setup - Ready for Cloning!

## ? What Was Fixed

The repository is now properly configured so that when you clone it on another PC, you'll have everything you need to set up the database.

### Changes Made:

1. **`.gitignore` Updated** ?
   - Clarified that SQL Server database files (`.mdf`, `.ldf`, `.ndf`) are ignored
   - Added explicit rules to **include** SQL scripts (`.sql` files)
   - Added section to keep documentation (`.md` files)

2. **`README.md` Enhanced** ?
   - Added comprehensive setup instructions
   - Included database initialization steps
   - Added troubleshooting section
   - Documented project structure

3. **Database Scripts Included** ?
   - `Database/InitializeDatabase.sql` - Main setup script
   - `Database/Migration_AddItemGroup.sql` - Migration script
   - Both files are tracked in Git

---

## ?? Setup Instructions for New Clone

When you clone this repository on another PC, follow these steps:

### 1. Clone the Repository

```bash
git clone https://github.com/jorgenharreby/PersonalPortal.git
cd PersonalPortal
```

### 2. Verify Database Scripts Are Present

Check that these files exist:
```
Database/
??? InitializeDatabase.sql
??? Migration_AddItemGroup.sql
```

If they're not there, the repository wasn't cloned correctly or the files weren't committed.

### 3. Install Prerequisites

- ? .NET 10 SDK
- ? SQL Server LocalDB (comes with Visual Studio)

Verify:
```cmd
dotnet --version
sqllocaldb info
```

### 4. Create and Initialize Database

Run the initialization script:

```cmd
sqlcmd -S "(localdb)\mssqllocaldb" -i Database\InitializeDatabase.sql
```

This creates:
- `PersonalPortal` database
- All tables
- Indexes
- Default user (harreby/fishalot)

### 5. Run the Applications

```cmd
start-portal-improved.bat
```

Or manually:

**Terminal 1:**
```cmd
cd PersonalPortal.API
dotnet run --launch-profile https
```

**Terminal 2:**
```cmd
cd PersonalPortal.Web
dotnet run --launch-profile https
```

### 6. Access the Application

1. Go to: `https://localhost:7000`
2. Login: `harreby` / `fishalot`
3. Done! ?

---

## ?? Verification Checklist

After cloning, verify these files exist:

### Database Files:
- [ ] `Database/InitializeDatabase.sql` exists
- [ ] `Database/Migration_AddItemGroup.sql` exists

### Documentation Files:
- [ ] `README.md` exists with setup instructions
- [ ] `SETUP-CHECKLIST.md` exists
- [ ] `QUICKSTART.md` exists
- [ ] Feature documentation files (*.md) exist

### Startup Scripts:
- [ ] `start-portal-improved.bat` exists
- [ ] `test-api-connection.bat` exists

### Configuration Files:
- [ ] `PersonalPortal.API/appsettings.json` exists
- [ ] `PersonalPortal.Web/appsettings.json` exists

---

## ?? What's NOT in the Repository

These are correctly excluded (via `.gitignore`):

- ? **Database files** (`.mdf`, `.ldf`, `.ndf`) - You create these by running the scripts
- ? **Build output** (`bin/`, `obj/` folders)
- ? **User-specific files** (`.suo`, `.user` files)
- ? **IDE settings** (`.vs/` folder)
- ? **NuGet packages** - Restored automatically with `dotnet restore`

This is intentional! The database is created fresh on each machine using the SQL scripts.

---

## ?? Troubleshooting

### "Database scripts not found after clone"

**Problem:** `Database/InitializeDatabase.sql` doesn't exist after cloning

**Solution:**
1. Check if you're in the right directory: `cd PersonalPortal`
2. Check Git status: `git status`
3. List database files: `dir Database\` (Windows) or `ls Database/` (Mac/Linux)
4. If still missing, pull from origin: `git pull origin main`
5. Verify on GitHub that the files are in the repository

### "sqlcmd command not found"

**Problem:** sqlcmd is not installed

**Solution:**
1. Install SQL Server Command Line Utilities:
   - Download from: https://learn.microsoft.com/en-us/sql/tools/sqlcmd/sqlcmd-utility
   - Or install with Visual Studio (includes SQL Server tools)

2. Or use Azure Data Studio:
   - Open `Database/InitializeDatabase.sql`
   - Click "Run" button

### "Cannot connect to LocalDB"

**Problem:** LocalDB instance not running

**Solution:**
```cmd
# List instances
sqllocaldb info

# Create instance if needed
sqllocaldb create MSSQLLocalDB

# Start instance
sqllocaldb start MSSQLLocalDB

# Try database creation again
sqlcmd -S "(localdb)\mssqllocaldb" -i Database\InitializeDatabase.sql
```

---

## ?? For Repository Maintainers

### Adding New Migration Scripts

When you update the database schema:

1. **Create the migration file:**
   ```sql
   -- Database/Migration_YourFeature.sql
   USE PersonalPortal;
   GO
   
   -- Your migration SQL here
   ALTER TABLE YourTable ADD YourColumn NVARCHAR(100);
   GO
   ```

2. **Test it locally:**
   ```cmd
   sqlcmd -S "(localdb)\mssqllocaldb" -d PersonalPortal -i Database\Migration_YourFeature.sql
   ```

3. **Commit and push:**
   ```cmd
   git add Database/Migration_YourFeature.sql
   git commit -m "Add migration for YourFeature"
   git push
   ```

4. **Update documentation:**
   - Add to `InitializeDatabase.sql` if it should be part of initial setup
   - Document in feature-specific `.md` file
   - Update this file with setup instructions

### Updating InitializeDatabase.sql

When adding new tables or features:

1. Add the new schema to `InitializeDatabase.sql`
2. Use `IF NOT EXISTS` checks for idempotency
3. Test on a fresh database
4. Commit and push

---

## ? Summary

Your repository is now properly configured:

- ? **Database scripts are included** in Git
- ? **Database files are excluded** from Git (created locally)
- ? **Documentation is complete** with setup instructions
- ? **`.gitignore` is optimized** for the project
- ? **Ready to clone** on any PC with prerequisites

### What Happens When You Clone:

1. ? All source code downloads
2. ? Database scripts download
3. ? Documentation downloads
4. ? Startup scripts download
5. ? Database is NOT downloaded (you create it)
6. ? Build output is NOT downloaded (you build it)

### Steps on New PC:

1. Clone repository
2. Run `Database/InitializeDatabase.sql`
3. Run `start-portal-improved.bat`
4. Done! ?

---

## ?? Quick Reference

### Database Setup (One Command):
```cmd
sqlcmd -S "(localdb)\mssqllocaldb" -i Database\InitializeDatabase.sql
```

### Start Applications:
```cmd
start-portal-improved.bat
```

### Access Application:
```
https://localhost:7000
Username: harreby
Password: fishalot
```

---

**Everything is ready for cloning! ??**

When you clone this repository on another PC, you'll have all the database scripts and documentation you need to get started.
