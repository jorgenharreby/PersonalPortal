# Development Setup Checklist

Use this checklist to ensure your development environment is properly configured.

## Prerequisites

- [ ] .NET 10.0 SDK installed
  ```bash
  dotnet --version
  # Should show 10.x.x
  ```

- [ ] SQL Server or LocalDB installed
  - For LocalDB: Part of Visual Studio or SQL Server Express
  - Test connection: `sqlcmd -S (localdb)\mssqllocaldb -Q "SELECT @@VERSION"`

- [ ] Git installed (for version control)
  ```bash
  git --version
  ```

- [ ] IDE installed (choose one):
  - [ ] Visual Studio 2022 (Community or higher)
  - [ ] Visual Studio Code with C# Dev Kit
  - [ ] JetBrains Rider

## Initial Setup

- [ ] Clone or open the repository
  ```bash
  cd C:\Users\Z6FMB\Source\Repos\PersonalPortal
  ```

- [ ] Restore NuGet packages
  ```bash
  dotnet restore
  ```

- [ ] Build the solution
  ```bash
  dotnet build
  # Should succeed with no errors
  ```

## Database Setup

- [ ] SQL Server/LocalDB is running
- [ ] Database created
  ```sql
  CREATE DATABASE PersonalPortal;
  ```
- [ ] Schema initialized
  - Execute `Database/InitializeDatabase.sql`
- [ ] Default user exists
  ```sql
  SELECT * FROM Users WHERE Username = 'harreby';
  -- Should return one row
  ```
- [ ] Connection string configured in `PersonalPortal.API/appsettings.json`

## Configuration

- [ ] API connection string verified in `PersonalPortal.API/appsettings.json`
- [ ] Web API URL verified in `PersonalPortal.Web/appsettings.json`
- [ ] API port: 7001 (HTTPS) or 5001 (HTTP)
- [ ] Web port: 7000 (HTTPS) or 5000 (HTTP)
- [ ] CORS configured for Web URL in `PersonalPortal.API/Program.cs`

## SSL Certificates

- [ ] Development certificates trusted
  ```bash
  dotnet dev-certs https --trust
  ```
- [ ] No SSL warnings in browser

## Running the Application

Choose your preferred method:

### Method 1: Startup Scripts (Recommended)
- [ ] PowerShell script works: `.\start-portal.ps1`
- [ ] OR Batch file works: `start-portal.bat`
- [ ] Both terminal windows open successfully
- [ ] No errors in console output

### Method 2: Visual Studio
- [ ] Solution opens without errors
- [ ] Both projects build successfully
- [ ] Can start API project manually
- [ ] Can start Web project manually

### Method 3: Command Line
- [ ] API starts in first terminal
  ```bash
  cd PersonalPortal.API
  dotnet run
  ```
- [ ] Web starts in second terminal
  ```bash
  cd PersonalPortal.Web
  dotnet run
  ```

## Verification

- [ ] API is accessible at https://localhost:7001
- [ ] Swagger UI loads at https://localhost:7001/swagger
- [ ] Web app is accessible at https://localhost:7000
- [ ] Login page loads without errors
- [ ] Can login with harreby/fishalot
- [ ] Home page displays after login
- [ ] No JavaScript errors in browser console (F12)

## Test Basic Functionality

- [ ] Can navigate to Text Notes page
- [ ] Can create a new text note
- [ ] Can view the created note
- [ ] Can edit the note
- [ ] Can delete the note
- [ ] Can navigate to Checklists page
- [ ] Can create a new checklist with items
- [ ] Can view the checklist
- [ ] Can logout successfully

## API Testing (via Swagger)

- [ ] Swagger UI is accessible
- [ ] Can expand endpoint groups
- [ ] Can execute Auth/login endpoint
- [ ] Can execute GET /api/textnotes
- [ ] Can execute POST /api/textnotes
- [ ] Can execute other endpoints

## Common Issues Resolved

Mark these if you encountered and resolved them:

- [ ] Port conflict resolved (changed ports in launchSettings.json)
- [ ] Database connection fixed (updated connection string)
- [ ] SSL certificate trusted (ran dotnet dev-certs https --trust)
- [ ] CORS error fixed (verified allowed origins)
- [ ] Build errors resolved (ran dotnet restore)
- [ ] Multiple startup projects not working (using scripts instead)

## Development Environment Ready! ??

Once all items are checked, you're ready to develop!

## Next Steps for Development

1. [ ] Read the IMPLEMENTATION.md for feature details
2. [ ] Review the API endpoints in README.md
3. [ ] Familiarize yourself with the codebase structure
4. [ ] Start implementing remaining features (Recipes/Pictures UI)
5. [ ] Consider security enhancements for production

## Quick Reference

### Start Development Session
```bash
# Option 1: Use script
.\start-portal.ps1

# Option 2: Manual (two terminals)
# Terminal 1:
cd PersonalPortal.API && dotnet run

# Terminal 2:
cd PersonalPortal.Web && dotnet run
```

### Access URLs
- Web: https://localhost:7000
- API: https://localhost:7001
- Swagger: https://localhost:7001/swagger

### Default Login
- Username: harreby
- Password: fishalot

### Rebuild Everything
```bash
dotnet clean
dotnet restore
dotnet build
```

### Database Reset
Re-run `Database/InitializeDatabase.sql` in SSMS or Azure Data Studio.

---

**Last Updated:** Initial setup
**Status:** ? Ready for development
