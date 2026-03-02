<p align="center">
  <img src="https://raw.githubusercontent.com/feiyit/fytsoa-admin/main/doc/fytsoa-cover.jpg" alt="FytSoa Cover" width="100%" />
</p>

<p align="center">
  <a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet&logoColor=white" alt=".NET 8"></a>
  <a href="https://vuejs.org/"><img src="https://img.shields.io/badge/Vue-3-42B883?logo=vue.js&logoColor=white" alt="Vue 3"></a>
  <a href="https://www.typescriptlang.org/"><img src="https://img.shields.io/badge/TypeScript-5-3178C6?logo=typescript&logoColor=white" alt="TypeScript"></a>
  <a href="https://vitejs.dev/"><img src="https://img.shields.io/badge/Vite-7-646CFF?logo=vite&logoColor=white" alt="Vite 7"></a>
  <a href="https://www.mysql.com/"><img src="https://img.shields.io/badge/MySQL-8-4479A1?logo=mysql&logoColor=white" alt="MySQL 8"></a>
  <img src="https://img.shields.io/badge/Architecture-Layered-0EA5E9" alt="Layered Architecture">
</p>

<p align="center">
  <a href="README.md"><strong>English</strong></a> |
  <a href="README.zh-CN.md"><strong>简体中文</strong></a>
</p>

# FytSoa Enterprise Framework

FytSoa is an open-source full-stack framework for enterprise back-office systems, built with **.NET 8 + Vue 3**. It includes multi-tenant support, RBAC authorization, organization & employment management, CMS, workflow engine, scheduler, and audit logging.

## Production Showcase

The official website **Feiyit** is built on this framework: https://www.feiyit.com  
Its website source implementation is available in `src/FytSoa.Web`, and is kept open for learning and secondary development reference.

## Highlights

- Fast development via dynamic WebAPI + dynamic frontend routing
- Clear layered architecture for long-term maintainability
- Rich enterprise modules (auth, tenant, workflow, CMS, scheduler, logs)
- Security-focused design (JWT, token blacklist, slider verification, audit)
- DevOps-friendly (Swagger/FytApiUI, Quartz job management, structured logging)

## Backend Architecture (`src/`)

### Tech Stack

- Runtime: .NET 8 / ASP.NET Core Web API
- ORM: SqlSugar
- Mapping: Mapster
- Auth: JWT Bearer (`accessToken` header)
- API Docs: Swashbuckle + FytApiUI
- Scheduler: Quartz
- Event Bus: CAP (in-memory setup by default)
- Cache: FreeRedis + IMemoryCache
- Real-time: SignalR

### Layering

- `FytSoa.Domain`: domain entities (`Sys` / `Cms` / `User` / `Wf`)
- `FytSoa.Application`: application services and orchestration
- `FytSoa.Sugar`: SqlSugar repository and unit-of-work infrastructure
- `FytSoa.Common`: common utilities (JWT/cache/helpers)
- `FytSoa.CrossCutting`: DI/auth/scheduling/dynamic API registration
- `FytSoa.ApiService`: API host (middleware/filters/swagger/hubs)
- `FytSoa.Generator`: code generator
- `FytSoa.Web`: Razor Pages site host

## Frontend Architecture (`admin/`)

### Tech Stack

- Vue 3 + TypeScript
- Vite 7
- Pinia
- Vue Router (dynamic route mounting)
- Axios (unified interceptors and token refresh handling)
- Element Plus + TailwindCSS
- ECharts, TinyMCE, reusable low-code-like components

### Frontend Advantages

- Menu-driven dynamic routes after login
- Unified API response/error handling
- Button-level permission control via `v-auth`
- Runtime API config with `public/runtime-config.js`
- Reusable business components (table/form/upload/workflow)

## Functional Modules

- **System & Permission**: admin, role, menu, permission, code dictionary
- **Multi-Tenant & Organization**: tenant, org tree/closure, position, employment, reporting
- **CMS**: site, column, article, template, ad, file, message
- **Workflow**: definition/model/form/instance/task/history/business data
- **Operations**: notice, logs, scheduler
- **Membership**: member and member groups
- **Security extension models**: login audit, password reset token, account/session models

## Security

- JWT-based authentication and authorization (controllers are globally protected by default)
- Token blacklist mechanism for logout invalidation
- Slider verification before login
- Operation and exception audit logs
- Role conflict constraints and permission isolation
- Request-signing extension support (`appkey/timestamp/signature`)

### Frontend-to-Backend API Security

- Frontend injects `accessToken` centrally through Axios interceptors; backend validates JWT signature, issuer, audience, and expiration
- Backend can return `X-Refresh-Token`; frontend rotates token automatically to reduce session interruption and replay window
- Signature headers (`appkey`, `timestamp`, `signature`) are supported to protect API calls against tampering/replay when enabled in environment config
- Login flow combines slider verification token + credential verification, reducing automated abuse risk
- Unified response and route guards on frontend handle `401` consistently and force re-authentication when needed
- Recommended for production: HTTPS-only access, strict CORS allowlist, and secure management of secret keys in deployment environments

## Project Structure

```text
.
├── src/                         # .NET backend solution
│   ├── FytSoa.ApiService
│   ├── FytSoa.Application
│   ├── FytSoa.Domain
│   ├── FytSoa.Sugar
│   ├── FytSoa.Common
│   ├── FytSoa.CrossCutting
│   ├── FytSoa.Generator
│   └── FytSoa.Web
├── admin/                       # Vue admin frontend
└── doc/fytsoa_admin.sql         # MySQL schema
```

## Quick Start

### Requirements

- .NET SDK 8+
- Node.js 20+
- pnpm 10+
- MySQL 8+
- Redis (optional)

### 1) Database

Import:

- `doc/fytsoa_admin.sql`

### 2) Run backend

```bash
cd src
dotnet restore FytSoa.sln
dotnet build FytSoa.sln
dotnet run --project FytSoa.ApiService/FytSoa.ApiService.csproj
```

Configure in `src/FytSoa.ApiService/appsettings.Development.json`:

- `SqlConnectionString:MySql`
- `Cache:Redis`
- `JwtAuth`
- `Security`

### 3) Run frontend

```bash
cd admin
pnpm install
pnpm dev
```

Configure via:

- `admin/.env.development` (`VITE_API_BASE_URL`)
- or `admin/public/runtime-config.js` (`API_BASE_URL`)

### 4) Access

- Admin UI: `http://localhost:2320`
- Swagger: `http://localhost:5111/swagger`
- FytApiUI: `http://localhost:5111/fytapiui/index.html`


## Suitable Scenarios

- Enterprise management systems
- Multi-tenant SaaS back-office platforms
- CMS + workflow-driven operations platforms
- Framework base for rapid secondary development
