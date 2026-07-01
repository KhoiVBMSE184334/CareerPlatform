import { isAxiosError } from "axios";
import { type FormEvent, useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";

import { ErrorAlert } from "../../components/common";
import { register } from "../../services/authService";
import { getDashboardPath } from "../../utils/auth";

type RegisterForm = {
  fullName: string;
  email: string;
  password: string;
};

type RegisterErrors = Partial<Record<keyof RegisterForm | "form", string>>;

const emptyRegisterForm: RegisterForm = {
  fullName: "",
  email: "",
  password: "",
};

const registerFields: Array<{
  id: keyof RegisterForm;
  label: string;
  type: string;
  autoComplete: string;
}> = [
  ["fullName", "Full name", "text", "off"],
  ["email", "Email", "email", "new-email"],
  ["password", "Password", "password", "new-password"],
].map(([id, label, type, autoComplete]) => ({
  id: id as keyof RegisterForm,
  label,
  type,
  autoComplete,
}));

function getApiErrorMessage(error: unknown) {
  console.error(error);

  if (isAxiosError(error)) {
    console.error("Register API error", {
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

  return "Unable to register. Please try again.";
}

function validateForm(form: RegisterForm) {
  const errors: RegisterErrors = {};

  if (!form.fullName.trim()) {
    errors.fullName = "Full name is required.";
  }

  if (!form.email.trim()) {
    errors.email = "Email is required.";
  }

  if (form.password.length < 8) {
    errors.password = "Password must be at least 8 characters.";
  }

  return errors;
}

function RegisterIllustration() {
  return (
    <div className="relative overflow-hidden rounded-[2rem] border border-white/70 bg-gradient-to-br from-violet-600 via-indigo-600 to-blue-600 p-8 text-white shadow-2xl shadow-violet-200">
      <div className="absolute -right-16 -top-16 h-44 w-44 rounded-full bg-white/20 blur-3xl" />
      <div className="absolute -bottom-20 left-8 h-52 w-52 rounded-full bg-emerald-300/20 blur-3xl" />
      <div className="relative space-y-6">
        <p className="text-sm font-bold uppercase tracking-wider text-violet-100">
          Student onboarding
        </p>
        <h1 className="text-4xl font-bold tracking-tight">
          Create a learning roadmap that fits your career goal.
        </h1>
        <p className="max-w-xl text-lg leading-8 text-violet-50">
          Register, pick a career path, analyze skill gaps, and collect portfolio
          projects in one polished workspace.
        </p>
        <svg
          aria-label="Career planning illustration"
          className="mt-8 h-auto w-full"
          fill="none"
          role="img"
          viewBox="0 0 520 300"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path d="M80 220C112 170 146 152 190 166C236 180 254 118 298 98C344 78 392 96 446 62" stroke="white" strokeDasharray="12 12" strokeLinecap="round" strokeWidth="8" />
          <circle cx="82" cy="220" fill="#FDE68A" r="18" />
          <circle cx="190" cy="166" fill="#A7F3D0" r="18" />
          <circle cx="298" cy="98" fill="#BFDBFE" r="18" />
          <circle cx="446" cy="62" fill="#F5D0FE" r="18" />
          <rect fill="white" fillOpacity="0.92" height="94" rx="20" width="168" x="54" y="54" />
          <rect fill="#C7D2FE" height="12" rx="6" width="90" x="82" y="82" />
          <rect fill="#E2E8F0" height="10" rx="5" width="112" x="82" y="108" />
          <rect fill="white" fillOpacity="0.92" height="108" rx="22" width="190" x="274" y="144" />
          <rect fill="#DDD6FE" height="36" rx="12" width="36" x="302" y="174" />
          <rect fill="#E2E8F0" height="11" rx="5.5" width="92" x="356" y="176" />
          <rect fill="#E2E8F0" height="11" rx="5.5" width="68" x="356" y="202" />
          <rect fill="#E2E8F0" height="11" rx="5.5" width="82" x="356" y="226" />
        </svg>
      </div>
    </div>
  );
}

function Register() {
  const navigate = useNavigate();
  const [form, setForm] = useState<RegisterForm>(emptyRegisterForm);
  const [errors, setErrors] = useState<RegisterErrors>({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    setForm(emptyRegisterForm);
    setErrors({});
  }, []);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const validationErrors = validateForm(form);
    setErrors(validationErrors);

    if (Object.keys(validationErrors).length > 0) {
      return;
    }

    setIsSubmitting(true);

    try {
      const session = await register({
        fullName: form.fullName.trim(),
        email: form.email.trim(),
        password: form.password,
      });

      setForm(emptyRegisterForm);
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
        <RegisterIllustration />
      </div>

      <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-xl shadow-slate-200/60 sm:p-8">
        <div>
          <p className="text-sm font-semibold uppercase tracking-wide text-indigo-600">
            New account
          </p>
          <h1 className="mt-2 text-2xl font-bold tracking-tight text-slate-950">
            Register
          </h1>
          <p className="mt-2 text-sm leading-6 text-slate-600">
            Your first account starts as a student profile.
          </p>
        </div>

        <form autoComplete="off" className="mt-6 space-y-4" onSubmit={handleSubmit}>
          {errors.form ? <ErrorAlert message={errors.form} /> : null}

          {registerFields.map(({ id, label, type, autoComplete }) => (
            <div key={id}>
              <label className="text-sm font-semibold text-slate-700" htmlFor={id}>
                {label}
              </label>
              <input
                autoComplete={autoComplete}
                className="mt-1 w-full rounded-xl border border-slate-300 bg-white px-4 py-3 outline-none transition focus:border-indigo-500 focus:ring-4 focus:ring-indigo-100"
                id={id}
                name={id}
                onChange={(event) =>
                  setForm((current) => ({
                    ...current,
                    [id]: event.target.value,
                  }))
                }
                type={type}
                value={form[id]}
              />
              {errors[id] ? (
                <p className="mt-1 text-sm text-red-600">
                  {errors[id]}
                </p>
              ) : null}
            </div>
          ))}

          <button
            className="w-full rounded-xl bg-indigo-600 px-4 py-3 font-semibold text-white shadow-sm transition hover:bg-indigo-700 disabled:cursor-not-allowed disabled:bg-gray-400"
            disabled={isSubmitting}
            type="submit"
          >
            {isSubmitting ? "Creating account..." : "Register"}
          </button>
        </form>

        <p className="mt-5 text-center text-sm text-slate-600">
          Already have an account?{" "}
          <Link className="font-semibold text-indigo-700 hover:underline" to="/login">
            Login
          </Link>
        </p>
      </div>
    </section>
  );
}

export default Register;
