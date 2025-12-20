# Azure Deployment Configuration

This directory contains configuration files for deploying the ASP.NET MVC application to Azure App Service.

## Prerequisites

1. An Azure subscription
2. An Azure App Service (Web App) created for hosting the application
3. GitHub repository secrets configured

## Required GitHub Secrets

To enable automated deployment, configure the following secrets in your GitHub repository:

### `AZURE_WEBAPP_NAME`
The name of your Azure App Service (Web App). This can be found in the Azure Portal.

**Example:** `my-website-mvc-app`

### `AZURE_WEBAPP_PUBLISH_PROFILE`
The publish profile for your Azure App Service. This contains authentication credentials for deployment.

**How to get the publish profile:**
1. Go to the Azure Portal
2. Navigate to your App Service
3. Click "Download publish profile" button
4. Open the downloaded `.publishsettings` file in a text editor
5. Copy the entire contents
6. Paste it as the value for this secret in GitHub

## Setting Up GitHub Secrets

1. Go to your GitHub repository
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**
4. Add each secret with the name and value as described above

## Deployment Workflow

The deployment workflow is triggered automatically when:
- Code is pushed to the `main` branch (including merged pull requests)
- Manual trigger via GitHub Actions UI (workflow_dispatch)

**Note:** Pull requests to `main` do not trigger deployment automatically. Deployment only occurs after code is merged or directly pushed to the `main` branch.

### Workflow Steps

1. **Checkout code** - Downloads the repository code
2. **Setup .NET** - Configures .NET 8.0 environment
3. **Restore dependencies** - Downloads NuGet packages
4. **Build** - Compiles the solution in Release configuration
5. **Run tests** - Executes unit tests to verify build quality
6. **Publish** - Creates deployment package
7. **Deploy to Azure** - Deploys the package to Azure App Service

## Configuration File

`config.json` contains metadata about the deployment configuration:
- Runtime framework and version
- Build configuration
- Deployment settings
- Required secrets list

This file is for documentation purposes and is not directly used by the deployment workflow.

## Troubleshooting

### Build Failures
- Check that all NuGet packages are properly restored
- Verify .NET 8.0 SDK is being used
- Review build logs in GitHub Actions

### Test Failures
- The workflow will fail if tests don't pass
- Fix failing tests before deployment
- Tests run in Release configuration

### Deployment Failures
- Verify `AZURE_WEBAPP_NAME` matches your Azure App Service name exactly
- Ensure `AZURE_WEBAPP_PUBLISH_PROFILE` is up-to-date
- Check Azure App Service logs for runtime errors
- Verify the App Service is running and not stopped

## Local Testing

To test the publish step locally:

```bash
dotnet publish "Website(MVC)/Website(MVC).csproj" --configuration Release --output ./publish
```

This creates the same deployment package that will be deployed to Azure.
