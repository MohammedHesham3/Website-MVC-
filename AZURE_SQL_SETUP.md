# Azure SQL Database Configuration

## Overview
This application uses SQL Server for data persistence, supporting both local development and Azure SQL Database for production deployment.

## Local Development Setup

The application is configured to use a local SQL Server instance by default. The connection string in `appsettings.json` uses Windows Authentication:

```
Server=.;Database=FoodTrackerDb;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true
```

### Prerequisites
- SQL Server LocalDB or SQL Server Express installed
- .NET 8.0 SDK

### Running Locally
1. The application will automatically apply migrations and create the database on first run in Development mode
2. Sample data (Apple) is defined in `ApplicationDbContext.cs` using Entity Framework's `HasData()` method, which is included in the initial migration

## Azure Deployment Setup

### 1. Create Azure SQL Database

Create an Azure SQL Database using the Azure Portal, Azure CLI, or Azure Resource Manager:

```bash
# Using Azure CLI
az sql server create --name <server-name> --resource-group <resource-group> --location <location> --admin-user <admin-username> --admin-password <admin-password>

az sql db create --resource-group <resource-group> --server <server-name> --name FoodTrackerDb --service-objective S0
```

### 2. Configure Firewall Rules

Allow your Azure Web App to access the database:

```bash
az sql server firewall-rule create --resource-group <resource-group> --server <server-name> --name AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0
```

### 3. Set Connection String in Azure

> **⚠️ SECURITY WARNING:** The `appsettings.Production.json` file contains a placeholder connection string template. **NEVER commit actual database credentials to source control!** Always use Azure Key Vault or App Service Application Settings to manage sensitive connection strings.

In your Azure Web App configuration, add the connection string as an environment variable or in Application Settings:

**Name:** `ConnectionStrings__DefaultConnection`

**Value:**
```
Server=tcp:<your-server>.database.windows.net,1433;Initial Catalog=FoodTrackerDb;Persist Security Info=False;User ID=<your-username>;Password=<your-password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

> **Security Note:** Never commit actual credentials to source control. Use Azure Key Vault or App Service Application Settings to manage sensitive connection strings.

### 4. Run Database Migrations

Before deploying the application, ensure the database schema is created:

**Option A: Using EF Core CLI**
```bash
dotnet ef database update --connection "your-connection-string"
```

**Option B: Using Azure DevOps or GitHub Actions**
Add a deployment step to run migrations automatically:
```yaml
- name: Run EF Migrations
  run: dotnet ef database update --project Website(MVC)
  env:
    ConnectionStrings__DefaultConnection: ${{ secrets.AZURE_SQL_CONNECTION_STRING }}
```

**Option C: Enable automatic migrations in production (not recommended for production)**
Modify `Program.cs` to run `context.Database.Migrate()` on startup (currently disabled in production).

## Database Seeding

Sample data (Apple) is configured in `ApplicationDbContext.cs` using the `OnModelCreating` method and is included in the initial migration file (`20251217180715_InitialCreate.cs`). This data will be automatically seeded when migrations are applied using `dotnet ef database update` or `context.Database.Migrate()`.

## Troubleshooting

### Connection Issues
- Verify firewall rules allow your IP or Azure services
- Check that the connection string format is correct for Azure SQL
- Ensure SQL authentication is enabled if not using Managed Identity

### Migration Issues
- Run migrations manually before first deployment
- Check that the EF Core tools are installed: `dotnet tool install --global dotnet-ef`

## Alternative: Using Managed Identity

For enhanced security, configure your Azure Web App to use Managed Identity to connect to Azure SQL Database without storing credentials:

1. Enable Managed Identity on your Web App
2. Grant database permissions to the Managed Identity
3. Use a connection string without credentials:
   ```
   Server=tcp:<your-server>.database.windows.net,1433;Initial Catalog=FoodTrackerDb;Authentication=Active Directory Managed Identity;
   ```
