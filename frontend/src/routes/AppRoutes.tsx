import { BrowserRouter, Navigate, Routes, Route } from "react-router-dom";

import CareerPaths from "../admin/CareerPaths";
import AdminDashboard from "../admin/Dashboard";
import Users from "../admin/Users";
import AdminLayout from "../layouts/AdminLayout";
import PortfolioProjects from "../pages/admin/PortfolioProjects";
import SkillNodes from "../pages/admin/SkillNodes";
import Home from "../pages/Home";
import PublicLayout from "../layouts/PublicLayout";
import StudentLayout from "../layouts/StudentLayout";
import Login from "../pages/auth/Login";
import Register from "../pages/auth/Register";
import AIMentor from "../student/AIMentor";
import CareerPath from "../student/CareerPath";
import StudentDashboard from "../student/Dashboard";
import Portfolio from "../student/Portfolio";
import Profile from "../student/Profile";
import Roadmap from "../student/Roadmap";
import SkillGap from "../student/SkillGap";
import ProtectedRoute from "./ProtectedRoute";
import RoleRoute from "./RoleRoute";

function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<PublicLayout />}>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Route>

        <Route element={<ProtectedRoute />}>
          <Route element={<RoleRoute allowedRoles={["Student"]} />}>
            <Route element={<StudentLayout />} path="/student">
              <Route index element={<Navigate replace to="/student/dashboard" />} />
              <Route path="dashboard" element={<StudentDashboard />} />
              <Route path="career-path" element={<CareerPath />} />
              <Route path="roadmap" element={<Roadmap />} />
              <Route path="skill-gap" element={<SkillGap />} />
              <Route path="mentor" element={<AIMentor />} />
              <Route path="portfolio" element={<Portfolio />} />
              <Route path="profile" element={<Profile />} />
            </Route>
          </Route>

          <Route element={<RoleRoute allowedRoles={["Admin"]} />}>
            <Route element={<AdminLayout />} path="/admin">
              <Route index element={<Navigate replace to="/admin/dashboard" />} />
              <Route path="dashboard" element={<AdminDashboard />} />
              <Route path="users" element={<Users />} />
              <Route path="career-paths" element={<CareerPaths />} />
              <Route path="skill-nodes" element={<SkillNodes />} />
              <Route path="portfolio-projects" element={<PortfolioProjects />} />
            </Route>
          </Route>
        </Route>

        <Route
          path="*"
          element={
            <div className="grid min-h-screen place-items-center bg-slate-50 px-6 text-center">
              <div>
                <h1 className="text-3xl font-bold text-slate-950">
                  Page not found
                </h1>
                <p className="mt-2 text-slate-600">
                  The page you requested does not exist.
                </p>
              </div>
            </div>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}

export default AppRoutes;
