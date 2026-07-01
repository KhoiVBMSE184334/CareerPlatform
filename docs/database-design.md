# Database Design

## Database Name

CareerPlatformDB

## Database Management System

* SQL Server
* Entity Framework Core 8 (Code First)

---

# Overview

The database supports:

* User Authentication
* Student Profiles
* Career Path Management
* Dynamic Learning Roadmaps
* Skill Gap Analysis
* AI Mentor Chat History
* Portfolio Management

---

# Tables

## 1. Users

Purpose:
Store system user accounts.

| Column       | Type             | Constraints |
| ------------ | ---------------- | ----------- |
| UserId       | uniqueidentifier | PK          |
| FullName     | nvarchar(100)    | NOT NULL    |
| Email        | nvarchar(255)    | UNIQUE      |
| PasswordHash | nvarchar(max)    | NOT NULL    |
| Role         | nvarchar(20)     | NOT NULL    |
| CreatedAt    | datetime         | NOT NULL    |

Relationship:

* One User has one StudentProfile
* One User has many UserSkills
* One User has many ChatSessions
* One User has many PortfolioProjects
* One User has many RoadmapProgresses

---

## 2. StudentProfiles

Purpose:
Store student academic information.

| Column     | Type             | Constraints |
| ---------- | ---------------- | ----------- |
| ProfileId  | uniqueidentifier | PK          |
| UserId     | uniqueidentifier | FK          |
| University | nvarchar(150)    | NULL        |
| Major      | nvarchar(100)    | NULL        |
| GPA        | decimal(3,2)     | NULL        |
| GithubUrl  | nvarchar(255)    | NULL        |

Relationship:

* One StudentProfile belongs to one User

---

## 3. CareerPaths

Purpose:
Store predefined career paths.

Examples:

* Backend Developer
* Frontend Developer
* Full Stack Developer
* Mobile Developer
* DevOps Engineer
* Data Engineer
* AI Engineer

| Column       | Type          | Constraints |
| ------------ | ------------- | ----------- |
| CareerPathId | int           | PK          |
| Name         | nvarchar(100) | NOT NULL    |
| Description  | nvarchar(500) | NULL        |

Relationship:

* One CareerPath has many SkillNodes

---

## 4. SkillNodes

Purpose:
Store roadmap skills for a career path.

| Column         | Type          | Constraints |
| -------------- | ------------- | ----------- |
| SkillNodeId    | int           | PK          |
| CareerPathId   | int           | FK          |
| Name           | nvarchar(100) | NOT NULL    |
| Description    | nvarchar(500) | NULL        |
| Difficulty     | int           | NOT NULL    |
| DisplayOrder   | int           | NOT NULL    |
| EstimatedHours | int           | NULL        |

Relationship:

* One SkillNode belongs to one CareerPath
* One SkillNode has many LearningResources

---

## 5. LearningResources

Purpose:
Store learning materials for skills.

| Column      | Type          | Constraints |
| ----------- | ------------- | ----------- |
| ResourceId  | int           | PK          |
| SkillNodeId | int           | FK          |
| Title       | nvarchar(200) | NOT NULL    |
| Url         | nvarchar(500) | NOT NULL    |

Examples:

* Microsoft Learn
* Official Documentation
* FreeCodeCamp
* YouTube Tutorials

---

## 6. UserSkills

Purpose:
Store skills entered by students.

| Column      | Type             | Constraints |
| ----------- | ---------------- | ----------- |
| UserSkillId | int              | PK          |
| UserId      | uniqueidentifier | FK          |
| SkillName   | nvarchar(100)    | NOT NULL    |

Examples:

* C#
* SQL
* HTML
* CSS
* JavaScript

---

## 7. UserCareerPaths

Purpose:
Store selected target career path.

| Column       | Type             | Constraints |
| ------------ | ---------------- | ----------- |
| Id           | int              | PK          |
| UserId       | uniqueidentifier | FK          |
| CareerPathId | int              | FK          |

Relationship:

* One User selects one Career Path

---

## 8. RoadmapProgresses

Purpose:
Track roadmap completion.

| Column      | Type             | Constraints |
| ----------- | ---------------- | ----------- |
| ProgressId  | int              | PK          |
| UserId      | uniqueidentifier | FK          |
| SkillNodeId | int              | FK          |
| IsCompleted | bit              | NOT NULL    |
| CompletedAt | datetime         | NULL        |

Relationship:

* One User has many RoadmapProgress records

---

## 9. ChatSessions

Purpose:
Store AI mentor conversations.

| Column    | Type             | Constraints |
| --------- | ---------------- | ----------- |
| SessionId | uniqueidentifier | PK          |
| UserId    | uniqueidentifier | FK          |
| CreatedAt | datetime         | NOT NULL    |

Relationship:

* One ChatSession has many ChatMessages

---

## 10. ChatMessages

Purpose:
Store chat messages.

| Column    | Type             | Constraints |
| --------- | ---------------- | ----------- |
| MessageId | uniqueidentifier | PK          |
| SessionId | uniqueidentifier | FK          |
| Role      | nvarchar(20)     | NOT NULL    |
| Content   | nvarchar(max)    | NOT NULL    |
| CreatedAt | datetime         | NOT NULL    |

Role Values:

* User
* Assistant

---

## 11. PortfolioProjects

Purpose:
Store imported GitHub repositories.

| Column         | Type             | Constraints |
| -------------- | ---------------- | ----------- |
| ProjectId      | uniqueidentifier | PK          |
| UserId         | uniqueidentifier | FK          |
| RepositoryName | nvarchar(200)    | NOT NULL    |
| Description    | nvarchar(max)    | NULL        |
| TechStack      | nvarchar(max)    | NULL        |
| GithubUrl      | nvarchar(500)    | NOT NULL    |
| ImportedAt     | datetime         | NOT NULL    |

Relationship:

* One User has many PortfolioProjects

---

# Enum Definitions

## UserRole

* Student
* Admin

## Difficulty

* Beginner = 1
* Intermediate = 2
* Advanced = 3

---

# Seed Data

## Career Paths

1. Backend Developer
2. Frontend Developer
3. Full Stack Developer
4. Mobile Developer
5. DevOps Engineer
6. Data Engineer
7. AI Engineer

---

## Demo Accounts

Admin:

* Email: [admin@career.com](mailto:admin@career.com)

Student:

* Email: [student@career.com](mailto:student@career.com)

Passwords must be hashed using ASP.NET Identity PasswordHasher.

---

# Future Extensions

Possible future tables:

* Mentor
* MentorSession
* Certificates
* LearningPlans
* Notifications
* SkillAssessmentResults

These tables are not included in version 1.0.
