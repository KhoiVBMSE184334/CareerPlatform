# Career Platform

Personalized Career Orientation & Learning Roadmap Platform for Software Engineering students.

## Tech Stack

Backend:
- ASP.NET Core 8 Web API
- Entity Framework Core 8
- SQL Server
- JWT authentication
- Swagger/OpenAPI

Frontend:
- React
- TypeScript
- Vite
- Tailwind CSS
- React Router
- Axios

## Backend Setup

From the repository root:

```powershell
cd backend\CareerPlatform
dotnet restore
dotnet build CareerPlatform.sln
```

Configure `CareerPlatform.API/appsettings.json` or user secrets before running the API.

Required placeholders:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=CareerPlatformDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Secret": "YOUR_LONG_LOCAL_DEVELOPMENT_SECRET",
    "Issuer": "CareerPlatform",
    "Audience": "CareerPlatform.Client",
    "ExpiryMinutes": 60
  },
  "OpenAI": {
    "ApiKey": "YOUR_OPENAI_API_KEY",
    "Model": "gpt-4o-mini"
  },
  "GitHub": {
    "Token": "YOUR_GITHUB_TOKEN"
  }
}
```

Run EF Core migrations:

```powershell
dotnet ef database update --project CareerPlatform.Infrastructure --startup-project CareerPlatform.API
```

If `dotnet ef` is not installed:

```powershell
dotnet tool install --global dotnet-ef
```

Run the API:

```powershell
dotnet run --project CareerPlatform.API --launch-profile http
```

Swagger:

```text
http://localhost:5240/swagger
```

The `https` launch profile is also configured for:

```text
https://localhost:7013/swagger
```

## Frontend Setup

From the repository root:

```powershell
cd frontend
npm install
npm run dev
```

The frontend Axios client defaults to:

```text
https://localhost:7001/api
```

If the backend is running with the included `http` launch profile, create `frontend/.env.local`:

```text
VITE_API_BASE_URL=http://localhost:5240/api
```

Build the frontend:

```powershell
npm run build
```

On Windows PowerShell, if `npm.ps1` is blocked by execution policy, use:

```powershell
npm.cmd run build
```

## Demo Accounts

Seed data includes these accounts:

| Role | Email |
| --- | --- |
| Admin | admin@career.com |
| Student | student@career.com |

The seed file stores password hashes, not plaintext passwords. Use the known seed password from your team setup, reset the password in the database, or register a new Student account through the UI.

## Verification

Latest local verification:

```powershell
dotnet build backend\CareerPlatform\CareerPlatform.sln
npm.cmd run build
```

Both commands completed successfully. Swagger was confirmed by starting the API with the `http` launch profile and requesting:

```text
http://localhost:5240/swagger/index.html
```
