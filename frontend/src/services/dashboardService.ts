import api from "../api/api";
import type { CareerPath } from "./careerPathService";
import type { ChatSession } from "./aiService";

export type StudentDashboard = {
  selectedCareerPath?: CareerPath | null;
  roadmapCompletionPercentage: number;
  skillMatchPercentage: number;
  missingSkillsSummary: string[];
  recentAIChatSessions: ChatSession[];
  portfolioProjectCount: number;
};

export type AdminDashboard = {
  totalUsers: number;
  totalStudents: number;
  totalAdmins: number;
  totalCareerPaths: number;
  totalSkillNodes: number;
  totalPortfolioProjects: number;
};

export async function getStudentDashboard() {
  const { data } = await api.get<StudentDashboard>("/api/dashboard/student");
  return data;
}

export async function getAdminDashboard() {
  const { data } = await api.get<AdminDashboard>("/api/dashboard/admin");
  return data;
}
