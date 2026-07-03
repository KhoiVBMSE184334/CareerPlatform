import { useEffect, useState, type FormEvent } from "react";
import { isAxiosError } from "axios";

import {
  deletePortfolioProject,
  getPortfolioProjects,
  importGitHubPortfolio,
  type PortfolioProject,
} from "../services/portfolioService";
import {
  Badge,
  EmptyState,
  ErrorAlert,
  LoadingSpinner,
  PageHeader,
  SectionCard,
} from "../components/common";

function getApiErrorMessage(error: unknown) {
  if (isAxiosError(error)) {
    const data = error.response?.data as
      | string
      | {
          message?: string;
          title?: string;
          errors?: Record<string, string[]>;
        }
      | undefined;

    if (typeof data === "string") {
      return data.trim() || "Unable to import GitHub portfolio.";
    }

    if (data?.message) {
      return data.message;
    }

    const firstError = Object.values(data?.errors ?? {})[0]?.[0];

    if (firstError) {
      return firstError;
    }

    if (data?.title) {
      return data.title;
    }

    if (error.message) {
      return error.message;
    }
  }

  return "Unable to import GitHub portfolio.";
}

function parseGitHubUsername(input: string) {
  const value = input.trim();

  if (!value) {
    return "";
  }

  let username = value;

  try {
    const url = new URL(value);

    if (!["github.com", "www.github.com"].includes(url.hostname.toLowerCase())) {
      return "";
    }

    username = url.pathname.split("/").filter(Boolean)[0] ?? "";
  } catch {
    username = value.replace(/^@/, "");
  }

  const isValidGitHubUsername =
    /^[A-Za-z0-9](?:[A-Za-z0-9-]{0,37}[A-Za-z0-9])?$/.test(username);

  return isValidGitHubUsername ? username : "";
}

function GitHubEmptyIllustration() {
  return (
    <svg
      aria-label="GitHub portfolio illustration"
      className="h-auto w-64"
      fill="none"
      role="img"
      viewBox="0 0 260 170"
      xmlns="http://www.w3.org/2000/svg"
    >
      <rect fill="#EEF2FF" height="170" rx="28" width="260" />
      <rect fill="white" height="104" rx="20" stroke="#CBD5E1" width="188" x="36" y="34" />
      <circle cx="72" cy="70" fill="#111827" r="22" />
      <path d="M64 70L72 78L84 62" stroke="white" strokeLinecap="round" strokeWidth="5" />
      <rect fill="#C7D2FE" height="12" rx="6" width="94" x="108" y="58" />
      <rect fill="#E2E8F0" height="10" rx="5" width="72" x="108" y="84" />
      <rect fill="#E2E8F0" height="10" rx="5" width="88" x="108" y="106" />
      <path d="M74 138C90 118 108 118 124 138C140 158 166 158 188 132" stroke="#4F46E5" strokeLinecap="round" strokeWidth="6" />
    </svg>
  );
}

function Portfolio() {
  const [projects, setProjects] = useState<PortfolioProject[]>([]);
  const [githubUrl, setGithubUrl] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [isImporting, setIsImporting] = useState(false);
  const [deletingId, setDeletingId] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    getPortfolioProjects()
      .then(setProjects)
      .catch(() => setError("Unable to load portfolio projects."))
      .finally(() => setIsLoading(false));
  }, []);

  const handleImport = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError("");
    setSuccess("");

    if (isImporting) {
      return;
    }

    const githubUsername = parseGitHubUsername(githubUrl);

    if (!githubUsername) {
      setError("Enter a valid GitHub username or github.com profile URL.");
      return;
    }

    setIsImporting(true);

    try {
      const importedProjects = await importGitHubPortfolio(githubUsername);
      setProjects(importedProjects);
      setGithubUrl("");
      setSuccess("GitHub portfolio imported successfully.");
    } catch (importError) {
      setError(getApiErrorMessage(importError));
    } finally {
      setIsImporting(false);
    }
  };

  const handleDelete = async (projectId: string) => {
    setDeletingId(projectId);
    setError("");
    setSuccess("");

    try {
      await deletePortfolioProject(projectId);
      setProjects((current) =>
        current.filter((project) => project.projectId !== projectId),
      );
      setSuccess("Project removed.");
    } catch {
      setError("Unable to remove project.");
    } finally {
      setDeletingId("");
    }
  };

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Showcase"
        title="Portfolio"
        description="Import GitHub projects and prepare a simple career portfolio for interviews."
      />

      <form
        className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm"
        onSubmit={handleImport}
      >
        {error ? <div className="mb-4"><ErrorAlert message={error} /></div> : null}
        {success ? (
          <p className="mb-4 rounded-xl border border-emerald-200 bg-emerald-50 px-3 py-2 text-sm font-medium text-emerald-700">
            {success}
          </p>
        ) : null}
        <label className="text-sm font-medium text-slate-700" htmlFor="github">
          GitHub URL or username
        </label>
        <div className="mt-1 flex flex-col gap-3 sm:flex-row">
          <input
            className="min-w-0 flex-1 rounded-xl border border-slate-300 px-4 py-3 outline-none transition focus:border-indigo-500 focus:ring-4 focus:ring-indigo-100"
            id="github"
            onChange={(event) => setGithubUrl(event.target.value)}
            placeholder="https://github.com/username or username"
            value={githubUrl}
          />
          <button
            className="rounded-xl bg-indigo-600 px-5 py-3 font-semibold text-white transition hover:bg-indigo-700 disabled:cursor-not-allowed disabled:bg-gray-400"
            disabled={isImporting}
            type="submit"
          >
            {isImporting ? "Importing..." : "Import"}
          </button>
        </div>
      </form>

      {isLoading ? <LoadingSpinner label="Loading portfolio..." /> : null}

      <div className="grid gap-4 lg:grid-cols-2">
        {projects.map((project) => (
          <SectionCard className="transition hover:-translate-y-0.5 hover:shadow-md" key={project.projectId}>
            <div className="flex flex-col gap-3 sm:flex-row sm:items-start sm:justify-between">
              <div>
                <h2 className="text-lg font-bold text-slate-950">
                  {project.repositoryName}
                </h2>
                <p className="mt-2 text-sm text-slate-600">
                  {project.description ?? "No description available."}
                </p>
                {project.techStack ? (
                  <div className="mt-3">
                    <Badge tone="violet">{project.techStack}</Badge>
                  </div>
                ) : null}
              </div>
              <button
                className="rounded-lg border border-red-200 px-3 py-2 text-sm font-semibold text-red-700 transition hover:bg-red-50 disabled:cursor-not-allowed disabled:border-gray-300 disabled:bg-gray-50 disabled:text-gray-400"
                disabled={deletingId === project.projectId}
                onClick={() => handleDelete(project.projectId)}
                type="button"
              >
                {deletingId === project.projectId ? "Removing..." : "Remove"}
              </button>
            </div>
            <a
              className="mt-4 inline-block rounded-lg border border-slate-300 px-3 py-2 text-sm font-semibold text-slate-700 transition hover:bg-slate-50"
              href={project.githubUrl}
              rel="noreferrer"
              target="_blank"
            >
              View on GitHub
            </a>
          </SectionCard>
        ))}
      </div>

      {!isLoading && projects.length === 0 ? (
        <EmptyState
          illustration={<GitHubEmptyIllustration />}
          title="No portfolio projects imported yet"
          description="Import a GitHub profile to start building your project showcase."
        />
      ) : null}
    </section>
  );
}

export default Portfolio;
