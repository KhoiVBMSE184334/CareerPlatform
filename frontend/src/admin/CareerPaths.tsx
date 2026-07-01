import { useEffect, useState, type FormEvent } from "react";

import {
  createCareerPath,
  deleteCareerPath,
  getCareerPaths,
  updateCareerPath,
  type CareerPath,
} from "../services/careerPathService";
import {
  EmptyState,
  ErrorAlert,
  LoadingSpinner,
  PageHeader,
  SectionCard,
} from "../components/common";

type CareerPathForm = {
  name: string;
  description: string;
};

const emptyForm: CareerPathForm = {
  name: "",
  description: "",
};

function CareerPaths() {
  const [careerPaths, setCareerPaths] = useState<CareerPath[]>([]);
  const [form, setForm] = useState<CareerPathForm>(emptyForm);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [deletingId, setDeletingId] = useState<number | null>(null);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    getCareerPaths()
      .then(setCareerPaths)
      .catch(() => setError("Unable to load career paths."))
      .finally(() => setIsLoading(false));
  }, []);

  const resetForm = () => {
    setForm(emptyForm);
    setEditingId(null);
  };

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError("");
    setSuccess("");

    if (!form.name.trim()) {
      setError("Career path name is required.");
      return;
    }

    setIsSaving(true);

    const request = {
      name: form.name.trim(),
      description: form.description.trim() || null,
    };

    try {
      if (editingId) {
        const updatedCareerPath = await updateCareerPath(editingId, request);
        setCareerPaths((current) =>
          current.map((careerPath) =>
            careerPath.careerPathId === editingId
              ? updatedCareerPath
              : careerPath,
          ),
        );
        setSuccess("Career path updated.");
      } else {
        const createdCareerPath = await createCareerPath(request);
        setCareerPaths((current) => [...current, createdCareerPath]);
        setSuccess("Career path created.");
      }

      resetForm();
    } catch {
      setError("Unable to save this career path.");
    } finally {
      setIsSaving(false);
    }
  };

  const handleEdit = (careerPath: CareerPath) => {
    setEditingId(careerPath.careerPathId);
    setForm({
      name: careerPath.name,
      description: careerPath.description ?? "",
    });
    setError("");
    setSuccess("");
  };

  const handleDelete = async (careerPathId: number) => {
    setDeletingId(careerPathId);
    setError("");
    setSuccess("");

    try {
      await deleteCareerPath(careerPathId);
      setCareerPaths((current) =>
        current.filter((careerPath) => careerPath.careerPathId !== careerPathId),
      );
      setSuccess("Career path deleted.");
      if (editingId === careerPathId) {
        resetForm();
      }
    } catch {
      setError("Unable to delete this career path.");
    } finally {
      setDeletingId(null);
    }
  };

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Catalog"
        title="Career Path Management"
        description="Create and maintain the career paths students can select."
      />

      <form
        className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm"
        onSubmit={handleSubmit}
      >
        <h2 className="text-lg font-bold text-slate-950">
          {editingId ? "Edit career path" : "Create career path"}
        </h2>

        {error ? <div className="mt-4"><ErrorAlert message={error} /></div> : null}

        {success ? (
          <p className="mt-4 rounded-xl border border-emerald-200 bg-emerald-50 px-3 py-2 text-sm font-medium text-emerald-700">
            {success}
          </p>
        ) : null}

        <div className="mt-4 grid gap-4 lg:grid-cols-[1fr_2fr]">
          <div>
            <label className="text-sm font-semibold text-slate-700" htmlFor="name">
              Name
            </label>
            <input
              className="mt-1 w-full rounded-xl border border-slate-300 px-4 py-3 outline-none transition focus:border-indigo-500 focus:ring-4 focus:ring-indigo-100"
              id="name"
              onChange={(event) =>
                setForm((current) => ({ ...current, name: event.target.value }))
              }
              value={form.name}
            />
          </div>
          <div>
            <label
              className="text-sm font-semibold text-slate-700"
              htmlFor="description"
            >
              Description
            </label>
            <input
              className="mt-1 w-full rounded-xl border border-slate-300 px-4 py-3 outline-none transition focus:border-indigo-500 focus:ring-4 focus:ring-indigo-100"
              id="description"
              onChange={(event) =>
                setForm((current) => ({
                  ...current,
                  description: event.target.value,
                }))
              }
              value={form.description}
            />
          </div>
        </div>

        <div className="mt-4 flex flex-wrap gap-3">
          <button
            className="rounded-lg bg-indigo-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-indigo-700 disabled:cursor-not-allowed disabled:bg-gray-400"
            disabled={isSaving}
            type="submit"
          >
            {isSaving ? "Saving..." : editingId ? "Update" : "Create"}
          </button>
          {editingId ? (
            <button
              className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-semibold text-slate-700 transition hover:bg-slate-50"
              onClick={resetForm}
              type="button"
            >
              Cancel
            </button>
          ) : null}
        </div>
      </form>

      {isLoading ? <LoadingSpinner label="Loading career paths..." /> : null}

      <div className="grid gap-4 lg:grid-cols-2">
        {careerPaths.map((careerPath) => (
          <SectionCard className="transition hover:-translate-y-0.5 hover:shadow-md" key={careerPath.careerPathId}>
            <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
              <div>
                <h2 className="text-lg font-bold text-slate-950">{careerPath.name}</h2>
                <p className="mt-2 text-sm leading-6 text-slate-600">
                  {careerPath.description ?? "No description available."}
                </p>
              </div>
              <div className="flex gap-2">
                <button
                  className="rounded-md border border-zinc-300 px-3 py-2 text-sm font-medium hover:bg-zinc-100"
                  onClick={() => handleEdit(careerPath)}
                  type="button"
                >
                  Edit
                </button>
                <button
                  className="rounded-md border border-red-200 px-3 py-2 text-sm font-medium text-red-700 hover:bg-red-50 disabled:cursor-not-allowed disabled:border-gray-300 disabled:bg-gray-50 disabled:text-gray-400"
                  disabled={deletingId === careerPath.careerPathId}
                  onClick={() => handleDelete(careerPath.careerPathId)}
                  type="button"
                >
                  {deletingId === careerPath.careerPathId
                    ? "Deleting..."
                    : "Delete"}
                </button>
              </div>
            </div>
          </SectionCard>
        ))}
      </div>

      {!isLoading && careerPaths.length === 0 ? (
        <EmptyState title="No career paths found" />
      ) : null}
    </section>
  );
}

export default CareerPaths;
