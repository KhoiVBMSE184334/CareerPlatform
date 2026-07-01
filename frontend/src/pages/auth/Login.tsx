import { isAxiosError } from "axios";
import { type FormEvent, useState } from "react";
import { Link, useNavigate } from "react-router-dom";

import { ErrorAlert } from "../../components/common";
import { login } from "../../services/authService";
import { getDashboardPath } from "../../utils/auth";

type LoginForm = {
  email: string;
  password: string;
};

type LoginErrors = Partial<Record<keyof LoginForm | "form", string>>;

function getApiErrorMessage(error: unknown) {
  console.error(error);

  if (isAxiosError(error)) {
    console.error("Login API error", {
      status: error.response?.status,
      data: error.response?.data,
      message: error.message,
    });

    const data = error.response?.data as
      | { message?: string; errors?: Record<string, string[]> }
      | undefined;

    if (data?.message) {
      return data.message;
    }

    const firstError = Object.values(data?.errors ?? {})[0]?.[0];

    if (firstError) {
      return firstError;
    }
  }

  return "Unable to log in. Please try again.";
}

function validateForm(form: LoginForm) {
  const errors: LoginErrors = {};

  if (!form.email.trim()) {
    errors.email = "Email is required.";
  }

  if (!form.password) {
    errors.password = "Password is required.";
  }

  return errors;
}

function LoginIllustration() {
  return (
    <div className="relative overflow-hidden rounded-[2rem] border border-white/70 bg-gradient-to-br from-indigo-600 via-blue-600 to-violet-600 p-8 text-white shadow-2xl shadow-indigo-200">
      <div className="absolute -right-16 -top-16 h-44 w-44 rounded-full bg-white/20 blur-3xl" />
      <div className="absolute -bottom-20 left-8 h-52 w-52 rounded-full bg-cyan-300/20 blur-3xl" />
      <div className="relative space-y-6">
        <p className="text-sm font-bold uppercase tracking-wider text-indigo-100">
          CareerPlatform
        </p>
        <h1 className="text-4xl font-bold tracking-tight">
          Build a focused software engineering roadmap.
        </h1>
        <p className="max-w-xl text-lg leading-8 text-indigo-50">
          Choose a career path, measure your skill gaps, chat with an AI mentor,
          and shape a portfolio that looks ready for interviews.
        </p>
        <svg
          aria-label="Software engineer studying illustration"
          className="mt-8 h-auto w-full"
          fill="none"
          role="img"
          viewBox="0 0 520 300"
          xmlns="http://www.w3.org/2000/svg"
        >
          <rect fill="white" fillOpacity="0.16" height="184" rx="24" width="286" x="190" y="42" />
          <rect fill="white" fillOpacity="0.9" height="16" rx="8" width="132" x="222" y="76" />
          <rect fill="white" fillOpacity="0.36" height="12" rx="6" width="194" x="222" y="112" />
          <rect fill="white" fillOpacity="0.36" height="12" rx="6" width="152" x="222" y="140" />
          <rect fill="white" fillOpacity="0.36" height="12" rx="6" width="176" x="222" y="168" />
          <circle cx="130" cy="98" fill="#FDE68A" r="38" />
          <path d="M72 214C80 160 104 126 139 126C176 126 200 158 208 214" fill="white" fillOpacity="0.9" />
          <rect fill="#111827" height="58" rx="12" width="168" x="54" y="198" />
          <path d="M104 228L126 214L104 200M166 200L144 214L166 228" stroke="#A7F3D0" strokeLinecap="round" strokeWidth="8" />
          <rect fill="white" fillOpacity="0.5" height="8" rx="4" width="76" x="270" y="206" />
        </svg>
      </div>
    </div>
  );
}

function Login() {
  const navigate = useNavigate();
  const [form, setForm] = useState<LoginForm>({ email: "", password: "" });
  const [errors, setErrors] = useState<LoginErrors>({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const validationErrors = validateForm(form);
    setErrors(validationErrors);

    if (Object.keys(validationErrors).length > 0) {
      return;
    }

    setIsSubmitting(true);

    try {
      const session = await login({
        email: form.email.trim(),
        password: form.password,
      });

      navigate(getDashboardPath(session.user.role), { replace: true });
    } catch (error) {
      setErrors({ form: getApiErrorMessage(error) });
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <section className="mx-auto grid min-h-[70vh] max-w-6xl items-center gap-8 lg:grid-cols-[1fr_440px]">
      <div className="hidden lg:block">
        <LoginIllustration />
      </div>

      <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-xl shadow-slate-200/60 sm:p-8">
        <div>
          <p className="text-sm font-semibold uppercase tracking-wide text-indigo-600">
            Welcome back
          </p>
          <h1 className="mt-2 text-2xl font-bold tracking-tight text-slate-950">
            Login
          </h1>
          <p className="mt-2 text-sm leading-6 text-slate-600">
            Access your roadmap, mentor chat, and portfolio workspace.
          </p>
        </div>

        <form className="mt-6 space-y-4" onSubmit={handleSubmit}>
          {errors.form ? (
            <ErrorAlert message={errors.form} />
          ) : null}

          <div>
            <label className="text-sm font-semibold text-slate-700" htmlFor="email">
              Email
            </label>
            <input
              className="mt-1 w-full rounded-xl border border-slate-300 bg-white px-4 py-3 outline-none transition focus:border-indigo-500 focus:ring-4 focus:ring-indigo-100"
              id="email"
              name="email"
              onChange={(event) =>
                setForm((current) => ({ ...current, email: event.target.value }))
              }
              type="email"
              value={form.email}
            />
            {errors.email ? (
              <p className="mt-1 text-sm text-red-600">{errors.email}</p>
            ) : null}
          </div>

          <div>
            <label
              className="text-sm font-semibold text-slate-700"
              htmlFor="password"
            >
              Password
            </label>
            <input
              className="mt-1 w-full rounded-xl border border-slate-300 bg-white px-4 py-3 outline-none transition focus:border-indigo-500 focus:ring-4 focus:ring-indigo-100"
              id="password"
              name="password"
              onChange={(event) =>
                setForm((current) => ({
                  ...current,
                  password: event.target.value,
                }))
              }
              type="password"
              value={form.password}
            />
            {errors.password ? (
              <p className="mt-1 text-sm text-red-600">{errors.password}</p>
            ) : null}
          </div>

          <button
            className="w-full rounded-xl bg-indigo-600 px-4 py-3 font-semibold text-white shadow-sm transition hover:bg-indigo-700 disabled:cursor-not-allowed disabled:bg-gray-400"
            disabled={isSubmitting}
            type="submit"
          >
            {isSubmitting ? "Logging in..." : "Login"}
          </button>
        </form>

        <p className="mt-5 text-center text-sm text-slate-600">
          No account yet?{" "}
          <Link className="font-semibold text-indigo-700 hover:underline" to="/register">
            Register
          </Link>
        </p>
      </div>
    </section>
  );
}

export default Login;
