<p align="center">
  <img src="https://raw.githubusercontent.com/feiyit/fytsoa-admin/main/doc/fytsoa-cover.jpg" alt="FytSoa Cover" width="100%" />
</p>

<p align="center">
  <a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet&logoColor=white" alt=".NET 8"></a>
  <a href="https://vuejs.org/"><img src="https://img.shields.io/badge/Vue-3-42B883?logo=vue.js&logoColor=white" alt="Vue 3"></a>
  <a href="https://www.typescriptlang.org/"><img src="https://img.shields.io/badge/TypeScript-5-3178C6?logo=typescript&logoColor=white" alt="TypeScript"></a>
  <a href="https://vitejs.dev/"><img src="https://img.shields.io/badge/Vite-7-646CFF?logo=vite&logoColor=white" alt="Vite 7"></a>
  <a href="https://www.mysql.com/"><img src="https://img.shields.io/badge/MySQL-8-4479A1?logo=mysql&logoColor=white" alt="MySQL 8"></a>
  <img src="https://img.shields.io/badge/架构-分层设计-0EA5E9" alt="Layered Architecture">
</p>

<p align="center">
  <a href="README.md"><strong>English</strong></a> |
  <a href="README.zh-CN.md"><strong>简体中文</strong></a>
</p>

# FytSoa 企业管理框架

FytSoa 是一个面向中后台业务场景的开源全栈框架，采用 **.NET 8 + Vue 3** 技术体系，内置多租户、RBAC 权限、组织与任用、内容管理、工作流、任务调度、审计日志等企业级能力。

## 线上实践（官网案例）

飞易腾官网已基于本框架构建并稳定运行：**https://www.feiyit.com**。  
官网相关源码位于 `src/FytSoa.Web` 工程，并随项目一并开源，便于学习参考与二次开发落地。

## 框架亮点

- 动态 WebAPI + 动态路由，业务开发效率高
- 领域分层清晰，便于长期维护和扩展
- 覆盖企业核心模块（权限、租户、流程、内容、日志）
- 安全机制完善（JWT、黑名单、滑块验证、审计）
- 运维友好（Swagger/FytApiUI、Quartz、结构化日志）

## 后端架构（`src/`）

### 技术栈

- 运行时：.NET 8 / ASP.NET Core Web API
- ORM：SqlSugar
- 对象映射：Mapster
- 鉴权：JWT Bearer（`accessToken` 请求头）
- 文档：Swashbuckle + FytApiUI
- 调度：Quartz
- 事件总线：CAP（默认内存模式）
- 缓存：FreeRedis + IMemoryCache
- 实时通信：SignalR

### 分层结构

- `FytSoa.Domain`：领域实体（`Sys` / `Cms` / `User` / `Wf`）
- `FytSoa.Application`：应用服务与业务编排
- `FytSoa.Sugar`：SqlSugar 仓储与工作单元
- `FytSoa.Common`：公共能力（JWT/缓存/工具）
- `FytSoa.CrossCutting`：横切关注点（DI/鉴权/调度/动态API）
- `FytSoa.ApiService`：API 宿主（中间件/过滤器/Swagger/Hub）
- `FytSoa.Generator`：代码生成
- `FytSoa.Web`：Razor Pages 前台站点

## 前端架构（`admin/`）

### 技术栈

- Vue 3 + TypeScript
- Vite 7
- Pinia
- Vue Router（动态路由挂载）
- Axios（统一拦截、错误处理、Token 续期）
- Element Plus + TailwindCSS
- ECharts、TinyMCE、自定义业务组件

### 前端优势

- 登录后按菜单动态装载路由，天然契合 RBAC
- 统一处理 `code/data/message` 与 HTTP 异常
- `v-auth` 指令实现按钮级权限控制
- `runtime-config.js` 支持运行时切换 API 地址
- 表单/表格/上传/流程等组件复用度高

## 功能模块（基于 `doc/fytsoa_admin.sql`）

- **系统与权限**：管理员、角色、菜单、授权、字典
- **多租户与组织人事**：租户、组织树/闭包、岗位、任用、汇报关系
- **内容管理 CMS**：站点、栏目、文章、模板、广告、资源、留言
- **工作流引擎**：流程定义、模型、表单、实例、任务、历史
- **运维能力**：通知、日志、任务调度
- **会员中心**：会员、会员组
- **安全扩展模型**：登录审计、重置令牌、会话账户模型

## 安全性

- JWT 统一鉴权（接口默认受保护）
- Token 黑名单机制（退出后失效）
- 登录滑块验证
- 操作/异常审计日志
- 角色互斥与权限隔离
- 支持请求签名扩展（`appkey/timestamp/signature`）

### 前端访问后端接口安全（重点）

- 前端通过 Axios 拦截器统一注入 `accessToken`，后端统一校验 JWT 的签名、签发方、受众与过期时间
- 后端可通过 `X-Refresh-Token` 返回新令牌，前端自动轮换，降低会话中断与令牌重放风险
- 支持 `appkey + timestamp + signature` 签名头机制（按环境配置启用），用于增强防篡改与防重放能力
- 登录链路采用“滑块验证 token + 账号密码校验”双步骤，降低脚本化撞库/爆破风险
- 前端统一处理 `401` 并强制重新登录，避免失效凭据继续访问受保护资源
- 生产环境建议：全站 HTTPS、严格 CORS 白名单、密钥与签名参数通过安全配置托管

## 目录结构

```text
.
├── src/                         # .NET 后端解决方案
│   ├── FytSoa.ApiService
│   ├── FytSoa.Application
│   ├── FytSoa.Domain
│   ├── FytSoa.Sugar
│   ├── FytSoa.Common
│   ├── FytSoa.CrossCutting
│   ├── FytSoa.Generator
│   └── FytSoa.Web
├── admin/                       # Vue 管理端
└── doc/fytsoa_admin.sql         # MySQL 脚本
```

## 快速开始

### 环境要求

- .NET SDK 8+
- Node.js 20+
- pnpm 10+
- MySQL 8+
- Redis（可选）

### 1）初始化数据库

导入：

- `doc/fytsoa_admin.sql`

### 2）启动后端

```bash
cd src
dotnet restore FytSoa.sln
dotnet build FytSoa.sln
dotnet run --project FytSoa.ApiService/FytSoa.ApiService.csproj
```

按需配置 `src/FytSoa.ApiService/appsettings.Development.json`：

- `SqlConnectionString:MySql`
- `Cache:Redis`
- `JwtAuth`
- `Security`

### 3）启动前端

```bash
cd admin
pnpm install
pnpm dev
```

按需配置：

- `admin/.env.development` 的 `VITE_API_BASE_URL`
- 或 `admin/public/runtime-config.js` 的 `API_BASE_URL`

### 4）访问地址

- 管理端：`http://localhost:2320`
- Swagger：`http://localhost:5111/swagger`
- FytApiUI：`http://localhost:5111/fytapiui/index.html`


## 适用场景

- 企业管理中后台
- SaaS 多租户后台平台
- CMS + 工作流运营平台
- 二次开发框架基座
