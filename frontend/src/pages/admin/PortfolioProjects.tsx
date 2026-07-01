import { useEffect, useState } from "react";

import {
  getAdminPortfolioProjects,
  type AdminPortfolioProject,
} from "../../services/portfolioService";
import {
  Badge,
  EmptyState,
  ErrorAlert,
  LoadingSpinner,
  PageHeader,
  SectionCard,
} from "../../components/common";

function PortfolioProjects() {
  const [projects, setProjects] = useState<AdminPortfolioProject[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    getAdminPortfolioProjects()
      .then(setProjects)
      .catch(() => setError("Unable to load portfolio projects."))
      .finally(() => setIsLoading(false));
  }, []);

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Showcase"
        title="Portfolio Projects"
        description="View GitHub projects imported by students."
      />

      {isLoading ? <LoadingSpinner label="Loading portfolio projects..." /> : null}

      {error ? <ErrorAlert message={error} /> : null}

      <div className="grid gap-4 xl:grid-cols-2">
        {projects.map((project) => (
          <SectionCard className="transition hover:-translate-y-0.5 hover:shadow-md" key={project.projectId}>
            <div className="flex flex-col gap-3 sm:flex-row sm:items-start sm:justify-between">
              <div>
                <h2 className="text-lg font-bold text-slate-950">
                  {project.repositoryName}
                </h2>
                <p className="mt-1 text-sm text-slate-600">
                  Student: {project.studentName}
                </p>
              </div>
              <p className="text-sm text-slate-500">
                {new Date(project.importedAt).toLocaleDateString()}
              </p>
            </div>

            <p className="mt-3 text-sm leading-6 text-slate-600">
              {project.description ?? "No description available."}
            </p>

            <div className="mt-4 flex flex-wrap items-center gap-3">
              {project.techStack ? (
                <Badge tone="violet">{project.techStack}</Badge>
              ) : null}
              <a
                className="rounded-lg border border-slate-300 px-3 py-2 text-sm font-semibold text-slate-700 transition hover:bg-slate-50"
                href={project.githubUrl}
                rel="noreferrer"
                target="_blank"
              >
                GitHub Link
              </a>
            </div>
          </SectionCard>
        ))}
      </div>

      {!isLoading && projects.length === 0 ? (
        <EmptyState title="No portfolio projects found" />
      ) : null}
    </section>
  );
}

export default PortfolioProjects;
