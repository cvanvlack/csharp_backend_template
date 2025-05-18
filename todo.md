# TODO

## Phase 0 — Bootstrap

### 0.1 Repository Initialization

* [x] **0.1.1** Create project root folder; run `git init`
* [x] **0.1.2** Create `.githooks/` directory and set `core.hooksPath` to `.githooks`
* [x] **0.1.3** Commit initial empty state: `git commit --allow-empty -m "chore: bootstrap repo"`

### 0.2 License & Documentation Stubs

* [x] **0.2.1** Add `LICENSE` file with MIT template (fill year and author)
* [x] **0.2.2** Create `README.md` stub including project name and Sections placeholder: Overview, Requirements, Getting Started
* [x] **0.2.3** Commit: `git add LICENSE README.md` → `git commit -m "chore: add license and README stub"`

### 0.3 Solution & Project Scaffolding

* [x] **0.3.1** Run `dotnet new sln -n CsharpBackendService` → generates `.sln`
* [x] **0.3.2** Scaffold Web API: `dotnet new webapi -n CsharpBackendService -o src/CsharpBackendService --framework net8.0 --no-https`
* [x] **0.3.3** Add API project to solution: `dotnet sln add src/CsharpBackendService/CsharpBackendService.csproj`
* [x] **0.3.4** Delete sample `WeatherForecast.cs` and `WeatherForecastController.cs`
* [x] **0.3.5** Commit: `git add src/CsharpBackendService` → `git commit -m "chore: scaffold webapi project"`

### 0.4 Test Project Setup

* [x] **0.4.1** Create NUnit test project: `dotnet new nunit -n CsharpBackendService.Tests -o tests/CsharpBackendService.Tests`
* [x] **0.4.2** Add test project to solution: `dotnet sln add tests/CsharpBackendService.Tests/CsharpBackendService.Tests.csproj`
* [x] **0.4.3** Add project reference: edit `tests/CsharpBackendService.Tests/CsharpBackendService.Tests.csproj` ⇒ `<ProjectReference Include="../../src/CsharpBackendService/CsharpBackendService.csproj" />`
* [x] **0.4.4** Commit: `git add tests` → `git commit -m "chore: add test project"`

### 0.5 Baseline Build & Verification

* [x] **0.5.1** Run `dotnet build --configuration Release`
* [x] **0.5.2** Ensure no errors/warnings
* [x] **0.5.3** Commit build baseline: `git commit -m "chore: baseline build successful"`

---

## Phase 1 — Core Domain

### 1.1 Model Definition

* [x] **1.1.1** Create folder `src/CsharpBackendService/Models`
* [x] **1.1.2** Add `Todo.cs` with:

  ```csharp
  namespace CsharpBackendService.Models;
  public record Todo(Guid Id, string Title, string Description, bool Done);
  ```
* [x] **1.1.3** Enable nullable reference types in `CsharpBackendService.csproj`:

  ```xml
  <Nullable>enable</Nullable>
  ```
* [x] **1.1.4** Build to verify compilation
* [x] **1.1.5** Commit: `git add Models/Todo.cs` → `git commit -m "feat: add Todo record model"`

### 1.2 Store Interface

* [x] **1.2.1** Create folder `src/CsharpBackendService/Services`
* [x] **1.2.2** Add `ITodoStore.cs`:

  ```csharp
  namespace CsharpBackendService.Services;
  public interface ITodoStore
  {
    IEnumerable<Todo> GetAll();
    Todo? Get(Guid id);
    Todo Create(Todo todo);
    bool Replace(Guid id, Todo replacement);
    bool Delete(Guid id);
  }
  ```
* [x] **1.2.3** Add `using CsharpBackendService.Models;`
* [x] **1.2.4** Build & commit: `git commit -m "feat: define ITodoStore interface"`

### 1.3 In‑Memory Implementation

* [x] **1.3.1** Create `TodoStore.cs` in `Services`
* [x] **1.3.2** Add `ConcurrentDictionary<Guid, Todo>` field
* [x] **1.3.3** Implement `GetAll()` ⇒ return `Values`
* [x] **1.3.4** Implement `Get(id)` ⇒ `TryGetValue`
* [x] **1.3.5** Implement `Create(todo)` ⇒ assign `Id` if empty; `TryAdd`
* [x] **1.3.6** Implement `Replace(id, replacement)` ⇒ `TryUpdate`
* [x] **1.3.7** Implement `Delete(id)` ⇒ `TryRemove`
* [x] **1.3.8** Build & commit: `git commit -m "feat: implement TodoStore in-memory"`

### 1.4 Unit Tests for Store

* [x] **1.4.1** In `tests/CsharpBackendService.Tests/TodoStoreTests.cs`, import `Services` and `Models`
* [x] **1.4.2** Write test: creating with empty ID generates new GUID
* [x] **1.4.3** Write test: `Replace` returns false when ID not found
* [x] **1.4.4** Write test: `Delete` removes existing and returns true
* [x] **1.4.5** Write test: `GetAll` returns all items
* [x] **1.4.6** Write test: `Get(id)` returns correct item or null
* [x] **1.4.7** Run `dotnet test` to verify failures→green
* [x] **1.4.8** Commit: `git commit -m "test: add TodoStore unit tests"`

---

## Phase 2 — HTTP Surface

### 2.1 Controller Skeleton

* [x] **2.1.1** Create `src/CsharpBackendService/Controllers` folder
* [x] **2.1.2** Add `TodosController.cs` with:

  ```csharp
  namespace CsharpBackendService.Controllers;
  [ApiController]
  [Route("api/[controller]")]
  public class TodosController : ControllerBase
  {
    private readonly ITodoStore _store;
    public TodosController(ITodoStore store) => _store = store;
  }
  ```
* [x] **2.1.3** Add `using Microsoft.AspNetCore.Mvc;` and `using CsharpBackendService.Services;`
* [x] **2.1.4** Build & commit: `git commit -m "feat: scaffold TodosController"`

### 2.2 Dependency Injection

* [x] **2.2.1** In `Program.cs`, add:

  ```csharp
  builder.Services.AddSingleton<ITodoStore, TodoStore>();
  ```
* [x] **2.2.2** Add `using CsharpBackendService.Services;`
* [x] **2.2.3** Run `dotnet run` and verify startup logs
* [x] **2.2.4** Commit: `git commit -m "feat: register ITodoStore for DI"`

### 2.3 CRUD Action Methods

* [x] **2.3.1** Add `GET /api/todos` endpoint
* [x] **2.3.2** Add `POST /api/todos` endpoint with 201 Created
* [x] **2.3.3** Add `GET /api/todos/{id}` endpoint with 200/404
* [x] **2.3.4** Add `PUT /api/todos/{id}` endpoint with 204/404
* [x] **2.3.5** Add `DELETE /api/todos/{id}` endpoint with 204/404
* [x] **2.3.6** Build & manual smoke test via Postman/cURL
* [x] **2.3.7** Commit: `git commit -m "feat: implement full CRUD endpoints"`

### 2.4 Swagger Documentation Attributes

* [x] **2.4.1** Add `[ProducesResponseType]` attributes to controller methods
* [x] **2.4.2** Build & run; visit `/swagger/index.html`
* [x] **2.4.3** Commit: `git commit -m "docs: add ProducesResponseType for Swagger"`

### 2.5 Integration Tests

* [x] **2.5.1** Add `Microsoft.AspNetCore.Mvc.Testing` to test project
* [x] **2.5.2** Create `TodoEndpointTests.cs` in tests folder
* [x] **2.5.3** Configure `WebApplicationFactory<Program>` fixture
* [x] **2.5.4** Write tests for GET, POST, GET by ID, PUT, DELETE scenarios
* [x] **2.5.5** Run `dotnet test`; ensure all pass
* [x] **2.5.6** Commit: `git commit -m "test: add integration tests for Todo API"`

---

## Phase 3 — Quality Gates

### 3.1 StyleCop & .editorconfig

* [x] **3.1.1** Add `StyleCop.Analyzers` package: `dotnet add src/CsharpBackendService/CsharpBackendService.csproj package StyleCop.Analyzers --version <latest>`
* [x] **3.1.2** Create root `.editorconfig` with strict Microsoft C# rules (indentation, naming, file-scoped namespaces)
* [x] **3.1.3** (Optional) Add `stylecop.json` for rule customizations
* [x] **3.1.4** Commit: `git commit -m "chore: configure StyleCop and editorconfig"`

### 3.2 Project Settings

* [x] **3.2.1** Edit `src/CsharpBackendService/CsharpBackendService.csproj` to include:

  ```xml
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  <Nullable>enable</Nullable>
  ```
* [x] **3.2.2** Build and verify no warnings
* [x] **3.2.3** Commit: `git commit -m "chore: treat warnings as errors and enable nullable"`

### 3.3 Git Hooks

* [x] **3.3.1** Create `.githooks/pre-commit.ps1` with:

  ```powershell
  dotnet format --verify-no-changes
  dotnet build -c Release
  dotnet test -c Release --collect:"XPlat Code Coverage"
  ```
* [x] **3.3.2** Create `.githooks/install.ps1` to set `git config core.hooksPath .githooks`
* [x] **3.3.3** Run install script locally and verify hook execution
* [x] **3.3.4** Commit: `git commit -m "chore: add pre-commit hook scripts"`

---

## Phase 4 — Developer UX

### 4.1 Swagger

* [x] **4.1.1** Add `Swashbuckle.AspNetCore` package: `dotnet add src/CsharpBackendService/CsharpBackendService.csproj package Swashbuckle.AspNetCore --version <latest>`
* [x] **4.1.2** In `Program.cs`, add:

  ```csharp
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();
  ```
* [x] **4.1.3** In request pipeline (`Program.cs`):

  ```csharp
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }
  ```
* [x] **4.1.4** Commit: `git commit -m "feat: add Swagger documentation"`

### 4.2 CORS Policy

* [x] **4.2.1** In `Program.cs`, add CORS service:

  ```csharp
  builder.Services.AddCors(options =>
  {
    options.AddPolicy("AllowAll", policy =>
      policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
  });
  ```
* [x] **4.2.2** In request pipeline, add `app.UseCors("AllowAll");`
* [x] **4.2.3** Test CORS from a sample front-end client
* [x] **4.2.4** Commit: `git commit -m "feat: configure CORS policy"`

### 4.3 README Updates

* [x] **4.3.1** Update `README.md` with:

  * Project overview
  * Setup instructions
  * Running & testing commands
  * Swagger UI URL
* [x] **4.3.2** Commit: `git commit -m "docs: update README with usage instructions"`

---

## Phase 5 — Hosting Modes

### 5.1 Windows Service Support

* [x] **5.1.1** Add `Microsoft.Extensions.Hosting.WindowsServices` package: `dotnet add src/CsharpBackendService/CsharpBackendService.csproj package Microsoft.Extensions.Hosting.WindowsServices`
* [x] **5.1.2** In `Program.cs`, add:

  ```csharp
  if (!Environment.UserInteractive)
    builder.Host.UseWindowsService();
  ```
* [x] **5.1.3** Commit: `git commit -m "feat: enable Windows Service hosting"`

### 5.2 Service Name Argument

* [x] **5.2.1** Parse optional `--serviceName` argument via `Configuration`/`HostBuilder` in `Program.cs`
* [x] **5.2.2** Use serviceName when calling `UseWindowsService(options => options.ServiceName = serviceName)`
* [x] **5.2.3** Commit: `git commit -m "feat: add serviceName CLI override"`

### 5.3 Installer Stub

* [x] **5.3.1** Create `installer/` folder with `README.md` stub describing expected Wix configuration
* [x] **5.3.2** Commit: `git commit -m "chore: add installer project stub"`

---

## Phase 6 — Hardening

### 6.1 Global Error Handling

* [x] **6.1.1** In `Program.cs`, add:

  ```csharp
  app.UseExceptionHandler("/error");
  ```
* [x] **6.1.2** Map `/error` endpoint returning `ProblemDetails`:

  ```csharp
  app.Map("/error", (HttpContext http) =>
    Results.Problem(new ProblemDetails { Title = "An error occurred" }));
  ```
* [x] **6.1.3** Commit: `git commit -m "feat: add global error handler"`

### 6.2 Health Checks

* [x] **6.2.1** Add `Microsoft.AspNetCore.Diagnostics.HealthChecks` package: `dotnet add src/CsharpBackendService/CsharpBackendService.csproj package Microsoft.AspNetCore.Diagnostics.HealthChecks`
* [x] **6.2.2** In `Program.cs`, add:

  ```csharp
  builder.Services.AddHealthChecks();
  ```
* [x] **6.2.3** Map health endpoint: `app.MapHealthChecks("/healthz");`
* [x] **6.2.4** Test `/healthz` returns 200 OK
* [x] **6.2.5** Commit: `git commit -m "feat: add health checks"`

### 6.3 Logging Configuration

* [x] **6.3.1** In `appsettings.Production.json`, configure console logger to use JSON formatter
* [x] **6.3.2** In `Program.cs`, add:

  ```csharp
  builder.Logging.AddConsole();
  ```
* [x] **6.3.3** Test logs in Production vs Development output formats
* [x] **6.3.4** Commit: `git commit -m "chore: configure logging outputs"`

---

## Phase 7 — Packaging

### 7.1 Publish Script

* [x] **7.1.1** Create `scripts/publish.ps1`:

  ```powershell
  dotnet publish src/CsharpBackendService -c Release -o out/
  ```
* [x] **7.1.2** Run script and verify `out/` directory contains published binaries
* [x] **7.1.3** Commit: `git commit -m "chore: add publish script"`

### 7.2 Code Coverage

* [x] **7.2.1** Add `coverlet.collector` to test project: `dotnet add tests/CsharpBackendService.Tests/CsharpBackendService.Tests.csproj package coverlet.collector --version <latest>`
* [x] **7.2.2** Update test project `.csproj` to include `<CollectCoverage>true</CollectCoverage>` if desired
* [x] **7.2.3** Run `dotnet test --collect:"XPlat Code Coverage"` and verify output in `TestResults/`
* [x] **7.2.4** Commit: `git commit -m "test: enable code coverage collection"`

### 7.3 CI Workflow Stub

* [x] **7.3.1** Create `.github/workflows/ci.yml` with build, test, coverage steps
* [x] **7.3.2** Include job matrix for `dotnet build`, `dotnet test --collect:"XPlat Code Coverage"`
* [x] **7.3.3** Commit: `git commit -m "chore: add CI workflow stub"`
