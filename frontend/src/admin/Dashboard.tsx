import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import {
  ErrorAlert,
  LoadingSpinner,
  PageHeader,
  StatCard,
} from "../components/common";
import {
  getAdminDashboard,
  type AdminDashboard as AdminDashboardData,
} from "../services/dashboardService";

function AdminDashboard() {
  const [dashboard, setDashboard] = useState<AdminDashboardData | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    getAdminDashboard()
      .then(setDashboard)
      .catch(() => setError("Unable to load admin dashboard statistics."))
      .finally(() => setIsLoading(false));
  }, []);

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Admin"
        title="Dashboard"
        description="Monitor platform activity and manage capstone demo data."
      />

      <div className="relative overflow-hidden rounded-[1.75rem] border border-white/80 bg-gradient-to-br from-slate-900 via-indigo-900 to-slate-800 p-6 text-white shadow-xl shadow-slate-300/70">
        <div className="pointer-events-none absolute -right-10 -top-12 h-44 w-44 rounded-full bg-violet-400/25 blur-3xl" />
        <div className="pointer-events-none absolute -bottom-16 left-12 h-44 w-44 rounded-full bg-blue-400/20 blur-3xl" />
        <div className="relative flex flex-col gap-4 md:flex-row md:items-end md:justify-between">
          <div>
            <p className="text-sm font-semibold uppercase tracking-wide text-indigo-200">
              Platform control center
            </p>
            <h2 className="mt-2 text-2xl font-bold tracking-tight md:text-3xl">
              Manage users, paths, skills, and portfolio activity.
            </h2>
            <p className="mt-2 max-w-2xl text-sm leading-6 text-slate-200">
              Keep the demo data visible and organized with quick access to the
              most important admin workflows.
            </p>
          </div>
          <div className="grid grid-cols-3 gap-2 text-center text-xs font-semibold text-slate-200">
            <span className="rounded-xl bg-white/10 px-3 py-2">Users</span>
            <span className="rounded-xl bg-white/10 px-3 py-2">Roadmaps</span>
            <span className="rounded-xl bg-white/10 px-3 py-2">Projects</span>
          </div>
        </div>
      </div>

      {isLoading ? <LoadingSpinner label="Loading statistics..." /> : null}

      {error ? <ErrorAlert message={error} /> : null}

      {dashboard ? (
        <>
          <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-3">
            <StatCard
              icon="US"
              label="Total users"
              tone="indigo"
              to="/admin/users"
              value={dashboard.totalUsers}
            />
            <StatCard icon="ST" label="Students" tone="emerald" value={dashboard.totalStudents} />
            <StatCard icon="AD" label="Admins" tone="slate" value={dashboard.totalAdmins} />
            <StatCard
              icon="CP"
              label="Career paths"
              tone="violet"
              to="/admin/career-paths"
              value={dashboard.totalCareerPaths}
            />
            <StatCard
              icon="SN"
              label="Skill nodes"
              tone="amber"
              to="/admin/skill-nodes"
              value={dashboard.totalSkillNodes}
            />
            <StatCard
              icon="PF"
              label="Portfolio projects"
              tone="emerald"
              to="/admin/portfolio-projects"
              value={dashboard.totalPortfolioProjects}
            />
          </div>

          <div className="grid gap-4 lg:grid-cols-4">
            <Link
              className="rounded-2xl border border-slate-200 bg-white/95 p-5 shadow-sm transition duration-200 hover:-translate-y-1 hover:scale-[1.01] hover:border-indigo-200 hover:shadow-lg"
              to="/admin/users"
            >
              <h2 className="font-semibold">View Users</h2>
              <p className="mt-1 text-sm text-zinc-600">
                Review admin and student accounts.
              </p>
            </Link>
            <Link
              className="rounded-2xl border border-slate-200 bg-white/95 p-5 shadow-sm transition duration-200 hover:-translate-y-1 hover:scale-[1.01] hover:border-indigo-200 hover:shadow-lg"
              to="/admin/skill-nodes"
            >
              <h2 className="font-semibold">View Skill Nodes</h2>
              <p className="mt-1 text-sm text-zinc-600">
                Inspect roadmap skills and resources.
              </p>
            </Link>
            <Link
              className="rounded-2xl border border-slate-200 bg-white/95 p-5 shadow-sm transition duration-200 hover:-translate-y-1 hover:scale-[1.01] hover:border-indigo-200 hover:shadow-lg"
              to="/admin/career-paths"
            >
              <h2 className="font-semibold">Manage Career Paths</h2>
              <p className="mt-1 text-sm text-zinc-600">
                Skill nodes are managed through Career Path Management.
              </p>
            </Link>
            <Link
              className="rounded-2xl border border-slate-200 bg-white/95 p-5 shadow-sm transition duration-200 hover:-translate-y-1 hover:scale-[1.01] hover:border-indigo-200 hover:shadow-lg"
              to="/admin/portfolio-projects"
            >
              <h2 className="font-semibold">View Portfolio Projects</h2>
              <p className="mt-1 text-sm text-zinc-600">
                Review imported GitHub repositories.
              </p>
            </Link>
          </div>
        </>
      ) : null}
    </section>
  );
}

export default AdminDashboard;
