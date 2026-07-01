import { Navigate, Outlet } from "react-router-dom";

import { getCurrentRole, type UserRole } from "../utils/auth";

type RoleRouteProps = {
  allowedRoles: UserRole[];
};

function RoleRoute({ allowedRoles }: RoleRouteProps) {
  const currentRole = getCurrentRole();
  const canAccess = currentRole ? allowedRoles.includes(currentRole) : false;

  if (!canAccess) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
}

export default RoleRoute;
