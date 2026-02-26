# Personal Portal

A full-stack personal information management system built with **Blazor Server** (.NET 10) and **SQL Server**.

## Features

- ?? **Text Notes** - Create and manage text notes
- ? **Checklists** - Organize items with groups, export to PDF
- ?? **Recipes** - Store recipes with rich text formatting and pictures
- ?? **Pictures** - Upload and manage images, link to recipes
- ?? **Authentication** - Secure login system
- ?? **PDF Export** - Generate printable checklist PDFs

## Technology Stack

### Backend (API)
- .NET 10
- ASP.NET Core Web API
- Dapper (SQL data access)
- QuestPDF (PDF generation)
- SQL Server (LocalDB)

### Frontend (Web)
- Blazor Server
- Bootstrap 5
- Quill.js (WYSIWYG editor)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (included with Visual Studio)
- Visual Studio 2022 or VS Code (optional but recommended)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/jorgenharreby/PersonalPortal.git
cd PersonalPortal
```

### 2. Set Up the Database

#### Option A: Using the Initialization Script (Recommended)

```cmd
sqlcmd -S "(localdb)\mssqllocaldb" -i Database\InitializeDatabase.sql
```

This will:
- Create the `PersonalPortal` database
- Create all tables (Users, TextNotes, Checklists, ChecklistItems, Recipes, Pictures)
- Create indexes for performance
- Insert default user (username: `harreby`, password: `fishalot`)

#### Option B: Manual Setup

1. Open SQL Server Management Studio (SSMS) or Azure Data Studio
2. Connect to `(localdb)\mssqllocaldb`
3. Open `Database\InitializeDatabase.sql`
4. Execute the script

### 3. Run the Applications

#### Option A: Using the Startup Script (Easiest)

```cmd
start-portal-improved.bat
```

This will:
- Start the API on `https://localhost:7001`
- Start the Web app on `https://localhost:7000`
- Open your browser automatically

#### Option B: Manual Startup

**Terminal 1 - API:**
```cmd
cd PersonalPortal.API
dotnet run --launch-profile https
```

**Terminal 2 - Web:**
```cmd
cd PersonalPortal.Web
dotnet run --launch-profile https
```

### 4. Access the Application

1. Open your browser to: **https://localhost:7000**
2. Login with default credentials:
   - **Username:** `harreby`
   - **Password:** `fishalot`
3. Start using your Personal Portal!

## Project Structure

```
PersonalPortal/
??? PersonalPortal.API/          # Backend API
?   ??? Controllers/             # API endpoints
?   ??? Data/                    # Database repositories
?   ??? Services/                # Business logic (PDF generation)
?   ??? Program.cs
??? PersonalPortal.Web/          # Blazor Server frontend
?   ??? Components/
?   ?   ??? Pages/              # Razor pages
?   ?   ??? Layout/             # Layout components
?   ?   ??? Shared/             # Shared components (QuillEditor)
?   ??? Services/               # Frontend services
?   ??? wwwroot/                # Static files, CSS, JS
?   ??? Program.cs
??? PersonalPortal.Core/         # Shared models
?   ??? Models/                 # Data models
??? Database/                    # SQL scripts
?   ??? InitializeDatabase.sql  # Main setup script
?   ??? Migration_*.sql         # Migration scripts
??? Documentation files         # Feature documentation (*.md)
```

## Database Schema

### Tables

- **Users** - User authentication
- **TextNotes** - Simple text notes
- **Checklists** - Checklist headers
- **ChecklistItems** - Individual checklist items with groups
- **Recipes** - Recipe information with rich text
- **Pictures** - Image storage with optional recipe links

### Connection String

Default connection string (configured in `appsettings.json`):
```
Server=(localdb)\\mssqllocaldb;Database=PersonalPortal;Trusted_Connection=true;TrustServerCertificate=true
```

## Configuration

### API Configuration (`PersonalPortal.API/appsettings.json`)
- **ConnectionStrings:DefaultConnection** - Database connection
- **CORS** - Allowed origins for Blazor app

### Web Configuration (`PersonalPortal.Web/appsettings.json`)
- **ApiBaseUrl** - API endpoint URL

## Features Documentation

Detailed documentation for each feature:

- **[Setup Checklist](SETUP-CHECKLIST.md)** - Complete setup guide
- **[Quick Start](QUICKSTART.md)** - Get started quickly
- **[Checklist Groups](CHECKLIST-GROUPS-FEATURE.md)** - Organize checklist items
- **[PDF Export](CHECKLIST-PDF-FEATURE.md)** - Generate printable PDFs
- **[Recipe Editor](QUILL-EDITOR-FIX.md)** - Rich text editing with Quill
- **[Recipes & Pictures](RECIPES-PICTURES-COMPLETE.md)** - Complete guide

## Troubleshooting

### Database Connection Issues

**Problem:** Cannot connect to database

**Solution:**
1. Verify SQL Server LocalDB is installed:
   ```cmd
   sqllocaldb info
   ```
2. Create/start LocalDB instance:
   ```cmd
   sqllocaldb create MSSQLLocalDB
   sqllocaldb start MSSQLLocalDB
   ```
3. Run initialization script again

### Port Already in Use

**Problem:** Port 7000 or 7001 already in use

**Solution:**
1. Find and kill the process:
   ```cmd
   netstat -ano | findstr :7001
   taskkill /PID [PID] /F
   ```
2. Or change ports in `launchSettings.json`

### HTTPS Certificate Issues

**Problem:** Certificate errors in browser

**Solution:**
```cmd
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

See [TROUBLESHOOTING.md](TROUBLESHOOTING.md) for more issues and solutions.

## Development

### Building the Solution

```cmd
dotnet build
```

### Adding Migrations

When updating the database schema:

1. Create a new migration script in `Database/` folder
2. Name it `Migration_[Description].sql`
3. Include it in the repository
4. Run it on your database
5. Document the changes

## Default Credentials

**?? Important:** Change the default user password in production!

Default user (for development):
- **Username:** harreby
- **Password:** fishalot
- **Display Name:** Jørgen
- **Role:** Admin

## Security Notes

- Passwords are currently stored in **plain text** - implement proper hashing before production use
- HTTPS is required in production
- Update CORS settings for production domains
- Change default user credentials

## Author

**Jørgen Harreby**
- GitHub: [@jorgenharreby](https://github.com/jorgenharreby)

## Acknowledgments

- QuestPDF for PDF generation
- Quill.js for rich text editing
- Bootstrap for UI components
- Dapper for database access

## Version History

- **1.0.0** - Initial release
  - Text Notes
  - Checklists with groups
  - Recipes with rich text editor
  - Pictures with upload
  - PDF export for checklists
  - Authentication system

---

**Happy organizing! ???????**
