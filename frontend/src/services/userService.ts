import api from "./api";

export type AdminUser = {
  userId: string;
  fullName: string;
  email: string;
  role: "Admin" | "Student";
  createdAt?: string;
};

export async function getUsers() {
  const { data } = await api.get<AdminUser[]>("/users");
  return data;
}

export async function deleteUser(userId: string) {
  await api.delete(`/users/${userId}`);
}
