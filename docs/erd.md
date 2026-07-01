# Entity Relationship Diagram

## User

- UserId (PK)
- FullName
- Email
- PasswordHash
- Role
- CreatedAt

Relationships:
- One User -> One StudentProfile
- One User -> Many UserSkills
- One User -> Many ChatSessions
- One User -> Many PortfolioProjects
- One User -> Many RoadmapProgresses

---

## StudentProfile

- ProfileId (PK)
- UserId (FK)
- University
- Major
- GPA
- GithubUrl

---

## CareerPath

- CareerPathId (PK)
- Name
- Description

Relationships:
- One CareerPath -> Many SkillNodes

---

## SkillNode

- SkillNodeId (PK)
- CareerPathId (FK)
- Name
- Description
- Difficulty
- Order

Relationships:
- One SkillNode -> Many LearningResources

---

## LearningResource

- ResourceId (PK)
- SkillNodeId (FK)
- Title
- Url

---

## UserSkill

- UserSkillId (PK)
- UserId (FK)
- SkillName

---

## UserCareerPath

- Id (PK)
- UserId (FK)
- CareerPathId (FK)

---

## RoadmapProgress

- ProgressId (PK)
- UserId (FK)
- SkillNodeId (FK)
- IsCompleted

---

## ChatSession

- SessionId (PK)
- UserId (FK)
- CreatedAt

Relationships:
- One ChatSession -> Many ChatMessages

---

## ChatMessage

- MessageId (PK)
- SessionId (FK)
- Role
- Content
- CreatedAt

---

## PortfolioProject

- ProjectId (PK)
- UserId (FK)
- RepositoryName
- Description
- TechStack
- GithubUrl