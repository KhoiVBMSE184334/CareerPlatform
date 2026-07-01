import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import {
  getStudentDashboard,
  type StudentDashboard as StudentDashboardData,
} from "../services/dashboardService";

type StatCardProps = {
  accentClass: string;
  description: string;
  icon: string;
  label: string;
  linkLabel?: string;
  progress?: number;
  to?: string;
  value: string | number;
};

type QuickAction = {
  description: string;
  label: string;
  to: string;
};

function clampPercent(value: number) {
  return Math.min(100, Math.max(0, value));
}

function StatCard({
  accentClass,
  description,
  icon,
  label,
  linkLabel,
  progress,
  to,
  value,
}: StatCardProps) {
  const content = (
    <div className="h-full rounded-2xl border border-slate-200 bg-white/95 p-5 shadow-sm backdrop-blur transition duration-200 hover:-translate-y-1 hover:scale-[1.01] hover:border-indigo-200 hover:shadow-lg">
      <div className="flex items-start justify-between gap-4">
        <div>
          <p className="text-sm font-medium text-slate-500">{label}</p>
          <p className="mt-2 text-3xl font-bold tracking-tight text-slate-950">
            {value}
          </p>
        </div>
        <div
          className={`grid h-12 w-12 place-items-center rounded-2xl text-sm font-bold shadow-sm ${accentClass}`}
        >
          {icon}
        </div>
      </div>

      <p className="mt-3 text-sm leading-6 text-slate-600">{description}</p>

      {typeof progress === "number" ? (
        <div className="mt-4">
          <div className="h-2.5 overflow-hidden rounded-full bg-slate-100">
            <div
              className="h-full rounded-full bg-indigo-600 transition-all"
              style={{ width: `${clampPercent(progress)}%` }}
            />
          </div>
        </div>
      ) : null}

      {to && linkLabel ? (
        <p className="mt-4 text-sm font-semibold text-slate-950">
          {linkLabel}
        </p>
      ) : null}
    </div>
  );

  if (!to) {
    return content;
  }

  return (
    <Link className="block h-full" to={to}>
      {content}
    </Link>
  );
}

function QuickActionCard({ action }: { action: QuickAction }) {
  return (
    <Link
      className="rounded-2xl border border-slate-200 bg-white/95 p-4 shadow-sm transition duration-200 hover:-translate-y-1 hover:scale-[1.01] hover:border-indigo-200 hover:shadow-lg"
      to={action.to}
    >
      <p className="font-semibold text-slate-950">{action.label}</p>
      <p className="mt-1 text-sm leading-6 text-slate-600">
        {action.description}
      </p>
    </Link>
  );
}

function StudentDashboard() {
  const [dashboard, setDashboard] = useState<StudentDashboardData | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    let isMounted = true;

    getStudentDashboard()
      .then((data) => {
        if (isMounted) {
          setDashboard(data);
        }
      })
      .catch(() => {
        if (isMounted) {
          setError("Unable to load your dashboard.");
        }
      })
      .finally(() => {
        if (isMounted) {
          setIsLoading(false);
        }
      });

    return () => {
      isMounted = false;
    };
  }, []);

  const roadmapCompletion = dashboard?.roadmapCompletionPercentage ?? 0;
  const skillMatch = dashboard?.skillMatchPercentage ?? 0;
  const recentChats = dashboard?.recentAIChatSessions ?? [];

  const quickActions: QuickAction[] = [
    {
      label: "View Roadmap",
      description: "Continue your structured learning path.",
      to: "/student/roadmap",
    },
    {
      label: "Analyze Skill Gap",
      description: "Compare your skills with your career target.",
      to: "/student/skill-gap",
    },
    {
      label: "Chat with AI Mentor",
      description: "Ask for guidance on what to learn next.",
      to: "/student/mentor",
    },
    {
      label: "Manage Portfolio",
      description: "Import and review your GitHub projects.",
      to: "/student/portfolio",
    },
  ];

  if (isLoading) {
    return (
      <div className="rounded-2xl border border-slate-200 bg-white/90 p-6 shadow-sm backdrop-blur">
        <p className="text-sm font-medium text-slate-500">
          Loading dashboard...
        </p>
        <div className="mt-4 grid gap-4 md:grid-cols-3">
          <div className="h-32 rounded-2xl bg-slate-100" />
          <div className="h-32 rounded-2xl bg-slate-100" />
          <div className="h-32 rounded-2xl bg-slate-100" />
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-2xl border border-red-200 bg-red-50 p-6 text-red-700 shadow-sm">
        {error}
      </div>
    );
  }

  return (
    <section
      className="space-y-6 rounded-[2rem]"
      style={{
        backgroundImage:
          "radial-gradient(circle at 1px 1px, rgba(99, 102, 241, 0.10) 1px, transparent 0)",
        backgroundSize: "24px 24px",
      }}
    >
      <div className="relative overflow-hidden rounded-[1.75rem] border border-white/80 bg-gradient-to-br from-indigo-600 via-blue-600 to-violet-600 shadow-xl shadow-indigo-200/70">
        <div className="pointer-events-none absolute right-[-4rem] top-[-5rem] h-52 w-52 rounded-full bg-white/20 blur-3xl" />
        <div className="pointer-events-none absolute bottom-[-5rem] left-12 h-48 w-48 rounded-full bg-cyan-300/20 blur-3xl" />
        <div className="relative p-6">
          <p className="text-sm font-semibold uppercase tracking-wide text-indigo-100">
            Student Dashboard
          </p>
          <div className="mt-2 flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
            <div>
              <h1 className="text-3xl font-bold tracking-tight text-white">
                Career progress overview
              </h1>
              <p className="mt-2 max-w-2xl text-indigo-50">
                Track your roadmap, skill readiness, AI guidance, and portfolio
                work from one place.
              </p>
            </div>
            <Link
              className="w-fit rounded-xl bg-white px-4 py-2 text-sm font-semibold text-indigo-700 shadow-sm transition duration-200 hover:-translate-y-0.5 hover:bg-indigo-50"
              to="/student/career-path"
            >
              Choose career path
            </Link>
          </div>
        </div>
      </div>

      <div className="grid gap-4 md:grid-cols-3">
        <StatCard
          accentClass="bg-emerald-50 text-emerald-700"
          description="Completed roadmap skills for your selected career path."
          icon="RD"
          label="Roadmap Completion"
          progress={roadmapCompletion}
          to="/student/roadmap"
          value={`${roadmapCompletion}%`}
        />
        <StatCard
          accentClass="bg-sky-50 text-sky-700"
          description="How closely your current skills match your target role."
          icon="SM"
          label="Skill Match"
          progress={skillMatch}
          to="/student/skill-gap"
          value={`${skillMatch}%`}
        />
        <StatCard
          accentClass="bg-violet-50 text-violet-700"
          description="Imported GitHub projects ready for your portfolio."
          icon="PF"
          label="Portfolio Projects"
          linkLabel="Open portfolio"
          to="/student/portfolio"
          value={dashboard?.portfolioProjectCount ?? 0}
        />
      </div>

      <div className="grid gap-4 xl:grid-cols-[1.1fr_0.9fr]">
        <div className="rounded-2xl border border-slate-200 bg-white/95 p-6 shadow-sm backdrop-blur">
          <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
            <div>
              <span className="rounded-full bg-indigo-50 px-3 py-1 text-xs font-bold uppercase tracking-wide text-indigo-700 ring-1 ring-indigo-100">
                Career Path
              </span>
              <h2 className="mt-3 text-xl font-bold text-slate-950">
                Selected career path
              </h2>
            </div>
            <Link
              className="rounded-xl border border-slate-300 px-3 py-2 text-sm font-semibold text-slate-700 transition hover:bg-slate-100"
              to="/student/roadmap"
            >
              View roadmap
            </Link>
          </div>

          {dashboard?.selectedCareerPath ? (
            <div className="mt-5 rounded-2xl bg-gradient-to-br from-slate-50 to-indigo-50 p-5">
              <p className="text-lg font-bold text-slate-950">
                {dashboard.selectedCareerPath.name}
              </p>
              <p className="mt-2 leading-7 text-slate-600">
                {dashboard.selectedCareerPath.description ??
                  "No description available."}
              </p>
            </div>
          ) : (
            <div className="mt-5 rounded-2xl border border-dashed border-slate-300 p-5">
              <p className="font-semibold text-slate-950">
                No career path selected yet.
              </p>
              <p className="mt-1 text-sm text-slate-600">
                Choose a target role to unlock roadmap and skill insights.
              </p>
            </div>
          )}
        </div>

        <div className="rounded-2xl border border-slate-200 bg-white/95 p-6 shadow-sm backdrop-blur">
          <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
            <div>
              <h2 className="text-xl font-bold text-slate-950">
                Missing skills
              </h2>
              <p className="mt-1 text-sm text-slate-600">
                Focus areas from your latest analysis.
              </p>
            </div>
            <Link
              className="rounded-xl border border-slate-300 px-3 py-2 text-sm font-semibold text-slate-700 transition hover:bg-slate-100"
              to="/student/skill-gap"
            >
              Analyze
            </Link>
          </div>

          {dashboard?.missingSkillsSummary.length ? (
            <div className="mt-5 flex flex-wrap gap-2">
              {dashboard.missingSkillsSummary.map((skill) => (
                <span
                  className="rounded-full bg-amber-50 px-3 py-1.5 text-sm font-semibold text-amber-800 ring-1 ring-amber-100"
                  key={skill}
                >
                  {skill}
                </span>
              ))}
            </div>
          ) : (
            <div className="mt-5 rounded-2xl border border-emerald-200 bg-emerald-50 p-4 text-emerald-800">
              <p className="font-semibold">
                No missing skills. You're on track!
              </p>
            </div>
          )}
        </div>
      </div>

      <div className="grid gap-4 xl:grid-cols-[0.9fr_1.1fr]">
        <div className="rounded-2xl border border-slate-200 bg-white/95 p-6 shadow-sm backdrop-blur">
          <h2 className="text-xl font-bold text-slate-950">
            Recent AI chats
          </h2>
          <p className="mt-1 text-sm text-slate-600">
            Recent mentor sessions from your learning history.
          </p>

          {recentChats.length ? (
            <div className="mt-5 space-y-3">
              {recentChats.slice(0, 3).map((session) => (
                <Link
                  className="block rounded-2xl border border-slate-200 p-4 transition duration-200 hover:-translate-y-0.5 hover:border-indigo-200 hover:bg-indigo-50"
                  key={session.sessionId}
                  to="/student/mentor"
                >
                  <p className="font-semibold text-slate-950">
                    {new Date(session.createdAt).toLocaleDateString()}
                  </p>
                  <p className="mt-1 text-sm text-slate-600">
                    {session.messages.length} messages
                  </p>
                </Link>
              ))}
            </div>
          ) : (
            <div className="mt-5 rounded-2xl border border-dashed border-slate-300 p-5">
              <p className="font-semibold text-slate-950">
                No mentor chats yet.
              </p>
              <p className="mt-1 text-sm text-slate-600">
                Start a conversation to get personalized guidance.
              </p>
              <Link
                className="mt-4 inline-block rounded-xl bg-indigo-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-indigo-700"
                to="/student/mentor"
              >
                Open AI Mentor
              </Link>
            </div>
          )}
        </div>

        <div className="rounded-2xl border border-slate-200 bg-white/95 p-6 shadow-sm backdrop-blur">
          <h2 className="text-xl font-bold text-slate-950">Quick Actions</h2>
          <p className="mt-1 text-sm text-slate-600">
            Jump into the workflows students use most during the demo.
          </p>
          <div className="mt-5 grid gap-3 sm:grid-cols-2">
            {quickActions.map((action) => (
              <QuickActionCard action={action} key={action.to} />
            ))}
          </div>
        </div>
      </div>
    </section>
  );
}

export default StudentDashboard;
