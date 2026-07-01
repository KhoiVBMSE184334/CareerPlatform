import api from "../api/api";

export type PortfolioProject = {
  projectId: string;
  repositoryName: string;
  description?: string | null;
  techStack?: string | null;
  githubUrl: string;
  importedAt: string;
};

export type AdminPortfolioProject = {
  projectId: string;
  userId: string;
  studentName: string;
  repositoryName: string;
  description?: string | null;
  techStack?: string | null;
  githubUrl: string;
  importedAt: string;
};

export async function getPortfolioProjects() {
  const { data } = await api.get<PortfolioProject[]>("/api/portfolio");
  return data;
}

export async function getAdminPortfolioProjects() {
  const { data } = await api.get<AdminPortfolioProject[]>("/api/portfolio/admin");
  return data;
}

export async function getAdminPortfolioProject(projectId: string) {
  const { data } = await api.get<AdminPortfolioProject>(
    `/api/portfolio/admin/${projectId}`,
  );
  return data;
}

export async function importGitHubPortfolio(githubUrl: string) {
  const { data } = await api.post<PortfolioProject[]>(
    "/api/portfolio/import-github",
    { githubUrl },
  );
  return data;
}

export async function deletePortfolioProject(projectId: string) {
  await api.delete(`/api/portfolio/${projectId}`);
}
