# 🚀 TaskFlow

> A clean and testable Task Management REST API built with C# and .NET 8.

## 🛠️ Technologies

**C# · .NET 8 · ASP.NET Core · Entity Framework Core · SQL Server · xUnit · Moq · GitHub Actions**

---

## 🏗️ Architecture

```text
        🌐 Client
           │
           ▼
     ┌─────────────┐
     │     API     │
     │ Controllers │
     └──────┬──────┘
            │
            ▼
     ┌─────────────┐
     │ Application │
     │  Services   │
     └──────┬──────┘
            │
            ▼
     ┌─────────────┐
     │   Domain    │
     │  Entities   │
     └─────────────┘
            ▲
            │
     ┌──────┴──────┐
     │Infrastructure│
     │ EF Core / DB │
     └─────────────┘

Clean Architecture · SOLID · Repository Pattern · Dependency Injection

✨ Features
✅ Create / Read / Update / Delete Tasks
✅ Complete / Reopen Tasks
✅ Request Validation
✅ Global Exception Handling
✅ Swagger / OpenAPI
✅ SQL Server LocalDB
✅ EF Core Migrations
✅ Unit & Integration Tests
✅ GitHub Actions CI
🧪 Testing
22 Tests
────────────
13 Unit Tests
 9 Integration Tests

✅ 22 / 22 Passed
📡 API
Method	Endpoint	Description
POST	/api/tasks	Create task
GET	/api/tasks	Get all tasks
GET	/api/tasks/{id}	Get task
PUT	/api/tasks/{id}	Update task
PUT	/api/tasks/{id}/complete	Complete
PUT	/api/tasks/{id}/reopen	Reopen
DELETE	/api/tasks/{id}	Delete
🔐 Security

Development connection strings are managed with ASP.NET Core User Secrets and are not stored in the repository.

👨‍💻 Author

Göktuğ Göçer
