import api from "../api/api";

export type CareerPath = {
  careerPathId: number;
  name: string;
  description?: string | null;
};

export type CareerPathRequest = {
  name: string;
  description?: string | null;
};

export async function getCareerPaths() {
  const { data } = await api.get<CareerPath[]>("/api/careerpaths");
  return data;
}

export async function createCareerPath(request: CareerPathRequest) {
  const { data } = await api.post<CareerPath>("/api/careerpaths", request);
  return data;
}

export async function updateCareerPath(
  careerPathId: number,
  request: CareerPathRequest,
) {
  const { data } = await api.put<CareerPath>(
    `/api/careerpaths/${careerPathId}`,
    request,
  );
  return data;
}

export async function deleteCareerPath(careerPathId: number) {
  await api.delete(`/api/careerpaths/${careerPathId}`);
}

export async function selectCareerPath(careerPathId: number) {
  const { data } = await api.post<CareerPath>("/api/careerpaths/select", {
    careerPathId,
  });
  return data;
}
