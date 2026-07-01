# API Contract

Base URL:

https://localhost:7001/api

---

# Authentication

## POST /auth/register

Request

{
  "fullName": "John Doe",
  "email": "john@example.com",
  "password": "Password123!"
}

Response

{
  "token": "...",
  "email": "john@example.com"
}

---

## POST /auth/login

Request

{
  "email": "john@example.com",
  "password": "Password123!"
}

Response

{
  "token": "...",
  "email": "john@example.com"
}

---

# Career Paths

## GET /careerpaths

Response

[
  {
    "careerPathId": 1,
    "name": "Backend Developer"
  }
]

---

## POST /careerpaths/select

Request

{
  "careerPathId": 1
}

---

# Roadmap

## GET /roadmap

Response

[
  {
    "skillNodeId": 1,
    "name": "C#",
    "completed": false
  }
]

---

## PUT /roadmap/progress

Request

{
  "skillNodeId": 1,
  "isCompleted": true
}

---

# Skill Gap

## POST /skillgap/analyze

Request

{
  "careerPathId": 1,
  "skills": [
    "C#",
    "SQL"
  ]
}

Response

{
  "matchPercentage": 65,
  "missingSkills": [
    "ASP.NET Core",
    "JWT"
  ]
}

---

# AI Mentor

## POST /chat

Request

{
  "message": "How can I become a Backend Developer?"
}

Response

{
  "response": "..."
}

---

# Portfolio

## POST /portfolio/import-github

Request

{
  "githubUrl": "https://github.com/username"
}

Response

[
  {
    "repositoryName": "CareerPlatform",
    "description": "...",
    "techStack": "ASP.NET Core, React"
  }
]