import { useEffect, useState } from "react";

import {
  getRoadmap,
  updateRoadmapProgress,
  type Roadmap as RoadmapData,
} from "../services/roadmapService";
import {
  Badge,
  EmptyState,
  ErrorAlert,
  LoadingSpinner,
  PageHeader,
  ProgressBar,
  SectionCard,
} from "../components/common";

function Roadmap() {
  const [roadmap, setRoadmap] = useState<RoadmapData | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [updatingId, setUpdatingId] = useState<number | null>(null);
  const [error, setError] = useState("");

  useEffect(() => {
    getRoadmap()
      .then(setRoadmap)
      .catch(() => setError("Select a career path before loading a roadmap."))
      .finally(() => setIsLoading(false));
  }, []);

  const handleToggle = async (skillNodeId: number, isCompleted: boolean) => {
    setUpdatingId(skillNodeId);
    setError("");

    try {
      const updatedRoadmap = await updateRoadmapProgress(
        skillNodeId,
        isCompleted,
      );
      setRoadmap(updatedRoadmap);
    } catch {
      setError("Unable to update roadmap progress.");
    } finally {
      setUpdatingId(null);
    }
  };

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Learning plan"
        title="Roadmap"
        description="Follow required skills in order and mark your progress as you learn."
      />

      {isLoading ? <LoadingSpinner label="Loading roadmap..." /> : null}

      {error ? <ErrorAlert message={error} /> : null}

      {roadmap ? (
        <>
          <SectionCard>
            <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
              <div>
                <h2 className="text-xl font-bold text-slate-950">
                  {roadmap.careerPathName}
                </h2>
                <p className="text-sm text-slate-600">
                  {roadmap.completedSkillNodes}/{roadmap.totalSkillNodes} skills
                  completed
                </p>
              </div>
              <p className="text-2xl font-bold">
                {roadmap.completionPercentage}%
              </p>
            </div>
            <div className="mt-4">
              <ProgressBar value={roadmap.completionPercentage} tone="emerald" />
            </div>
          </SectionCard>

          <div className="relative space-y-4 before:absolute before:left-5 before:top-4 before:hidden before:h-[calc(100%-2rem)] before:w-px before:bg-slate-200 md:before:block">
            {roadmap.skillNodes.map((node) => {
              const isCompleted = node.isCompleted ?? node.completed ?? false;

              return (
                <article
                  className={`relative rounded-2xl border bg-white p-5 shadow-sm transition hover:-translate-y-0.5 hover:shadow-md md:ml-12 ${
                    isCompleted
                      ? "border-emerald-200"
                      : "border-slate-200 hover:border-indigo-200"
                  }`}
                  key={node.skillNodeId}
                >
                  <span
                    className={`absolute -left-[3.25rem] top-5 hidden h-10 w-10 place-items-center rounded-full border-4 border-slate-50 text-sm font-bold md:grid ${
                      isCompleted
                        ? "bg-emerald-500 text-white"
                        : "bg-white text-slate-500 ring-1 ring-slate-200"
                    }`}
                  >
                    {node.displayOrder}
                  </span>
                  <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
                    <div>
                      <div className="flex flex-wrap items-center gap-2">
                        <h3 className="text-lg font-bold text-slate-950">{node.name}</h3>
                        <Badge tone={isCompleted ? "emerald" : "indigo"}>
                          {isCompleted ? "Completed" : node.difficulty || "Core"}
                        </Badge>
                      </div>
                      <p className="mt-2 text-sm text-slate-600">
                        {node.description ?? "No description available."}
                      </p>
                      {node.estimatedHours ? (
                        <p className="mt-2 text-sm text-slate-500">
                          Estimated time: {node.estimatedHours} hours
                        </p>
                      ) : null}
                      {node.learningResources?.length ? (
                        <div className="mt-4 flex flex-wrap gap-2">
                          {node.learningResources.map((resource) => (
                            <a
                              className="rounded-lg border border-slate-200 px-3 py-1.5 text-xs font-semibold text-indigo-700 transition hover:border-indigo-200 hover:bg-indigo-50"
                              href={resource.url}
                              key={resource.learningResourceId}
                              rel="noreferrer"
                              target="_blank"
                            >
                              {resource.title}
                            </a>
                          ))}
                        </div>
                      ) : null}
                    </div>
                    <button
                      className={`rounded-lg px-4 py-2 text-sm font-semibold shadow-sm transition disabled:cursor-not-allowed ${
                        isCompleted
                          ? "border border-slate-300 text-slate-700 hover:bg-slate-100 disabled:border-gray-300 disabled:bg-gray-50 disabled:text-gray-400"
                          : "bg-indigo-600 text-white hover:bg-indigo-700 disabled:bg-gray-400"
                      }`}
                      disabled={updatingId === node.skillNodeId}
                      onClick={() =>
                        handleToggle(node.skillNodeId, !isCompleted)
                      }
                      type="button"
                    >
                      {updatingId === node.skillNodeId
                        ? "Saving..."
                        : isCompleted
                          ? "Mark incomplete"
                          : "Mark complete"}
                    </button>
                  </div>
                </article>
              );
            })}
          </div>
        </>
      ) : !isLoading && !error ? (
        <EmptyState
          title="No roadmap available"
          description="Select a career path first to generate your roadmap."
        />
      ) : null}
    </section>
  );
}

export default Roadmap;
