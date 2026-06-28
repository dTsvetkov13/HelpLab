# HelpLab

A Q&A web platform for school material, where students can post questions and provide answers. Built with a microservices architecture, event-driven messaging, and a modern Angular frontend.

---

## Overview

HelpLab lets users register, create question posts, write answers (with nested replies), and view community activity. Each user has a profile showing their total posts and answers. The system tracks counts asynchronously via an event bus rather than with direct service-to-service calls.

---

## Architecture

The application follows a **microservices** pattern with an API Gateway as the single entry point for clients.

```
Angular SPA
    │
    ▼
API Gateway  ──── JWT validation (IdentityServer4)
    │
    ├── HTTP ──► Microservices.Users
    ├── HTTP ──► Microservices.Posts
    └── HTTP ──► Microservices.Answers
                        │
             RabbitMQ (EasyNetQ)
             ┌──────────┴──────────┐
        PostsCreated         AnswersCreated
        PostsDeleted         AnswersDeleted
             └── Updates PostsCount / AnswersCount asynchronously
```

**Services:**

| Service | Responsibility | Port |
|---|---|---|
| API Gateway | Routes requests, enforces auth | 44330 |
| Microservices.Users | Auth, user management, IdentityServer4 | 44337 |
| Microservices.Posts | Post CRUD | 44367 |
| Microservices.Answers | Answer/reply CRUD | 44301 |
| WebClient | Hosts the Angular SPA | 44341 |

Each microservice owns its own SQL Server database (Database-per-Service pattern). User data is intentionally denormalized — each service holds a minimal copy of the user record to avoid inter-service queries at read time.

Cross-service state changes (e.g. incrementing a user's `PostsCount` when they publish a post) are handled by publishing domain events to RabbitMQ. Background services in each microservice subscribe to the relevant topics and apply updates asynchronously.

---

## Tech Stack

**Backend**
- ASP.NET Core 5.0 (C#)
- Entity Framework Core 5.0 — code-first migrations, SQL Server
- IdentityServer4 — OAuth2 / OpenID Connect, JWT issuance
- EasyNetQ over RabbitMQ — publish/subscribe event bus
- Swashbuckle — Swagger API documentation

**Frontend**
- Angular 12 (TypeScript 4.1)
- Angular Material + Bootstrap 4.6 — UI components and layout
- CKEditor 5 — rich text editing for posts and answers
- OIDC Client — token-based authentication in the browser
- RxJS — reactive data flows
- SweetAlert2 — user-facing notifications

**Infrastructure**
- Docker — multi-stage Dockerfiles for all services
- RabbitMQ — message broker (can be run via Docker)

---

## Key Design Decisions

**API Gateway as the single entry point.** Clients never talk to microservices directly. The gateway validates the JWT, then proxies the request over HTTP to the appropriate internal service. This keeps auth logic centralized and internal services simple.

**ResourceOwnerPassword flow with IdentityServer4.** Users authenticate by posting credentials to the gateway, which exchanges them for a JWT via the Users service acting as an OAuth2 token endpoint. The token is then used as a Bearer header on subsequent requests.

**Event bus for counter aggregation.** Rather than calling the Users service synchronously every time a post or answer is created, the service that owns the event publishes a message (e.g. `AnswersCreatedRoute`) and subscribes handle counter updates independently. This keeps services decoupled and prevents cascading failures.

**Shared models library.** `Microservices.Models` is a shared class library referenced by the gateway and all microservices. It holds the DTOs and response envelopes used across service boundaries, avoiding duplication.

**`IEventBus` abstraction.** The RabbitMQ implementation sits behind an interface, making the broker swappable without touching business logic.

---

## Data Models

**Post**
- `Id` (Guid), `Title`, `Description`, `PublishedAt`, `LastEditedAt`
- `AuthorId` → User, `AnswersCount`

**Answer**
- `Id` (Guid), `Text`, `PublishedAt`, `LastEditedAt`
- `AuthorId` → User, `PostId` → Post
- `AnswerId` → parent Answer (for nested replies), `AnswersCount`

**User**
- `Id`, `Name`, `Surname`, `Email`
- `PostsCount`, `AnswersCount` (maintained via event bus)

---

## API Endpoints (Gateway)

| Method | Route | Auth | Description |
|---|---|---|---|
| `GET` | `/api/posts/{id}` | — | Get single post |
| `GET` | `/api/posts?UserId=` | — | Get posts by user |
| `POST` | `/api/posts` | JWT | Create post |
| `PUT` | `/api/posts` | JWT | Update post |
| `DELETE` | `/api/posts?id=&userId=` | JWT | Delete post |
| `GET` | `/api/answers/postId/{id}` | — | Get answers for a post |
| `POST` | `/api/answers` | JWT | Create answer |
| `PUT` | `/api/answers` | JWT | Update answer |
| `DELETE` | `/api/answers?id=&userId=` | JWT | Delete answer |
| `GET` | `/api/users` | JWT | Get current user |
| `GET` | `/api/users/{id}` | — | Get user by ID |
| `POST` | `/api/users` | — | Register |
| `POST` | `/api/users/login` | — | Login (returns JWT) |

---

## Running Locally

**Prerequisites:** Visual Studio 2019+, .NET 5 SDK, SQL Server, Docker

1. **Start RabbitMQ**
   ```shell
   docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
   ```

2. **Configure database connections**  
   Set your SQL Server instance name in `appsettings.json` inside `Microservices.Users`, `Microservices.Posts`, and `Microservices.Answers`.

3. **Apply migrations**  
   Run from the solution root for each data project:
   ```shell
   dotnet ef database update --project Microservices.Users
   dotnet ef database update --project Microservices.Posts
   dotnet ef database update --project Microservices.Answers
   ```

4. **Start all services**  
   In Visual Studio: right-click the solution → Properties → Startup Projects → Multiple startup projects.  
   Select: `WebClient`, `APIGateway`, `Microservices.Users`, `Microservices.Posts`, `Microservices.Answers`.

5. Open `https://localhost:44341` in your browser.

---

## Project Structure

```
HelpLab/
├── APIGateway/              # Gateway: routing, JWT validation, Swagger
├── Microservices.Users/     # Auth, user profiles, IdentityServer4
├── Microservices.Posts/     # Post management
├── Microservices.Answers/   # Answer and reply management
├── Microservices.EventBus/  # IEventBus abstraction + RabbitMQ implementation
├── Microservices.Models/    # Shared DTOs, response envelopes, event messages
└── WebClient/               # Angular 12 SPA
    └── ClientApp/
        └── src/app/
            ├── pages/       # Route components: Home, Post, LatestPosts, Login, SignUp, Users, CreatePost
            ├── services/    # WebRequestsService, ErrorHandler
            ├── models/      # Post, Answer, User, Search
            └── routing/     # Angular routing module
```
