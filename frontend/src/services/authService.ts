import api from "../api/api";
import {
  getRoleFromToken,
  saveAuthSession,
  type AuthSession,
  type AuthUser,
} from "../utils/auth";

export type LoginRequest = {
  email: string;
  password: string;
};

export type RegisterRequest = {
  fullName: string;
  email: string;
  password: string;
};

export type AuthResponse = {
  token: string;
  email: string;
};

function createSession(response: AuthResponse): AuthSession {
  if (!response.token) {
    throw new Error("Login response did not include a JWT token.");
  }

  const user: AuthUser = {
    email: response.email,
    role: getRoleFromToken(response.token),
  };

  return {
    token: response.token,
    user,
  };
}

export async function login(request: LoginRequest) {
  const { data } = await api.post<AuthResponse>("/api/auth/login", request);
  const session = createSession(data);

  saveAuthSession(session);

  return session;
}

export async function register(request: RegisterRequest) {
  const { data } = await api.post<AuthResponse>("/api/auth/register", request);
  const session = createSession(data);

  saveAuthSession(session);

  return session;
}
