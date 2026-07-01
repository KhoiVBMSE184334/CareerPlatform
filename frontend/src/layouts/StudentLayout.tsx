import { Link, NavLink, Outlet, useNavigate } from "react-router-dom";

import { clearAuthSession } from "../utils/auth";

const studentLinks = [
  { to: "/student/dashboard", label: "Dashboard", icon: "DB" },
  { to: "/student/career-path", label: "Career Path", icon: "CP" },
  { to: "/student/roadmap", label: "Roadmap", icon: "RM" },
  { to: "/student/skill-gap", label: "Skill Gap", icon: "SG" },
  { to: "/student/mentor", label: "AI Mentor", icon: "AI" },
  { to: "/student/portfolio", label: "Portfolio", icon: "PF" },
  { to: "/student/profile", label: "Profile", icon: "PR" },
];

function StudentLayout() {
  const navigate = useNavigate();

  const handleLogout = () => {
    clearAuthSession();
    navigate("/login", { replace: true });
  };

  return (
    <div className="relative isolate min-h-screen overflow-hidden bg-slate-50 bg-gradient-to-br from-indigo-50 via-white to-violet-50 text-slate-950">
      <div className="pointer-events-none absolute right-[-8rem] top-24 -z-10 h-80 w-80 rounded-full bg-violet-400 opacity-20 blur-3xl" />
      <div className="pointer-events-none absolute bottom-[-10rem] left-[-8rem] -z-10 h-96 w-96 rounded-full bg-blue-400 opacity-20 blur-3xl" />
      <header className="sticky top-0 z-30 border-b border-slate-200/80 bg-white/85 backdrop-blur">
        <nav className="mx-auto flex max-w-7xl items-center justify-between px-4 py-4 sm:px-6">
          <Link className="flex items-center gap-3 text-lg font-bold" to="/student/dashboard">
            <span className="grid h-10 w-10 place-items-center rounded-xl bg-indigo-600 text-sm text-white shadow-sm">
              CP
            </span>
            <span>CareerPlatform</span>
          </Link>
          <button
            className="rounded-lg border border-slate-300 bg-white px-4 py-2 text-sm font-semibold text-slate-700 shadow-sm transition hover:bg-slate-50"
            onClick={handleLogout}
            type="button"
          >
            Logout
          </button>
        </nav>
      </header>

      <div className="mx-auto grid max-w-7xl gap-6 px-4 py-6 sm:px-6 lg:grid-cols-[260px_1fr]">
        <aside className="h-fit rounded-2xl border border-slate-200 bg-white/95 p-3 shadow-sm lg:sticky lg:top-24">
          <p className="px-3 pb-3 pt-2 text-xs font-bold uppercase tracking-wider text-slate-400">
            Student Workspace
          </p>
          <nav className="grid gap-1 sm:grid-cols-2 lg:grid-cols-1">
            {studentLinks.map((link) => (
              <NavLink
                className={({ isActive }) =>
                  `flex items-center gap-3 rounded-xl px-3 py-2.5 text-sm font-semibold transition ${
                    isActive
                      ? "bg-indigo-600 text-white shadow-sm"
                      : "text-slate-600 hover:bg-slate-100 hover:text-slate-950"
                  }`
                }
                end={link.to === "/student/dashboard"}
                key={link.to}
                to={link.to}
              >
                <span className="grid h-7 w-7 place-items-center rounded-lg bg-white/20 text-[11px] font-bold">
                  {link.icon}
                </span>
                {link.label}
              </NavLink>
            ))}
          </nav>
        </aside>

        <main className="min-w-0 pb-10">
          <Outlet />
        </main>
      </div>
    </div>
  );
}

export default StudentLayout;
