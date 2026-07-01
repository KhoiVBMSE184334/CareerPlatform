export type UserRole = "Admin" | "Student";

export type AuthUser = {
  email: string;
  role: UserRole | null;
};

export type AuthSession = {
  token: string;
  user: AuthUser;
};

const tokenKey = "token";
const userKey = "user";
const emailKey = "email";
const roleKey = "role";

const roleClaimKeys = [
  "role",
  "roles",
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
];

const emailClaimKeys = [
  "email",
  "sub",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
];

function decodeJwtPayload(token: string): Record<string, unknown> | null {
  try {
    const payload = token.split(".")[1];

    if (!payload) {
      return null;
    }

    const normalizedPayload = payload.replace(/-/g, "+").replace(/_/g, "/");
    const decodedPayload = atob(normalizedPayload);

    return JSON.parse(decodedPayload) as Record<string, unknown>;
  } catch {
    return null;
  }
}

function firstStringClaim(
  payload: Record<string, unknown> | null,
  keys: string[],
) {
  for (const key of keys) {
    const claim = payload?.[key];

    if (typeof claim === "string") {
      return claim;
    }

    if (Array.isArray(claim) && typeof claim[0] === "string") {
      return claim[0];
    }
  }

  return null;
}

export function getAuthToken() {
  return localStorage.getItem(tokenKey);
}

export function getRoleFromToken(token: string): UserRole | null {
  const role = firstStringClaim(decodeJwtPayload(token), roleClaimKeys);

  if (role === "Admin" || role === "Student") {
    return role;
  }

  return null;
}

export function getStoredUser(): AuthUser | null {
  const storedUser = localStorage.getItem(userKey);

  if (!storedUser) {
    return null;
  }

  try {
    return JSON.parse(storedUser) as AuthUser;
  } catch {
    return null;
  }
}

export function getCurrentUser(): AuthUser | null {
  const storedUser = getStoredUser();
  const token = getAuthToken();

  if (storedUser) {
    return storedUser;
  }

  if (!token) {
    return null;
  }

  const payload = decodeJwtPayload(token);
  const email = firstStringClaim(payload, emailClaimKeys) ?? "";

  return {
    email,
    role: getRoleFromToken(token),
  };
}

export function getCurrentRole(): UserRole | null {
  const storedRole = localStorage.getItem(roleKey);

  if (storedRole === "Admin" || storedRole === "Student") {
    return storedRole;
  }

  const user = getCurrentUser();

  return user?.role ?? null;
}

export function saveAuthSession(session: AuthSession) {
  localStorage.setItem(tokenKey, session.token);
  localStorage.setItem(emailKey, session.user.email);
  localStorage.setItem(userKey, JSON.stringify(session.user));

  if (session.user.role) {
    localStorage.setItem(roleKey, session.user.role);
  } else {
    localStorage.removeItem(roleKey);
  }
}

export function clearAuthSession() {
  localStorage.removeItem(tokenKey);
  localStorage.removeItem(emailKey);
  localStorage.removeItem(userKey);
  localStorage.removeItem(roleKey);
}

export function getDashboardPath(role: UserRole | null) {
  return role === "Admin" ? "/admin/dashboard" : "/student/dashboard";
}
