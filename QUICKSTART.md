# Quick Start Guide

## 1. Database Setup

### Option A: Using SQL Server Management Studio (SSMS)

1. Open SSMS and connect to your SQL Server instance:
   - For LocalDB: `(localdb)\mssqllocaldb`
   - For SQL Server: Your server name

2. Create the database:
   ```sql
   CREATE DATABASE PersonalPortal;
   GO
   ```

3. Open and execute the script: `Database/InitializeDatabase.sql`

### Option B: Using Command Line

```bash
# Connect to SQL Server using sqlcmd
sqlcmd -S (localdb)\mssqllocaldb -i Database\InitializeDatabase.sql
```

### Option C: Using Azure Data Studio

1. Open Azure Data Studio
2. Connect to `(localdb)\mssqllocaldb`
3. Open `Database/InitializeDatabase.sql`
4. Click "Run" button

## 2. Update Connection Strings

The default connection string uses SQL Server LocalDB. If using a different SQL Server instance:

Edit `PersonalPortal.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PersonalPortal;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

## 3. Configure API URL

Update the API base URL in `PersonalPortal.Web/appsettings.json` if needed:

```json
{
  "ApiBaseUrl": "https://localhost:7001"
}
```

## 4. Run the Application

### ? Option A: Using Startup Scripts (RECOMMENDED)

**PowerShell (Recommended):**
```powershell
# From the solution directory
.\start-portal.ps1
```

**Batch File:**
```cmd
# From the solution directory
start-portal.bat
```

This will open two terminal windows - one for the API and one for the Web app.

### Option B: Using Visual Studio 2022

?? **Note**: If you can't set multiple startup projects:

**Workaround 1 - Use Scripts Above**

**Workaround 2 - Manual Start:**
1. Open `PersonalPortal.sln` in Visual Studio
2. Right-click `PersonalPortal.API` ? Debug ? Start New Instance
3. Right-click `PersonalPortal.Web` ? Debug ? Start New Instance

**Workaround 3 - Run One, Debug Other:**
1. Open a terminal and run: `cd PersonalPortal.API && dotnet run`
2. In Visual Studio, set `PersonalPortal.Web` as startup project and press F5

### Option C: Using Command Line (Two Terminals)

**Terminal 1 - API:**
```bash
cd PersonalPortal.API
dotnet run
```

**Terminal 2 - Web:**
```bash
cd PersonalPortal.Web
dotnet run
```

### Option D: Using VS Code

1. Open the solution folder in VS Code
2. Install "C# Dev Kit" extension
3. Press F5 to start (it will ask which project - start API first)
4. Open another terminal and run the Web project

## 5. Access the Application

- **Web Application**: https://localhost:7000 (or http://localhost:5000)
- **API**: https://localhost:7001 (or http://localhost:5001)
- **Swagger Documentation**: https://localhost:7001/swagger

## 6. Login

Use the default credentials:
- Username: `harreby`
- Password: `fishalot`

Check "Trust this computer for 30 days" to stay logged in.

## Troubleshooting

### "There are no property pages for the selection" in Visual Studio

This issue can occur with certain Visual Studio configurations. **Solution**: Use the startup scripts (`start-portal.ps1` or `start-portal.bat`) instead.

### Database Connection Issues

**Error: "Cannot open database"**
- Ensure SQL Server/LocalDB is running
- Check the connection string in `appsettings.json`
- Verify the database exists: `SELECT name FROM sys.databases`

**Error: "Login failed"**
- For Trusted_Connection: Ensure your Windows user has access
- For SQL Auth: Use correct username/password in connection string

### API Connection Issues

**Error: "Failed to fetch"**
- Ensure both API and Web projects are running
- Check the API URL in `PersonalPortal.Web/appsettings.json`
- Verify CORS settings in `PersonalPortal.API/Program.cs`

### Port Already in Use

**Error: "Address already in use"**
- Change ports in `launchSettings.json` files
- Update CORS in API and ApiBaseUrl in Web accordingly

### SSL Certificate Issues

**Error: "SSL certificate problem"**
Run these commands to trust the dev certificate:
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

## Next Steps

1. **Add Sample Data**: Use Swagger UI to add test data via the API
2. **Create Text Notes**: Go to Text Notes ? Add New Note
3. **Create Checklists**: Go to Checklists ? Add New Checklist
4. **Explore the API**: Visit https://localhost:7001/swagger to test endpoints

## Common Tasks

### Add a New User

Execute this SQL in your database:
```sql
INSERT INTO Users (Username, Password, Role, DisplayName)
VALUES ('newuser', 'password123', 'Viewer', 'New User');
```

### Reset the Database

Re-run the `InitializeDatabase.sql` script. It's designed to be idempotent.

### Change Ports

Edit `launchSettings.json` in both projects:
- `PersonalPortal.API/Properties/launchSettings.json`
- `PersonalPortal.Web/Properties/launchSettings.json`

Don't forget to update:
- CORS origins in `PersonalPortal.API/Program.cs`
- ApiBaseUrl in `PersonalPortal.Web/appsettings.json`

### Stop the Applications

If running via scripts:
- Close the terminal windows
- Or press `Ctrl+C` in each terminal

If running via Visual Studio:
- Click the Stop button or press `Shift+F5`

## Support

If you encounter issues:
1. Check the console output for error messages
2. Review the README.md for detailed information
3. Verify all setup steps were completed
4. Ensure .NET 10.0 SDK is installed: `dotnet --version`
