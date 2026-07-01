import { useEffect, useState } from "react";

import {
  getCareerPaths,
  selectCareerPath,
  type CareerPath as CareerPathItem,
} from "../services/careerPathService";
import {
  EmptyState,
  ErrorAlert,
  LoadingSpinner,
  PageHeader,
  SectionCard,
} from "../components/common";

const careerPathVisuals: Record<
  string,
  { icon: string; description: string; tone: string }
> = {
  "Backend Developer": {
    icon: "BE",
    description: "Build APIs, databases, authentication, and scalable server logic.",
    tone: "from-indigo-600 to-blue-600",
  },
  "Frontend Developer": {
    icon: "FE",
    description: "Create responsive interfaces with polished user experiences.",
    tone: "from-sky-500 to-indigo-600",
  },
  "Full Stack Developer": {
    icon: "FS",
    description: "Connect frontend, backend, APIs, and databases into complete apps.",
    tone: "from-violet-600 to-indigo-600",
  },
  "Mobile Developer": {
    icon: "MB",
    description: "Design and ship mobile experiences for Android and cross-platform apps.",
    tone: "from-emerald-500 to-teal-600",
  },
  "DevOps Engineer": {
    icon: "DO",
    description: "Automate deployments, cloud workflows, CI/CD, and infrastructure.",
    tone: "from-amber-500 to-orange-600",
  },
  "Data Engineer": {
    icon: "DE",
    description: "Prepare data pipelines, storage, transformation, and analytics systems.",
    tone: "from-blue-600 to-cyan-600",
  },
  "AI Engineer": {
    icon: "AI",
    description: "Build intelligent features with models, prompts, and applied AI systems.",
    tone: "from-fuchsia-600 to-violet-600",
  },
};

function getCareerPathVisual(careerPath: CareerPathItem) {
  return (
    careerPathVisuals[careerPath.name] ?? {
      icon: "CP",
      description: careerPath.description ?? "Explore skills and roadmap steps.",
      tone: "from-indigo-600 to-blue-600",
    }
  );
}

function CareerPath() {
  const [careerPaths, setCareerPaths] = useState<CareerPathItem[]>([]);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    getCareerPaths()
      .then(setCareerPaths)
      .catch(() => setError("Unable to load career paths."))
      .finally(() => setIsLoading(false));
  }, []);

  const handleSelect = async (careerPathId: number) => {
    setSelectedId(careerPathId);
    setSuccess("");
    setError("");
    setIsSaving(true);

    try {
      const selectedCareerPath = await selectCareerPath(careerPathId);
      setSuccess(`${selectedCareerPath.name} selected successfully.`);
    } catch {
      setError("Unable to select this career path.");
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Direction"
        title="Career Paths"
        description="Choose a target software engineering role to generate your roadmap."
      />

      {isLoading ? <LoadingSpinner label="Loading career paths..." /> : null}

      {error ? <ErrorAlert message={error} /> : null}

      {success ? (
        <div className="rounded-2xl border border-emerald-200 bg-emerald-50 p-4 text-sm font-medium text-emerald-700">
          {success}
        </div>
      ) : null}

      <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
        {careerPaths.map((careerPath) => {
          const visual = getCareerPathVisual(careerPath);

          return (
            <SectionCard
              className="group overflow-hidden transition duration-200 hover:-translate-y-1 hover:scale-[1.01] hover:border-indigo-200 hover:shadow-lg"
              key={careerPath.careerPathId}
            >
              <div className="relative mb-5 h-28 overflow-hidden rounded-2xl bg-gradient-to-br from-indigo-50 to-violet-50">
                <div
                  className={`absolute left-5 top-5 grid h-16 w-16 place-items-center rounded-2xl bg-gradient-to-br ${visual.tone} text-lg font-bold text-white shadow-lg transition duration-200 group-hover:scale-105`}
                >
                  {visual.icon}
                </div>
                <div className="absolute bottom-4 right-4 h-16 w-24 rounded-full bg-white/70 blur-2xl" />
                <div className="absolute bottom-5 right-5 h-2 w-20 rounded-full bg-indigo-200" />
                <div className="absolute bottom-9 right-5 h-2 w-28 rounded-full bg-white" />
              </div>
              <h2 className="text-lg font-bold text-slate-950">
                {careerPath.name}
              </h2>
              <p className="mt-2 min-h-16 text-sm leading-6 text-slate-600">
                {careerPath.description ?? visual.description}
              </p>
              <button
                className="mt-4 rounded-xl bg-indigo-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-indigo-700 disabled:cursor-not-allowed disabled:bg-gray-400"
                disabled={isSaving && selectedId === careerPath.careerPathId}
                onClick={() => handleSelect(careerPath.careerPathId)}
                type="button"
              >
                {isSaving && selectedId === careerPath.careerPathId
                  ? "Selecting..."
                  : "Select path"}
              </button>
            </SectionCard>
          );
        })}
      </div>

      {!isLoading && careerPaths.length === 0 ? (
        <EmptyState
          title="No career paths available"
          description="Ask an admin to create career paths before selecting a roadmap."
        />
      ) : null}
    </section>
  );
}

export default CareerPath;
