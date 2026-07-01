import { useEffect, useState } from "react";

import {
  deleteUser,
  getUsers,
  type AdminUser,
} from "../services/userService";
import {
  Badge,
  EmptyState,
  ErrorAlert,
  LoadingSpinner,
  PageHeader,
} from "../components/common";

function Users() {
  const [users, setUsers] = useState<AdminUser[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [deletingId, setDeletingId] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const loadUsers = () => {
    setIsLoading(true);
    setError("");

    getUsers()
      .then(setUsers)
      .catch(() => setError("Unable to load users."))
      .finally(() => setIsLoading(false));
  };

  useEffect(() => {
    loadUsers();
  }, []);

  const handleDelete = async (userId: string) => {
    const shouldDelete = window.confirm(
      "Are you sure you want to delete this user?",
    );

    if (!shouldDelete) {
      return;
    }

    setDeletingId(userId);
    setError("");
    setSuccess("");

    try {
      await deleteUser(userId);
      setSuccess("User removed successfully.");
      await getUsers().then(setUsers);
    } catch {
      setError("Unable to remove this user.");
    } finally {
      setDeletingId("");
    }
  };

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Accounts"
        title="User Management"
        description="Review platform users and remove accounts when needed."
      />

      {isLoading ? <LoadingSpinner label="Loading users..." /> : null}

      {error ? <ErrorAlert message={error} /> : null}

      {success ? (
        <div className="rounded-2xl border border-emerald-200 bg-emerald-50 p-4 text-sm font-medium text-emerald-700">
          {success}
        </div>
      ) : null}

      <div className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        <div className="overflow-x-auto">
          <table className="w-full min-w-[720px] text-left text-sm">
            <thead className="bg-slate-50 text-slate-600">
              <tr>
                <th className="px-4 py-3 font-medium">Name</th>
                <th className="px-4 py-3 font-medium">Email</th>
                <th className="px-4 py-3 font-medium">Role</th>
                <th className="px-4 py-3 font-medium">Created</th>
                <th className="px-4 py-3 font-medium">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {users.map((user) => (
                <tr className="transition hover:bg-slate-50" key={user.userId}>
                  <td className="px-4 py-3 font-medium">{user.fullName}</td>
                  <td className="px-4 py-3 text-slate-600">{user.email}</td>
                  <td className="px-4 py-3">
                    <Badge tone={user.role === "Admin" ? "violet" : "indigo"}>{user.role}</Badge>
                  </td>
                  <td className="px-4 py-3 text-slate-600">
                    {user.createdAt
                      ? new Date(user.createdAt).toLocaleDateString()
                      : "Unknown"}
                  </td>
                  <td className="px-4 py-3">
                    <button
                      className="rounded-md border border-red-200 px-3 py-2 text-xs font-medium text-red-700 hover:bg-red-50 disabled:cursor-not-allowed disabled:border-gray-300 disabled:bg-gray-50 disabled:text-gray-400"
                      disabled={deletingId === user.userId}
                      onClick={() => handleDelete(user.userId)}
                      type="button"
                    >
                      {deletingId === user.userId ? "Removing..." : "Remove"}
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {!isLoading && users.length === 0 ? (
          <div className="p-6">
            <EmptyState title="No users found" />
          </div>
        ) : null}
      </div>
    </section>
  );
}

export default Users;
