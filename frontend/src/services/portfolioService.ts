import api from "./api";

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
  const { data } = await api.get<PortfolioProject[]>("/portfolio");
  return data;
}

export async function getAdminPortfolioProjects() {
  const { data } = await api.get<AdminPortfolioProject[]>("/portfolio/admin");
  return data;
}

export async function getAdminPortfolioProject(projectId: string) {
  const { data } = await api.get<AdminPortfolioProject>(
    `/portfolio/admin/${projectId}`,
  );
  return data;
}

export async function importGitHubPortfolio(githubUrl: string) {
  const { data } = await api.post<PortfolioProject[]>(
    "/portfolio/import-github",
    { githubUrl },
  );
  return data;
}

export async function deletePortfolioProject(projectId: string) {
  await api.delete(`/portfolio/${projectId}`);
}
