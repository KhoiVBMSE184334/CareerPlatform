import api from "./api";

export type LearningResource = {
  learningResourceId: number;
  title: string;
  url: string;
  type?: string | null;
};

export type RoadmapSkillNode = {
  skillNodeId: number;
  name: string;
  description?: string | null;
  difficulty: string;
  displayOrder: number;
  estimatedHours?: number | null;
  isCompleted?: boolean;
  completed?: boolean;
  completedAt?: string | null;
  learningResources?: LearningResource[];
};

export type Roadmap = {
  careerPathId: number;
  careerPathName: string;
  careerPathDescription?: string | null;
  totalSkillNodes: number;
  completedSkillNodes: number;
  completionPercentage: number;
  skillNodes: RoadmapSkillNode[];
};

export async function getRoadmap() {
  const { data } = await api.get<Roadmap>("/roadmap");
  return data;
}

export async function updateRoadmapProgress(
  skillNodeId: number,
  isCompleted: boolean,
) {
  const { data } = await api.put<Roadmap>("/roadmap/progress", {
    skillNodeId,
    isCompleted,
  });
  return data;
}
