# CsharpBackendService – Development Plan

## 0. Bird’s‑Eye Blueprint

| Phase                | Goal                                         | Key Deliverables                                                   |
| -------------------- | -------------------------------------------- | ------------------------------------------------------------------ |
| **0. Bootstrap**     | Empty repo ➜ runnable skeleton               | Git repo, solution, Web API project, Test project, CI placeholder  |
| **1. Core Domain**   | CRUD domain modeled & unit‑tested            | `Todo` record, `ITodoStore`, concurrent in‑memory impl, unit tests |
| **2. HTTP Surface**  | REST endpoints exposed & integration‑tested  | `TodosController`, validation filters, problem‑details handler     |
| **3. Quality Gates** | Code analysis & formatting block regressions | StyleCop, nullable strictness, pre‑commit PowerShell hook          |
| **4. Developer UX**  | Swagger docs, hot‑reload, README             | Swashbuckle, example cURL scripts                                  |
| **5. Hosting Modes** | Console *and* Windows‑service                | `UseWindowsService` toggle, sample installer stub                  |
| **6. Hardening**     | Robust error handling, logging, health probe | `/error` endpoint, `HealthChecks`, structured logging              |
| **7. Packaging**     | Release artefacts & coverage report          | `dotnet publish` release, Coverlet LCOV, draft GitHub Action       |

> **Scope discipline:** Each phase 🤝 single PR ‑> trunk; CI optional but recommended.

---

## 1. Iterative Chunks (Phase ➜ Chunk)

### Phase 0 — Bootstrap

* **0.1** Initialise Git repo, `.gitignore` (dotnet + VS + Rider presets).
* **0.2** `dotnet new sln` ➜ `dotnet new webapi` (`CsharpBackendService`) & add to solution.
* **0.3** `dotnet new nunit` test project under `tests/`; reference web project.
* **0.4** Run `dotnet build` & commit green baseline.

### Phase 1 — Core Domain

* **1.1** Create `Todo` record (immutable, non‑nullable).
* **1.2** Define `ITodoStore` interface.
* **1.3** Implement `TodoStore` with `ConcurrentDictionary`.
* **1.4** Write unit tests for all store methods.

### Phase 2 — HTTP Surface

* **2.1** Add `TodosController` with placeholder `GET /api/todos` returning empty list.
* **2.2** Wire DI (`builder.Services.AddSingleton<ITodoStore, TodoStore>()`).
* **2.3** Implement full CRUD in controller.
* **2.4** Integration tests using `WebApplicationFactory` for every endpoint & status path.

### Phase 3 — Quality Gates

* **3.1** Add `StyleCop.Analyzers` + `.editorconfig` strict preset.
* **3.2** Enable `<TreatWarningsAsErrors>` & `<Nullable>enable</Nullable>`.
* **3.3** Add `dotnet format` & `pre-commit.ps1`; document hook installation.

### Phase 4 — Developer UX

* **4.1** Add SwaggerGen & SwaggerUI services (dev‑only).
* **4.2** Enable CORS *any origin* (config‑driven) to ease front‑end testing.
* **4.3** Update README with local run, test, swagger instructions.

### Phase 5 — Hosting Modes

* **5.1** Add `if (!Environment.UserInteractive) UseWindowsService()`.
* **5.2** Expose `--serviceName` CLI arg fallback.
* **5.3** Stub Wix/MSI installer project (external) referencing published exe.

### Phase 6 — Hardening

* **6.1** Add global exception handler ➜ RFC7807 ProblemDetails.
* **6.2** Add `/healthz` using `AspNetCore.HealthChecks`.
* **6.3** Configure logging: JSON when `Production`, console‑pretty otherwise.

### Phase 7 — Packaging

* **7.1** `dotnet publish -c Release -o out/` script.
* **7.2** Integrate Coverlet data ➜ `coverage/` (Cobertura & LCOV).
* **7.3** Draft (commented) GitHub Action YAML for build + test.

---

## 2. Atomic Steps (Third Pass)

Below, each chunk is decomposed to the smallest *safe* step. Each ID is unique for traceability.

| ID        | Description                                      | Expected Artifact               | Tests?               |
| --------- | ------------------------------------------------ | ------------------------------- | -------------------- |
| **0.1.1** | Create repo root folder, initialise git          | `.git`                          | –                    |
| **0.1.2** | Add MIT LICENSE & minimal README stub            | `LICENSE`, `README.md`          | –                    |
| **0.1.3** | Add solution `CsharpBackendService.sln`          | `.sln`                          | build passes         |
| **0.1.4** | Scaffold Web API project                         | `src/CsharpBackendService/*.cs` | dotnet build         |
| **0.1.5** | Remove WeatherForecast sample & update namespace | clean Program.cs                | build                |
| **0.1.6** | Scaffold NUnit test project                      | `tests/*.cs`                    | `dotnet test` passes |
| **0.1.7** | Commit baseline ✅                                | VCS commit                      | –                    |
| **1.1.1** | Add `Models/Todo.cs` record                      | `Todo.cs`                       | unit compile         |
| **1.1.2** | Add failing test $todo absent$                   | `TodoStoreTests.cs` red         | ❌                    |
| **…**     | *continue pattern through 7.3.3*                 |                                 |                      |

*(Full atomic table continues to 7.3.3 – omitted here for brevity; see TODO list.)*

---

## 3. LLM Code‑Generation Prompts

Each prompt lives in a fenced `text` block so the LLM treats it verbatim.

### Prompt P‑00 — Repo Bootstrap

```text
You are ChatGPT‑Coder paired with a senior C# reviewer.
Task: Create an empty git repository with a .gitignore tailored for .NET and Rider.
Steps:
1. Run `git init`.
2. Add standard .NET `.gitignore` (source: GitHub templates).
3. Commit as “chore: initial repo”.
Output: none – just perform file ops.
Tests: `git status` shows clean.
```

### Prompt P‑01 — Solution & Projects

```text
Goal: Create solution & webapi + test projects without sample weather code.
Commands:
- `dotnet new sln -n CsharpBackendService`
- `dotnet new webapi -n CsharpBackendService -o src/CsharpBackendService --framework net8.0 --no-https`
- `dotnet sln add src/CsharpBackendService`
- `dotnet new nunit -n CsharpBackendService.Tests -o tests/CsharpBackendService.Tests`
- `dotnet sln add tests/CsharpBackendService.Tests`
- Delete WeatherForecast*, WeatherController*.
- Update namespaces.
Run `dotnet build` – expect success.
```

### Prompt P‑02 — Domain Model

```text
Add immutable record `Todo` in Models folder.
Properties:
- Guid Id
- string Title
- string Description
- bool Done
All props non‑nullable.
Update project file `CsharpBackendService.csproj` to enable `<Nullable>enable</Nullable>` if not yet enabled.
Add unit test asserting value‑equality of two records with same data.
```

### Prompt P‑03 — TodoStore Interface & Impl

```text
Create interface ITodoStore and class TodoStore backed by ConcurrentDictionary.
Implement CRUD.
Add unit tests verifying:
- Create returns entity with generated Guid when input Id == Guid.Empty.
- Replace returns false when id not found.
- Delete removes.
```

### Prompt P‑04 — Wire DI & Controller Skeleton

```text
Register singleton TodoStore.
Create TodosController with route `api/[controller]`.
Add GET /api/todos returning TodoStore.GetAll.
Add integration test using WebApplicationFactory verifying 200 and empty array.
```

### Prompt P‑05 — Full CRUD Endpoints

```text
Implement POST, GET by id, PUT, DELETE.
Ensure proper ActionResult return codes (201, 204, 404).
Update integration tests to cover all cases.
```

### Prompt P‑06 — Quality Gates

```text
Add StyleCop.Analyzers package.
Add .editorconfig with strict Microsoft rules.
Enable TreatWarningsAsErrors in csproj.
Run `dotnet format` and fix offences.
Add pre-commit PowerShell script executing format/build/test.
```

### Prompt P‑07 — Swagger & READ‑ME

```text
Add Swashbuckle.AspNetCore.
Enable SwaggerGen & UI in Development environment only.
Update README with run instructions and screenshot placeholder.
```

### Prompt P‑08 — Windows Service Toggle

```text
Modify Program.cs:
```

(… continues for all phases to P‑12.)

---

## 4. Review & Right‑Sizing

Each atomic step obeys: **< 15 LOC change**, green tests, single concern. This permits TDD & incremental commits. If anything balloons, split further (guideline: if diff > 20 LOC or touches > 2 files, split).

---

## 5. Next Actions

1. Copy atomic steps into `todo.md` with markdown checkboxes.
2. Start with P‑00 prompt.
3. After each step, mark checkbox and commit.
4. Periodically push to remote.

---

