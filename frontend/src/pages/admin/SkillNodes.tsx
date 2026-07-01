import { useEffect, useState } from "react";

import {
  getAdminSkillNodes,
  type AdminSkillNode,
} from "../../services/skillNodeService";
import {
  Badge,
  EmptyState,
  ErrorAlert,
  LoadingSpinner,
  PageHeader,
} from "../../components/common";

function SkillNodes() {
  const [skillNodes, setSkillNodes] = useState<AdminSkillNode[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    getAdminSkillNodes()
      .then(setSkillNodes)
      .catch(() => setError("Unable to load skill nodes."))
      .finally(() => setIsLoading(false));
  }, []);

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Curriculum"
        title="Skill Nodes"
        description="View predefined roadmap skills and learning resources."
      />

      {isLoading ? <LoadingSpinner label="Loading skill nodes..." /> : null}

      {error ? <ErrorAlert message={error} /> : null}

      <div className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        <div className="overflow-x-auto">
          <table className="w-full min-w-[760px] text-left text-sm">
            <thead className="bg-slate-50 text-slate-600">
              <tr>
                <th className="px-4 py-3 font-medium">Name</th>
                <th className="px-4 py-3 font-medium">Career Path</th>
                <th className="px-4 py-3 font-medium">Difficulty</th>
                <th className="px-4 py-3 font-medium">Estimated Hours</th>
                <th className="px-4 py-3 font-medium">Resources Count</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {skillNodes.map((skillNode) => (
                <tr className="transition hover:bg-slate-50" key={skillNode.skillNodeId}>
                  <td className="px-4 py-3 font-medium">{skillNode.name}</td>
                  <td className="px-4 py-3 text-slate-600">
                    {skillNode.careerPathName}
                  </td>
                  <td className="px-4 py-3">
                    <Badge tone="indigo">{skillNode.difficulty}</Badge>
                  </td>
                  <td className="px-4 py-3 text-slate-600">
                    {skillNode.estimatedHours ?? "Not set"}
                  </td>
                  <td className="px-4 py-3 text-slate-600">
                    {skillNode.learningResources.length}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {!isLoading && skillNodes.length === 0 ? (
          <div className="p-6">
            <EmptyState title="No skill nodes found" />
          </div>
        ) : null}
      </div>
    </section>
  );
}

export default SkillNodes;
