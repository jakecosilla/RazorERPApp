# RazorERPApp

RazorERPApp is a web API application designed to manage users and companies with roles for Admins and Users. It utilizes JWT authentication, CQRS, MediatR, FluentValidation, and Dapper for data access.

## Prerequisites

Before you can run the RazorERPApp, ensure you have the following prerequisites:

1. **.NET 6 SDK**: Ensure that you have .NET 6 SDK installed. You can download it from [here](https://dotnet.microsoft.com/download/dotnet/6.0).
2. **SQL Server**: Ensure you have SQL Server installed and running.
3. **Database Setup**: Run the scripts provided in the RazorERPDB repository to set up the necessary database and stored procedures.
   - Clone the RazorERPDB repository: `git clone https://github.com/jakecosilla/RazorERPDB.git`
   - Follow the instructions in the RazorERPDB repository to set up the database and stored procedures.

## Getting Started

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/RazorERPApp.git
   cd RazorERPApp

2 . **Set RazorERP.API as Startup Project**
  
  In Visual Studio:
   - Right-click on the RazorERP.API project in the Solution Explorer.
   - Select Set as Startup Project.

## Running the Application

1. **Build and Run**
     - In Visual Studio, build and run the application by pressing F5 or clicking on the Start button.
2. **Swagger UI**
     - The Swagger UI will be available at https://localhost:5001/swagger or http://localhost:5000/swagger (depending on your HTTPS settings). Use it to explore and test the API endpoints.
3. **Update appsettings.json**
     - Update the appsettings.json file in the RazorERP.API project to configure the connection string for your database and JWT settings.

    ```json
    {
      "ConnectionStrings": {
        "RazorERPDB": "Server=(local);Database=RazorERPDB;User Id=<your_db_userid>;Password=<your_db_password>;TrustServerCertificate=True;"
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Warning",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      },
      "AllowedHosts": "*",
      "Jwt": {
        "Key": "your-secure-key",
        "Issuer": "your-issuer",
        "Audience": "your-audience",
        "ExpiryMinutes": 60
      }
    }
    ```
4. **Update JWT in launchSettings.json**
   Update  "JWT_KEY": "<your-secure-key>" same from  appsettings.json
      ```json
      {
        "$schema": "http://json.schemastore.org/launchsettings.json",
        "iisSettings": {
          "windowsAuthentication": false,
          "anonymousAuthentication": true,
          "iisExpress": {
            "applicationUrl": "http://localhost:62350",
            "sslPort": 44366
          }
        },
        "profiles": {
          "http": {
            "commandName": "Project",
            "dotnetRunMessages": true,
            "launchBrowser": true,
            "launchUrl": "swagger",
            "applicationUrl": "http://localhost:5168",
            "environmentVariables": {
              "ASPNETCORE_ENVIRONMENT": "Development",
              "JWT_KEY": "ThisIsASecretKeyForJwtTokenGeneration12345!"
            }
          },
          "https": {
            "commandName": "Project",
            "dotnetRunMessages": true,
            "launchBrowser": true,
            "launchUrl": "swagger",
            "applicationUrl": "https://localhost:7114;http://localhost:5168",
            "environmentVariables": {
              "ASPNETCORE_ENVIRONMENT": "Development",
              "JWT_KEY": "ThisIsASecretKeyForJwtTokenGeneration12345!"
            }
          },
          "IIS Express": {
            "commandName": "IISExpress",
            "launchBrowser": true,
            "launchUrl": "swagger",
            "environmentVariables": {
              "ASPNETCORE_ENVIRONMENT": "Development",
              "JWT_KEY": "ThisIsASecretKeyForJwtTokenGeneration12345!"
            }
          }
        }
      }
      ```

## Testing
  Unit tests are available for the application. To run the tests:

1. **Open Test Explorer**

  In Visual Studio, go to Test > Test Explorer.

2. **Run All Tests**

  - Click Run All to execute the unit tests.

## Project Structure
- RazorERP.Core: Contains the domain models, interfaces, and application logic.
- RazorERP.Infrastructure: Contains the data access implementation using Dapper and the SQL connection factory.
- RazorERP.Web: Contains the API controllers and middleware setup.
