import { useEffect, useState, type FormEvent } from "react";

import {
  getCareerPaths,
  type CareerPath,
} from "../services/careerPathService";
import {
  analyzeSkillGap,
  getMySkillGapResult,
  type SkillGapResult,
} from "../services/skillGapService";
import {
  Badge,
  EmptyState,
  ErrorAlert,
  LoadingSpinner,
  PageHeader,
  ProgressBar,
  SectionCard,
} from "../components/common";

function SkillList({
  title,
  skills,
}: {
  title: string;
  skills: SkillGapResult["missingSkills"];
}) {
  return (
    <SectionCard>
      <h2 className="text-lg font-bold text-slate-950">{title}</h2>
      {skills.length ? (
        <div className="mt-4 flex flex-wrap gap-2">
          {skills.map((skill) => (
            <Badge key={skill.skillNodeId} tone={title.includes("Missing") ? "amber" : "emerald"}>
              {skill.name}
            </Badge>
          ))}
        </div>
      ) : (
        <p className="mt-3 text-sm text-slate-600">No skills to show.</p>
      )}
    </SectionCard>
  );
}

function SkillGap() {
  const [careerPaths, setCareerPaths] = useState<CareerPath[]>([]);
  const [careerPathId, setCareerPathId] = useState("");
  const [skills, setSkills] = useState("");
  const [result, setResult] = useState<SkillGapResult | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isAnalyzing, setIsAnalyzing] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    Promise.allSettled([getCareerPaths(), getMySkillGapResult()])
      .then(([careerPathResult, skillGapResult]) => {
        if (careerPathResult.status === "fulfilled") {
          setCareerPaths(careerPathResult.value);
          setCareerPathId(String(careerPathResult.value[0]?.careerPathId ?? ""));
        }

        if (skillGapResult.status === "fulfilled") {
          setResult(skillGapResult.value);
          setCareerPathId(String(skillGapResult.value.careerPathId));
        }
      })
      .catch(() => setError("Unable to load skill gap data."))
      .finally(() => setIsLoading(false));
  }, []);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError("");

    const parsedCareerPathId = Number(careerPathId);
    const parsedSkills = skills
      .split(",")
      .map((skill) => skill.trim())
      .filter(Boolean);

    if (!parsedCareerPathId) {
      setError("Please choose a career path.");
      return;
    }

    if (parsedSkills.length === 0) {
      setError("Enter at least one skill, separated by commas.");
      return;
    }

    setIsAnalyzing(true);

    try {
      const analysis = await analyzeSkillGap({
        careerPathId: parsedCareerPathId,
        skills: parsedSkills,
      });
      setResult(analysis);
    } catch {
      setError("Unable to analyze your skills.");
    } finally {
      setIsAnalyzing(false);
    }
  };

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Analysis"
        title="Skill Gap Analysis"
        description="Compare your current skills against a target career path and get a focused learning priority."
      />

      <form
        className="grid gap-4 rounded-2xl border border-slate-200 bg-white p-5 shadow-sm"
        onSubmit={handleSubmit}
      >
        {error ? <ErrorAlert message={error} /> : null}

        <div>
          <label
            className="text-sm font-medium text-slate-700"
            htmlFor="careerPathId"
          >
            Career path
          </label>
          <select
            className="mt-1 w-full rounded-md border border-slate-300 px-3 py-2"
            disabled={isLoading}
            id="careerPathId"
            onChange={(event) => setCareerPathId(event.target.value)}
            value={careerPathId}
          >
            <option value="">Select a career path</option>
            {careerPaths.map((careerPath) => (
              <option
                key={careerPath.careerPathId}
                value={careerPath.careerPathId}
              >
                {careerPath.name}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label className="text-sm font-medium text-slate-700" htmlFor="skills">
            Your skills
          </label>
          <textarea
            className="mt-1 min-h-28 w-full rounded-md border border-slate-300 px-3 py-2"
            id="skills"
            onChange={(event) => setSkills(event.target.value)}
            placeholder="C#, SQL, React, Git"
            value={skills}
          />
        </div>

        <button
          className="w-full rounded-lg bg-indigo-600 px-4 py-2 font-semibold text-white transition hover:bg-indigo-700 disabled:cursor-not-allowed disabled:bg-gray-400 sm:w-fit"
          disabled={isAnalyzing}
          type="submit"
        >
          {isAnalyzing ? "Analyzing..." : "Analyze skills"}
        </button>
      </form>

      {isLoading ? <LoadingSpinner label="Loading analysis..." /> : null}

      {result ? (
        <div className="space-y-4">
          <SectionCard>
            <h2 className="text-lg font-bold text-slate-950">{result.careerPathName}</h2>
            <p className="mt-2 text-3xl font-bold">
              {result.matchPercentage}% match
            </p>
            <p className="mt-1 text-sm text-slate-600">
              {result.matchedSkillCount} matched, {result.missingSkillCount}{" "}
              missing out of {result.totalRequiredSkills} required skills.
            </p>
            <div className="mt-4">
              <ProgressBar value={result.matchPercentage} tone="indigo" />
            </div>
          </SectionCard>

          <div className="grid gap-4 lg:grid-cols-2">
            <SkillList title="Matched skills" skills={result.matchedSkills} />
            <SkillList title="Missing skills" skills={result.missingSkills} />
          </div>
          <SkillList
            title="Recommended learning priority"
            skills={result.recommendedLearningPriority}
          />
        </div>
      ) : !isLoading ? (
        <EmptyState
          title="No analysis yet"
          description="Enter your skills and run an analysis to see your match percentage."
        />
      ) : null}
    </section>
  );
}

export default SkillGap;
