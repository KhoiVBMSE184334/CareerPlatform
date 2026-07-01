import api from "./api";

export type SkillGapSkill = {
  skillNodeId: number;
  name: string;
  description?: string | null;
  difficulty: string;
  displayOrder: number;
  estimatedHours?: number | null;
};

export type SkillGapResult = {
  careerPathId: number;
  careerPathName: string;
  totalRequiredSkills: number;
  matchedSkillCount: number;
  missingSkillCount: number;
  matchPercentage: number;
  matchedSkills: SkillGapSkill[];
  missingSkills: SkillGapSkill[];
  recommendedLearningPriority: SkillGapSkill[];
};

export type SkillGapRequest = {
  careerPathId: number;
  skills: string[];
};

export async function analyzeSkillGap(request: SkillGapRequest) {
  const { data } = await api.post<SkillGapResult>("/skillgap/analyze", request);
  return data;
}

export async function getMySkillGapResult() {
  const { data } = await api.get<SkillGapResult>("/skillgap/my-result");
  return data;
}
