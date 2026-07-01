import api from "./api";

export type AdminLearningResource = {
  resourceId: number;
  title: string;
  url: string;
};

export type AdminSkillNode = {
  skillNodeId: number;
  careerPathId: number;
  careerPathName: string;
  name: string;
  description?: string | null;
  difficulty: string;
  displayOrder: number;
  estimatedHours?: number | null;
  learningResources: AdminLearningResource[];
};

export async function getAdminSkillNodes() {
  const { data } = await api.get<AdminSkillNode[]>("/skillnodes");
  return data;
}

export async function getAdminSkillNode(skillNodeId: number) {
  const { data } = await api.get<AdminSkillNode>(`/skillnodes/${skillNodeId}`);
  return data;
}
