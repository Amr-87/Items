# Store and Item Management System

This project is a web application for managing stores and their associated items. It allows users to add, update, and delete items for each store. The application is built using ASP.NET Core MVC, Entity Framework Core, and SQL Server.

## Features

- View all stores and their items
- Add new items to stores
- Update quantities of existing items in stores
- Remove items from stores if quantity is zero
- Manage items that are not currently in a store

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or any code editor of your choice

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/Amr-87/Items.git
cd items

### 2. Set Up the Database
You have two options to set up the database:

Option 1: Restore from a Backup File
Open SQL Server Management Studio (SSMS).
Right-click on Databases and select Restore Database....
Choose the Device option and browse to select your backup file.
Follow the prompts to restore the database.
Option 2: Update Database from the NuGet Package Manager Console
Update the connection string in appsettings.json:
"ConnectionStrings": {
  "conn1": "Server=your_server_name;Database=your_database_name;Trusted_Connection=True;MultipleActiveResultSets=true"
}
Open the NuGet Package Manager Console in Visual Studio: Tools > NuGet Package Manager > Package Manager Console.

Run the following commands to apply migrations and update the database:
Update-Database
dotnet run

### 3. Navigate to https://localhost:5001 (or http://localhost:5000 for HTTP) to see the application running.
```
