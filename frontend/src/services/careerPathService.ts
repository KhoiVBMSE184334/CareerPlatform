import api from "./api";

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
  const { data } = await api.get<CareerPath[]>("/careerpaths");
  return data;
}

export async function createCareerPath(request: CareerPathRequest) {
  const { data } = await api.post<CareerPath>("/careerpaths", request);
  return data;
}

export async function updateCareerPath(
  careerPathId: number,
  request: CareerPathRequest,
) {
  const { data } = await api.put<CareerPath>(
    `/careerpaths/${careerPathId}`,
    request,
  );
  return data;
}

export async function deleteCareerPath(careerPathId: number) {
  await api.delete(`/careerpaths/${careerPathId}`);
}

export async function selectCareerPath(careerPathId: number) {
  const { data } = await api.post<CareerPath>("/careerpaths/select", {
    careerPathId,
  });
  return data;
}
