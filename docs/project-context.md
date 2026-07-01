# Project Context

Project Name:
Personalized Career Orientation & Learning Roadmap Platform

Purpose:
Help Software Engineering students choose a suitable career path, identify missing skills, follow a structured learning roadmap, receive AI-powered guidance, and build a professional portfolio.

---

# Technology Stack

Backend:
- ASP.NET Core 8 Web API
- Entity Framework Core 8
- SQL Server
- JWT Authentication
- AutoMapper
- FluentValidation

Frontend:
- React
- TypeScript
- Vite
- Tailwind CSS
- React Router
- Axios

AI:
- OpenAI API

External Services:
- GitHub API

---

# Architecture

Clean Architecture

Projects:

CareerPlatform.API
CareerPlatform.Application
CareerPlatform.Domain
CareerPlatform.Infrastructure

Dependency Direction:

API
-> Application
-> Domain

Infrastructure
-> Application
-> Domain

---

# Coding Rules

Use:
- async/await
- Repository Pattern
- Dependency Injection
- AutoMapper
- FluentValidation

Avoid:
- CQRS
- MediatR
- Redis
- SignalR
- Docker
- Microservices

Controllers must remain thin.

Business logic belongs in Application Services.

Database access belongs in Infrastructure.

---

# Main Features

- Authentication
- Career Path Management
- Roadmap Management
- Skill Gap Analysis
- AI Mentor Chat
- GitHub Portfolio Integration
- Dashboard

---

# Career Paths

- Backend Developer
- Frontend Developer
- Full Stack Developer
- Mobile Developer
- DevOps Engineer
- Data Engineer
- AI Engineer

Roadmaps are generated from predefined SkillNode data stored in SQL Server.

AI is only used for:
- Mentor Chat
- README Summarization