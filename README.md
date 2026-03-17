# DPM.Backend

> **Distributed Project Management Backend** — Hệ thống backend được xây dựng theo kiến trúc **Modular Monolith** trên nền tảng ASP.NET Core, hỗ trợ mở rộng linh hoạt theo từng module nghiệp vụ độc lập.

---

## 📐 Kiến trúc tổng quan

Dự án áp dụng **Modular Monolith Architecture** — một mô hình kiến trúc trung gian giữa Monolith truyền thống và Microservices. Mỗi module được đóng gói hoàn toàn độc lập (bounded context), nhưng vẫn chạy trong một process duy nhất, giúp giảm overhead giao tiếp mạng và đơn giản hóa quá trình triển khai.

```
DPM.Backend/
├── .github/
│   └── workflows/              # CI/CD pipelines (GitHub Actions)
├── DPM.Backend.Host/           # Entry point — ASP.NET Core Web Host
├── Modules/
│   └── Identity/               # Module quản lý xác thực & phân quyền
├── Shared/                     # Shared Kernel — abstractions & common utilities
├── docker-compose.yml          # Orchestration cho local development
├── .dockerignore
└── README.md
```

---

## 🧩 Phân tích từng thành phần

### `DPM.Backend.Host` — Application Host
Entry point của toàn bộ ứng dụng. Chịu trách nhiệm:
- Khởi tạo `WebApplication` / `IHost` từ ASP.NET Core
- Đăng ký (register) tất cả module vào DI Container
- Cấu hình middleware pipeline (authentication, authorization, exception handling, logging...)
- Load cấu hình từ `appsettings.json`, environment variables và secrets
- Expose HTTP endpoints tổng hợp từ tất cả module

> **Nguyên tắc thiết kế:** Host không chứa business logic. Nó chỉ là "dây kết nối" giữa infrastructure và các module nghiệp vụ.

---

### `Modules/Identity` — Identity Module
Module xử lý toàn bộ vòng đời xác thực và phân quyền người dùng. Theo chuẩn **Bounded Context** của DDD (Domain-Driven Design), module này tự quản lý:

- **Domain Layer**: Entities (`User`, `Role`, `Permission`...), Value Objects, Domain Events
- **Application Layer**: Use Cases (CQRS pattern với MediatR — Command/Query Handlers)
- **Infrastructure Layer**: EF Core DbContext riêng, Repository implementations, JWT token generation
- **API Layer**: Controllers/Endpoints đăng ký vào Host

Các tính năng dự kiến bao gồm:
- Đăng ký / Đăng nhập (local credentials)
- Phát hành và xác thực JWT Access Token + Refresh Token
- Quản lý Role-Based Access Control (RBAC)
- Tích hợp OAuth2 / External Providers (mở rộng)

---

### `Shared/` — Shared Kernel
Chứa các thành phần dùng chung giữa các module, **không phụ thuộc vào business logic** của bất kỳ module nào:

| Thành phần | Mô tả |
|---|---|
| `BaseEntity` / `AggregateRoot` | Base classes cho Domain Entities |
| `IRepository<T>` | Generic Repository abstraction |
| `IUnitOfWork` | Unit of Work interface |
| `IDomainEvent` | Marker interface cho Domain Events |
| `ICommand` / `IQuery<T>` | CQRS marker interfaces (MediatR) |
| `Result<T>` / `Error` | Functional error handling pattern |
| `PaginatedList<T>` | Pagination response wrapper |
| Common Extensions | String, DateTime, Enumerable helpers |

> **Quy tắc quan trọng:** Các module chỉ được phép phụ thuộc vào `Shared`. Module **không được** phụ thuộc trực tiếp vào module khác — giao tiếp liên module phải thông qua Domain Events hoặc Interface Contracts.

---

### `.github/workflows/` — CI/CD Pipeline
Cấu hình GitHub Actions tự động hóa quy trình build và deploy:

```
Push/PR → Build & Test → Docker Build → Push to Registry → Deploy
```

Workflow bao gồm:
- **Build**: Restore NuGet packages, compile toàn bộ solution
- **Test**: Chạy Unit Tests và Integration Tests
- **Docker**: Build Docker image, tag theo branch/commit SHA
- **Deploy**: Push image lên Container Registry (ghcr.io hoặc Docker Hub)

---

### `docker-compose.yml` — Local Development Stack
Định nghĩa toàn bộ môi trường phát triển cục bộ:

```yaml
services:
  api:          # DPM.Backend Host container
  db:           # PostgreSQL / SQL Server
  redis:        # Cache & Session store (nếu có)
  seq:          # Structured logging (Serilog sink)
```

---

## 🛠️ Tech Stack

| Thành phần | Công nghệ |
|---|---|
| Runtime | .NET 8 / ASP.NET Core |
| Ngôn ngữ | C# 12 |
| ORM | Entity Framework Core |
| CQRS / Mediator | MediatR |
| Authentication | ASP.NET Core Identity + JWT Bearer |
| Validation | FluentValidation |
| Logging | Serilog |
| Containerization | Docker + Docker Compose |
| CI/CD | GitHub Actions |
| Database | PostgreSQL (hoặc SQL Server) |

---

## 🚀 Hướng dẫn chạy local

### Yêu cầu
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Chạy với Docker Compose (khuyến nghị)

```bash
# Clone repository
git clone https://github.com/ngophong030700/DPM.Backend.git
cd DPM.Backend

# Khởi động toàn bộ stack (API + Database + các services phụ trợ)
docker-compose up -d

# Xem logs
docker-compose logs -f api
```

API sẽ chạy tại: `http://localhost:5000`  
Swagger UI: `http://localhost:5000/swagger`

### Chạy trực tiếp (Development mode)

```bash
# Restore dependencies
dotnet restore

# Chạy migrations (lần đầu)
dotnet ef database update --project Modules/Identity

# Chạy ứng dụng
dotnet run --project DPM.Backend.Host
```

---

## 📦 Thêm module mới

Kiến trúc Modular Monolith cho phép thêm module nghiệp vụ mà không ảnh hưởng đến các module hiện có:

```
Modules/
└── YourModule/
    ├── YourModule.Domain/          # Entities, Value Objects, Domain Events
    ├── YourModule.Application/     # Commands, Queries, Handlers, DTOs
    ├── YourModule.Infrastructure/  # DbContext, Repositories, External services
    └── YourModule.Api/             # Controllers, Endpoints, DI Registration
```

**Bước đăng ký module** trong `DPM.Backend.Host`:

```csharp
// Program.cs
builder.Services.AddYourModule(builder.Configuration);
```

Mỗi module tự implement extension method `AddYourModule()` để đăng ký tất cả dependencies nội bộ vào DI container.

---

## 🏗️ Đánh giá kiến trúc & Hướng cải thiện

### ✅ Điểm mạnh
- **Separation of Concerns** rõ ràng: Host, Module, Shared được tách biệt hoàn toàn
- **Độc lập về database**: Mỗi module có thể có DbContext riêng, dễ tách thành microservice sau này
- **CI/CD sẵn có**: GitHub Actions setup từ đầu giúp automation ngay từ bước đầu
- **Docker-first**: Môi trường nhất quán từ development đến production

### 🔧 Hướng mở rộng đề xuất
- Bổ sung `Module.Contracts` project cho mỗi module (chứa Interface và DTOs dùng để giao tiếp liên module)
- Thêm **Integration Events** (dùng MassTransit / RabbitMQ) khi cần giao tiếp async giữa các module
- Áp dụng **Outbox Pattern** để đảm bảo consistency khi publish Domain Events
- Tích hợp **Health Checks** (`/health`) cho monitoring
- Cấu hình **OpenTelemetry** cho distributed tracing

---

## 📄 License

This project is licensed under the MIT License.
