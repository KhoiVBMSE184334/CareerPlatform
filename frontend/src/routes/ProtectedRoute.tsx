import { Navigate, Outlet, useLocation } from "react-router-dom";

import { getAuthToken } from "../utils/auth";

function ProtectedRoute() {
  const location = useLocation();
  const token = getAuthToken();

  if (!token) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  return <Outlet />;
}

export default ProtectedRoute;
