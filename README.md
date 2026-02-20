# Personal Portal

A personal web portal for managing text notes, checklists, recipes, and pictures with secure authentication.

## Features

- **Authentication**: Secure login with option to trust computer for 30 days
- **Text Notes**: Store and manage plain text notes
- **Checklists**: Create and organize checklists with items
- **Recipes**: Save recipes with formatted text and pictures
- **Pictures**: Upload and manage pictures with captions
- **Search & Filter**: Search items by name and filter by type
- **Sortable Tables**: Sort lists by various columns
- **Responsive Design**: Bootstrap-based UI that works on all devices

## Technology Stack

- **Frontend**: Blazor Server with Bootstrap 5
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server (LocalDB or SQL Server)
- **Data Access**: Dapper ORM
- **API Documentation**: Swagger/OpenAPI

## Prerequisites

- .NET 10.0 SDK or later
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 (recommended) or VS Code

## Setup Instructions

### 1. Database Setup

1. Open SQL Server Management Studio (SSMS) or Azure Data Studio
2. Connect to your SQL Server instance:
   - For LocalDB: `(localdb)\mssqllocaldb`
   - For SQL Server: Your server name
3. Open the file `Database/InitializeDatabase.sql`
4. Create the database by uncommenting the first few lines:
   ```sql
   CREATE DATABASE PersonalPortal;
   GO
   USE PersonalPortal;
   GO
   ```
5. Execute the entire script to create tables and seed data

### 2. Update Connection Strings

Update the connection string in `PersonalPortal.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PersonalPortal;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

For SQL Server, use:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PersonalPortal;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True"
  }
}
```

### 3. Configure API URL

Update the API base URL in `PersonalPortal.Web/appsettings.json` if needed:

```json
{
  "ApiBaseUrl": "https://localhost:7001"
}
```

### 4. Build and Run

#### Using Visual Studio:

1. Open `PersonalPortal.sln`
2. Set multiple startup projects:
   - Right-click solution ? Properties ? Multiple startup projects
   - Set both `PersonalPortal.API` and `PersonalPortal.Web` to "Start"
3. Press F5 to run

#### Using Command Line:

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

### 5. Access the Application

- **Web Application**: https://localhost:7000 (or http://localhost:5000)
- **API**: https://localhost:7001 (or http://localhost:5001)
- **Swagger**: https://localhost:7001/swagger

### 6. Login

Default credentials:
- **Username**: harreby
- **Password**: fishalot
- **Role**: Admin

## Project Structure

```
PersonalPortal/
??? Database/
?   ??? InitializeDatabase.sql       # Database schema and seed data
??? PersonalPortal.Core/
?   ??? Models/                      # Shared data models
?       ??? User.cs
?       ??? TextNote.cs
?       ??? Checklist.cs
?       ??? Recipe.cs
?       ??? Picture.cs
?       ??? AuthModels.cs
??? PersonalPortal.API/
?   ??? Controllers/                 # API endpoints
?   ?   ??? AuthController.cs
?   ?   ??? TextNotesController.cs
?   ?   ??? ChecklistsController.cs
?   ?   ??? RecipesController.cs
?   ?   ??? PicturesController.cs
?   ??? Data/                        # Data access layer (Dapper)
?   ?   ??? IUserRepository.cs
?   ?   ??? UserRepository.cs
?   ?   ??? ... (other repositories)
?   ??? Program.cs
?   ??? appsettings.json
??? PersonalPortal.Web/
    ??? Components/
    ?   ??? Pages/                   # Razor pages
    ?   ?   ??? Home.razor
    ?   ?   ??? Login.razor
    ?   ?   ??? TextNotes.razor
    ?   ?   ??? TextNoteView.razor
    ?   ??? Layout/
    ?       ??? MainLayout.razor
    ??? Services/                    # Client services
    ?   ??? AuthService.cs
    ?   ??? ApiService.cs
    ??? Program.cs
    ??? appsettings.json
```

## API Endpoints

### Authentication
- `POST /api/auth/login` - Login with username/password
- `POST /api/auth/validate` - Validate authentication token

### Text Notes
- `GET /api/textnotes` - Get all text notes
- `GET /api/textnotes/{id}` - Get specific text note
- `GET /api/textnotes/latest/{count}` - Get latest N notes
- `GET /api/textnotes/search/{searchTerm}` - Search notes
- `POST /api/textnotes` - Create new note
- `PUT /api/textnotes/{id}` - Update note
- `DELETE /api/textnotes/{id}` - Delete note

### Checklists
- `GET /api/checklists` - Get all checklists
- `GET /api/checklists/{id}` - Get specific checklist
- `GET /api/checklists/latest/{count}` - Get latest N checklists
- `GET /api/checklists/type/{type}` - Get checklists by type
- `GET /api/checklists/search/{searchTerm}` - Search checklists
- `POST /api/checklists` - Create new checklist
- `PUT /api/checklists/{id}` - Update checklist
- `DELETE /api/checklists/{id}` - Delete checklist

### Recipes
- `GET /api/recipes` - Get all recipes
- `GET /api/recipes/{id}` - Get specific recipe
- `GET /api/recipes/latest/{count}` - Get latest N recipes
- `GET /api/recipes/type/{type}` - Get recipes by type
- `GET /api/recipes/search/{searchTerm}` - Search recipes
- `POST /api/recipes` - Create new recipe
- `PUT /api/recipes/{id}` - Update recipe
- `DELETE /api/recipes/{id}` - Delete recipe

### Pictures
- `GET /api/pictures` - Get all pictures
- `GET /api/pictures/{id}` - Get specific picture
- `GET /api/pictures/latest/{count}` - Get latest N pictures
- `GET /api/pictures/recipe/{recipeId}` - Get pictures for a recipe
- `POST /api/pictures` - Upload new picture
- `PUT /api/pictures/{id}` - Update picture
- `DELETE /api/pictures/{id}` - Delete picture

## Usage

### Home Page
The home page displays:
- Latest 3 text notes
- Latest 3 checklists
- Latest 3 recipes
- Carousel of latest 10 pictures

Each section has buttons to:
- Add new items
- View all items

### Administrative Pages
Each item type has an administrative page with:
- List view of all items
- Search functionality
- Sortable columns
- Filter by type (for checklists and recipes)
- Edit and Delete actions
- Add new item button

### Read-only View Pages
Each item can be accessed directly via its GUID URL for read-only viewing:
- `/textnotes/{id}`
- `/checklists/{id}`
- `/recipes/{id}`
- `/pictures/{id}`

These pages are accessible without authentication (future enhancement).

## Security Notes

?? **Important**: This is a basic implementation. For production use, consider:

1. **Password Hashing**: Currently passwords are stored in plain text. Use proper password hashing (bcrypt, PBKDF2, or Argon2).
2. **JWT Tokens**: Implement proper JWT-based authentication instead of simple token strings.
3. **HTTPS**: Always use HTTPS in production.
4. **CORS**: Configure CORS appropriately for your deployment environment.
5. **SQL Injection**: Dapper protects against SQL injection, but always validate input.
6. **Authorization**: Implement proper role-based authorization for Admin vs Viewer roles.
7. **Cookie Security**: Implement secure cookie handling with HttpOnly and Secure flags.

## Future Enhancements

- [ ] Implement proper JWT authentication
- [ ] Add password hashing
- [ ] Enable public read-only access via GUID links
- [ ] Add pagination for large lists
- [ ] Implement rich text editor for recipes
- [ ] Add image upload interface with preview
- [ ] Export/import functionality
- [ ] Tags and categories
- [ ] Full-text search across all content
- [ ] Mobile app
- [ ] Dark mode

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure database exists and tables are created

### API Not Accessible from Web
- Check both projects are running
- Verify API URL in Web appsettings.json
- Check CORS configuration in API Program.cs

### Bootstrap Not Loading
- Ensure Bootstrap files are in wwwroot/lib/bootstrap
- Check browser console for errors
- Run `dotnet restore` in Web project

## License

This is a personal project. Feel free to use and modify as needed.

## Support

For issues or questions, please create an issue in the repository.
