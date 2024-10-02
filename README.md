# Project "Inventory Management System"

The project is still incomplete but has a bit of responsiveness and does have CRUD operations working.


## Description
This project is built using ASP.NET Core with Entity Framework Core and targets **.NET 8.0**. It includes identity management, database support for MySQL, and uses Entity Framework migrations. This project also supports MSSQL Server, but adjustments are needed in the `Program.cs` file for Dependency Injection when using MSSQL. Additionally, there may be adjustments required in the `Models/` attributes to complement MSSQL Server.

## Prerequisites

Before running the project, ensure you have the following installed:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [Node.js](https://nodejs.org/) (not used in the project, but can be used if needed for handling frontend assets)
- [Entity Framework Core Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) (if not installed globally)
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/projectname.git
   cd projectname
   ```

2. Install dependencies:
   - Run the following command to restore the required .NET and NuGet packages:
     ```bash
     dotnet restore
     ```

3. (Optional) If there's a need to install frontend dependencies (like for `wwwroot/js`):
   ```bash
   npm install
   ```

## Configuration

### Database Configuration

The project is using MySQL. You'll need to reconfigure the MySQL database connection string in the `appsettings.json` file.

#### Example for MySQL:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server_name;Database=your_db_name;User=your_username;Password=your_password;"
}
```

### Migrations

#### In Visual Studio (using Package Manager Console)

1. Open the **Package Manager Console** (Tools > NuGet Package Manager > Package Manager Console).
2. Run the following commands to manage migrations and update the database:

   - To add a new migration:
     ```powershell
     Add-Migration MigrationName
     ```

   - To update the database:
     ```powershell
     Update-Database
     ```

#### In Visual Studio Code (using CLI)

1. Open a terminal in **VS Code** or your preferred command-line tool.
2. Run the following commands to manage migrations and update the database:

   - To add a new migration:
     ```bash
     dotnet ef migrations add MigrationName
     ```

   - To update the database:
     ```bash
     dotnet ef database update
     ```

## Running the Project

### In Visual Studio Code

1. Open the project folder in **VS Code**:
   ```bash
   code .
   ```

2. Install the recommended extensions for C# development:
   - C# for Visual Studio Code (powered by OmniSharp)
   - MySQL-related extensions (if needed for database integration)

3. Start the project:
   - Open the terminal in VS Code and run:
     ```bash
     dotnet run
     ```

4. Open the application in your browser at `https://localhost:5001` (or the port specified in the launch settings).

### In Visual Studio

1. Open **Visual Studio** and select `Open a project or solution`.

2. Navigate to the folder where the project is located and open the `.csproj` file.

3. Restore the NuGet packages:
   - Visual Studio should automatically restore them, but you can manually trigger it by right-clicking on the solution in the Solution Explorer and selecting `Restore NuGet Packages`.

4. Run the project:
   - Press `F5` to start debugging or `Ctrl + F5` to run without debugging.

5. Open the application in your browser at `https://localhost:5001` (or the port specified in the launch settings).

## API Fetch Logic in wwwroot

The project handles frontend API fetch operations from the `wwwroot` directory, specifically in the `wwwroot/js/` folder together with other scripts. If you need to modify or extend API fetch logic, you can find the JavaScript responsible for it in this directory.

For example, an API call might look like this in `wwwroot/js/api-fetch.js`:
```javascript
fetch('/api/data')
  .then(response => response.json())
  .then(data => {
    console.log(data);
  })
  .catch(error => {
    console.error('Error fetching API:', error);
  });
```

## Project Structure

- `wwwroot/`: Contains static frontend assets, such as the logic for API fetch operations, stylesheets, images, font, frontend libraries.
- `Attributes/`: Contains custom model validation attributes.
- `Data/`: Contains the Database Context, json file about the author, and the valid value reference for dropdowns.
- `Migrations/`: Contains Entity Framework migrations.
- `Views/`: Contains the Razor views (HTML templates).
- `Controllers/`: Contains Api Controller and View Controller.
  - `api/`: Contains the backend logic for routing and request handling.
  - `main/`: Contans the view controller.
- `Models/`: Contains the Entity Framework models and database context.
 - `Utilities/`: Contains the Helpers or Utility classes for the project
  - `Api/`: Contains helper for Api responses and validation logics.
  - `Data/`: Contains claim helper, password hasher, and database seeding

## Tools and Packages Used

- **ASP.NET Core Identity** (`Microsoft.AspNetCore.Identity`)
- **Entity Framework Core** with MySQL support (`MySql.EntityFrameworkCore`, `omelo.EntityFrameworkCore.MySql`)
- **Razor Pages and MVC** for building the front-end views
- **Code Generation Tools** (`Microsoft.VisualStudio.Web.CodeGeneration.Design`) for scaffolding and other operations

## Contribution

Feel free to fork this repository and submit pull requests. Any improvements or bug fixes are welcome.

## License

Not license.
```
