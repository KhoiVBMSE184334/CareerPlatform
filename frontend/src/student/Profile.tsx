import { getCurrentUser } from "../utils/auth";
import { Badge, PageHeader, SectionCard } from "../components/common";

function Profile() {
  const user = getCurrentUser();

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Account"
        title="Profile"
        description="Basic account information from your current login session."
      />

      <SectionCard>
        <dl className="grid gap-4 sm:grid-cols-2">
          <div>
            <dt className="text-sm font-semibold text-slate-500">Email</dt>
            <dd className="mt-1 font-semibold">{user?.email || "Unknown"}</dd>
          </div>
          <div>
            <dt className="text-sm font-semibold text-slate-500">Role</dt>
            <dd className="mt-2">
              <Badge tone="indigo">{user?.role || "Student"}</Badge>
            </dd>
          </div>
        </dl>
      </SectionCard>
    </section>
  );
}

export default Profile;
