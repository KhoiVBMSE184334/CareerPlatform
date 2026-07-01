import { Link, Outlet } from "react-router-dom";

function PublicLayout() {
  return (
    <div className="relative isolate min-h-screen overflow-hidden bg-slate-50 bg-gradient-to-br from-indigo-50 via-white to-violet-50 text-slate-950">
      <div className="pointer-events-none absolute right-[-8rem] top-24 -z-10 h-80 w-80 rounded-full bg-violet-400 opacity-20 blur-3xl" />
      <div className="pointer-events-none absolute bottom-[-10rem] left-[-8rem] -z-10 h-96 w-96 rounded-full bg-blue-400 opacity-20 blur-3xl" />
      <header className="sticky top-0 z-30 border-b border-slate-200/80 bg-white/85 backdrop-blur">
        <nav className="mx-auto flex max-w-7xl items-center justify-between px-4 py-4 sm:px-6">
          <Link className="flex items-center gap-3 text-lg font-bold" to="/">
            <span className="grid h-10 w-10 place-items-center rounded-xl bg-indigo-600 text-sm text-white shadow-sm">
              CP
            </span>
            <span>CareerPlatform</span>
          </Link>
          <div className="flex items-center gap-3 text-sm font-medium">
            <Link className="text-slate-600 transition hover:text-indigo-700" to="/login">
              Login
            </Link>
            <Link
              className="rounded-lg bg-indigo-600 px-4 py-2 text-white shadow-sm transition hover:bg-indigo-700"
              to="/register"
            >
              Register
            </Link>
          </div>
        </nav>
      </header>

      <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:py-12">
        <Outlet />
      </main>
    </div>
  );
}

export default PublicLayout;
