# C# ASP.NET Core 8 Todo API Template

A minimal yet production‑hardened starter kit for building C# / ASP.NET Core 8 REST APIs. It mirrors the classic FastAPI “Todo” example while showcasing modern C# best practices: strict static analysis, Swagger documentation, NUnit testing with Moq, StyleCop‑enforced conventions, and seamless Windows‑Service hosting.

---

## ✨ Key Features

* **Controller‑based CRUD** example (`/api/todos`) with full OpenAPI v3 docs (Swagger UI).
* **Single‑project layout** for simplicity; thread‑safe in‑memory store keeps the template lightweight.
* **Warnings‑as‑errors** + **nullable reference types** = null‑safe, clean builds.
* **StyleCop.Analyzers** and `.editorconfig` lock down consistent code style.
* **Pre‑commit PowerShell hook** runs formatter, build, tests, and coverage before every commit.
* Auto‑detects Windows‑Service mode—same binary runs as either console or service.

---

## 🚀 Quick Start

```bash
# 1 Clone & restore
 git clone <your‑repo‑url>
 cd CsharpBackendService
 dotnet restore

# 2 Run with hot‑reload
 dotnet watch --project src/CsharpBackendService run
```

Swagger UI → [http://localhost:5064/swagger](http://localhost:5064/swagger)
API root → [http://localhost:5064/api/todos](http://localhost:5064/api/todos)

### Run Tests & Coverage

```bash
# All tests + LCOV coverage
 dotnet test --collect:"XPlat Code Coverage"
```

### Install the Pre‑commit Hook

```bash
pwsh .githooks/install.ps1   # sets core.hooksPath=.githooks
```

---

## 📂 Folder Layout

```text
/
├── src/
│   └── CsharpBackendService/              # Web API project
│       ├── Controllers/                   # [ApiController] endpoints
│       ├── Models/                        # Immutable record types
│       ├── Services/                      # In‑memory store
│       └── Program.cs                     # Host + DI wiring
└── tests/
    └── CsharpBackendService.Tests/        # NUnit + Moq + WebAppFactory
```

---

## ⚙️ Quality Gates

| Concern            | Toolchain / Command                                        |
| ------------------ | ---------------------------------------------------------- |
| Static analysis    | Roslyn + StyleCop (`dotnet build`)                         |
| Formatting         | `dotnet format --verify-no-changes`                        |
| Unit / integration | NUnit, Moq, Microsoft.AspNetCore.Mvc.Testing               |
| Coverage           | Coverlet (`--collect:"XPlat Code Coverage"`)               |
| Git hook           | `.githooks/pre-commit.ps1` runs all the above sequentially |

> **All compiler warnings are treated as errors.** Keep VS / SDK updated to avoid false positives.

---

## 🖥️ Windows Service Support

The app automatically switches to Windows‑Service mode when `Environment.UserInteractive == false`.

* **Service name:** `CsharpBackendService`
* Installer / registration script lives in a separate deployment project (not included here).

Running as a console application remains the default during development.

---
