# C# ASP.NET Core 8 Todo API Template – Specification

A minimal yet production‑hardened template that demonstrates modern C# backend best practices while remaining small enough to skim in minutes. It mirrors the functionality of the original FastAPI Todo example but is opinionated about structure, safety, and code quality.

---

## 1  Goals & Non‑Goals

|                          | In Scope                                                                        | Out of Scope                              |
| ------------------------ | ------------------------------------------------------------------------------- | ----------------------------------------- |
| **Demonstrate concepts** | Controllers, strict analyzers, nullable safety, Swagger, unit/integration tests | Full persistence layer, orchestrators     |
| **Project structure**    | Single `WebApi` project + test project                                          | Multi‑layer DDD architecture              |
| **Quality gates**        | StyleCop, `dotnet format`, warnings‑as‑errors, Git pre‑commit hook              | CI workflow, code‑coverage threshold gate |
| **Hosting**              | Console/Kestrel **and** Windows‑Service auto‑detect                             | Containerisation, cloud deploy YAMLs      |
| **Docs**                 | OpenAPI v3 with Swagger UI                                                      | External README generation                |

---

## 2  Requirements

| Tool                                                         | Version / Channel | Purpose                       |
| ------------------------------------------------------------ | ----------------- | ----------------------------- |
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0) | **8 LTS**         | Build & run                   |
| PowerShell (Core/Windows)                                    | ≥ 7.4             | Pre‑commit hook               |
| Git (with hooks)                                             | —                 | Version control & local gates |
| (Optional) Visual Studio 2022 / Rider                        | Latest            | IDE experience                |

> All compiler warnings are treated as errors via `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`.

---

## 3  Project Structure

```text
/
├── CsharpBackendService.sln
├── src/
│   └── CsharpBackendService/
│       ├── CsharpBackendService.csproj
│       ├── Program.cs                # Host + DI wiring
│       ├── Controllers/
│       │   └── TodosController.cs    # CRUD endpoints
│       ├── Models/
│       │   └── Todo.cs               # Record type (immutable)
│       ├── Services/
│       │   └── TodoStore.cs          # Thread‑safe in‑memory store
│       └── appsettings*.json         # Env‑specific config (git‑ignored)
└── tests/
    └── CsharpBackendService.Tests/
        ├── CsharpBackendService.Tests.csproj
        ├── TodoEndpointTests.cs      # Integration tests
        └── TodoStoreTests.cs         # Unit tests
```

### Namespaces

`CsharpBackendService.[Subfolder]` – folder names mirror namespaces. Nullable reference types are **enabled**, and StyleCop rules enforce file‑scoped namespaces.

---

## 4  Implementation Details

### 4.1 Domain Model

```csharp
public record Todo(Guid Id, string Title, string Description, bool Done);
```

* Immutable `record` with non‑nullable properties.
* `Guid` primary key generated on insertion.

### 4.2 In‑Memory Store

```csharp
public interface ITodoStore
{
    IEnumerable<Todo> GetAll();
    Todo?            Get(Guid id);
    Todo             Create(Todo todo);
    bool             Replace(Guid id, Todo replacement);
    bool             Delete(Guid id);
}
```

`TodoStore` implements the interface with a `ConcurrentDictionary<Guid, Todo>`; thread‑safe for demo concurrent calls.

### 4.3 Controllers

* Controller class `TodosController : ControllerBase` decorated with `[ApiController]` and `[Route("api/[controller]")]`.
* CRUD actions returning ActionResult wrappers for proper status codes.
* `ProducesResponseType` attributes document responses for Swagger.

### 4.4 Program.cs

* Top‑level statements.
* `builder.Services.AddControllers();`
* `builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();`
* Dependency‑inject `ITodoStore` as a singleton.
* **Windows‑Service auto‑detect**:

  ```csharp
  if (!Environment.UserInteractive)
      builder.Host.UseWindowsService();
  ```

### 4.5 Logging & Error Handling

* Uses default Microsoft.Extensions.Logging (console JSON when `DOTNET_ENVIRONMENT != Production`).
* `app.UseExceptionHandler("/error");` returns ProblemDetails via minimal handler.
* Developer exception page enabled in Development.

---

## 5  API Contract

| Verb   | Path              | Summary          | Responses              |
| ------ | ----------------- | ---------------- | ---------------------- |
| GET    | `/api/todos`      | List all todos   | 200 OK (array of Todo) |
| GET    | `/api/todos/{id}` | Get by ID        | 200 OK / 404 Not Found |
| POST   | `/api/todos`      | Create new todo  | 201 Created + location |
| PUT    | `/api/todos/{id}` | Replace existing | 204 No Content / 404   |
| DELETE | `/api/todos/{id}` | Delete           | 204 No Content / 404   |

Swagger UI available at `/swagger` in Development.

---

## 6  Testing Strategy

| Layer       | Framework | Tooling                           | Notes                                                |
| ----------- | --------- | --------------------------------- | ---------------------------------------------------- |
| Unit        | NUnit     | Moq                               | Pure logic (TodoStore)                               |
| Integration | NUnit     | WebApplicationFactory             | In‑memory server, HTTP client                        |
| Coverage    | Coverlet  | `--collect:"XPlat Code Coverage"` | Generates Cobertura+LCOV; no hard threshold enforced |

> Test project targets `net8.0` and references the production project.

---

## 7  Code Quality & Conventions

* **StyleCop.Analyzers** + Roslyn default rules at **error** level.
* `.editorconfig` with Microsoft style guidelines (`dotnet_style_*, csharp_style_*`) and file header enforcement.
* `Nullable` enabled and included in warnings‑as‑errors.
* `dotnet format --verify-no-changes` executed in pre‑commit hook.

### Git Pre‑Commit Hook (`pre-commit.ps1`)

```powershell
& dotnet format --verify-no-changes
& dotnet build -c Release
& dotnet test -c Release --collect:"XPlat Code Coverage"
```

Install locally with:

```bash
pwsh .githooks/install.ps1  # sets core.hooksPath=.githooks
```

---

## 8  Windows‑Service Support

* Service is **named** `CsharpBackendService` (display name identical).
* Auto‑detect logic (non‑interactive) enables `UseWindowsService`.
* Running as a console app (`dotnet run`) remains the default for developers.
* The installer project (external) is expected to handle service registration.

---

## 9  Getting Started

```bash
# 1. Restore
 dotnet restore

# 2. Run with hot‑reload
 dotnet watch --project src/CsharpBackendService run
```

Browse Swagger UI at [http://localhost:5064/swagger](http://localhost:5064/swagger).

Run tests:

```bash
 dotnet test
```

---

## 10  Extensibility Guidance

* ✅ Swap `TodoStore` for a repository pattern + EF Core.
* ✅ Add Dockerfile & GitHub Actions when deployment is required.
* ✅ Introduce health checks with `AspNetCore.Diagnostics.HealthChecks`.

