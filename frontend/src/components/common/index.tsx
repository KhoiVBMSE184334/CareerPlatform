import type { ButtonHTMLAttributes, ReactNode } from "react";
import { Link, type LinkProps } from "react-router-dom";

type PageHeaderProps = {
  eyebrow?: string;
  title: string;
  description?: string;
  action?: ReactNode;
};

type SectionCardProps = {
  children: ReactNode;
  className?: string;
};

type BadgeProps = {
  children: ReactNode;
  tone?: "slate" | "indigo" | "emerald" | "amber" | "violet" | "red";
};

type ProgressBarProps = {
  value: number;
  tone?: "indigo" | "emerald" | "amber" | "violet";
};

type StatCardProps = {
  icon?: string;
  label: string;
  value: string | number;
  description?: string;
  progress?: number;
  to?: string;
  tone?: "indigo" | "emerald" | "amber" | "violet" | "slate";
};

type EmptyStateProps = {
  title: string;
  description?: string;
  action?: ReactNode;
  illustration?: ReactNode;
};

type ActionButtonProps = ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: "primary" | "secondary" | "danger";
};

const badgeClasses = {
  slate: "bg-slate-100 text-slate-700 ring-slate-200",
  indigo: "bg-indigo-50 text-indigo-700 ring-indigo-100",
  emerald: "bg-emerald-50 text-emerald-700 ring-emerald-100",
  amber: "bg-amber-50 text-amber-800 ring-amber-100",
  violet: "bg-violet-50 text-violet-700 ring-violet-100",
  red: "bg-red-50 text-red-700 ring-red-100",
};

const progressClasses = {
  indigo: "bg-indigo-600",
  emerald: "bg-emerald-500",
  amber: "bg-amber-500",
  violet: "bg-violet-500",
};

const statToneClasses = {
  indigo: "bg-indigo-50 text-indigo-700",
  emerald: "bg-emerald-50 text-emerald-700",
  amber: "bg-amber-50 text-amber-800",
  violet: "bg-violet-50 text-violet-700",
  slate: "bg-slate-100 text-slate-700",
};

function clampPercent(value: number) {
  return Math.min(100, Math.max(0, value));
}

export function PageHeader({ eyebrow, title, description, action }: PageHeaderProps) {
  return (
    <div className="relative overflow-hidden rounded-2xl border border-white/80 bg-white/90 p-6 shadow-lg shadow-slate-200/60 backdrop-blur">
      <div className="pointer-events-none absolute -right-10 -top-12 h-36 w-36 rounded-full bg-indigo-200/70 blur-3xl" />
      <div className="pointer-events-none absolute -bottom-16 left-10 h-32 w-32 rounded-full bg-violet-200/60 blur-3xl" />
      <div className="relative flex flex-col gap-4 md:flex-row md:items-end md:justify-between">
        <div>
          {eyebrow ? (
            <p className="text-xs font-bold uppercase tracking-wider text-indigo-600">
              {eyebrow}
            </p>
          ) : null}
          <h1 className="mt-2 text-2xl font-bold tracking-tight text-slate-950 md:text-3xl">
            {title}
          </h1>
          {description ? (
            <p className="mt-2 max-w-3xl text-sm leading-6 text-slate-600 md:text-base">
              {description}
            </p>
          ) : null}
        </div>
        {action ? <div className="shrink-0">{action}</div> : null}
      </div>
    </div>
  );
}

export function SectionCard({ children, className = "" }: SectionCardProps) {
  return (
    <div
      className={`rounded-2xl border border-slate-200 bg-white/95 p-5 shadow-sm backdrop-blur transition duration-200 ${className}`}
    >
      {children}
    </div>
  );
}

export function Badge({ children, tone = "slate" }: BadgeProps) {
  return (
    <span
      className={`inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold ring-1 ${badgeClasses[tone]}`}
    >
      {children}
    </span>
  );
}

export function ProgressBar({ value, tone = "indigo" }: ProgressBarProps) {
  return (
    <div className="h-2.5 overflow-hidden rounded-full bg-slate-100">
      <div
        className={`h-full rounded-full transition-all ${progressClasses[tone]}`}
        style={{ width: `${clampPercent(value)}%` }}
      />
    </div>
  );
}

export function StatCard({
  icon,
  label,
  value,
  description,
  progress,
  to,
  tone = "indigo",
}: StatCardProps) {
  const content = (
    <div className="h-full rounded-2xl border border-slate-200 bg-white/95 p-5 shadow-sm backdrop-blur transition duration-200 hover:-translate-y-1 hover:scale-[1.01] hover:border-indigo-200 hover:shadow-lg">
      <div className="flex items-start justify-between gap-4">
        <div>
          <p className="text-sm font-medium text-slate-500">{label}</p>
          <p className="mt-2 text-3xl font-bold tracking-tight text-slate-950">
            {value}
          </p>
        </div>
        {icon ? (
          <div
            className={`grid h-11 w-11 place-items-center rounded-xl text-sm font-bold ${statToneClasses[tone]}`}
          >
            {icon}
          </div>
        ) : null}
      </div>
      {description ? (
        <p className="mt-3 text-sm leading-6 text-slate-600">{description}</p>
      ) : null}
      {typeof progress === "number" ? (
        <div className="mt-4">
          <ProgressBar value={progress} tone={tone === "slate" ? "indigo" : tone} />
        </div>
      ) : null}
    </div>
  );

  return to ? (
    <Link className="block h-full" to={to}>
      {content}
    </Link>
  ) : (
    content
  );
}

export function LoadingSpinner({ label = "Loading..." }: { label?: string }) {
  return (
    <div className="flex items-center gap-3 rounded-2xl border border-slate-200 bg-white p-5 text-sm font-medium text-slate-600 shadow-sm">
      <span className="h-4 w-4 animate-spin rounded-full border-2 border-indigo-200 border-t-indigo-600" />
      {label}
    </div>
  );
}

export function EmptyState({ title, description, action, illustration }: EmptyStateProps) {
  return (
    <div className="rounded-2xl border border-dashed border-slate-300 bg-white/95 p-8 text-center shadow-sm backdrop-blur">
      {illustration ? (
        <div className="mx-auto mb-5 flex max-w-xs justify-center">{illustration}</div>
      ) : null}
      <p className="text-base font-semibold text-slate-950">{title}</p>
      {description ? (
        <p className="mx-auto mt-2 max-w-lg text-sm leading-6 text-slate-600">
          {description}
        </p>
      ) : null}
      {action ? <div className="mt-5">{action}</div> : null}
    </div>
  );
}

export function ErrorAlert({ message }: { message: string }) {
  return (
    <div className="rounded-2xl border border-red-200 bg-red-50 p-4 text-sm font-medium text-red-700 shadow-sm">
      {message}
    </div>
  );
}

export function ActionButton({
  variant = "primary",
  className = "",
  ...props
}: ActionButtonProps) {
  const variantClass =
    variant === "primary"
      ? "bg-indigo-600 text-white hover:bg-indigo-700 disabled:bg-gray-400"
      : variant === "danger"
        ? "border border-red-200 text-red-700 hover:bg-red-50 disabled:border-gray-300 disabled:bg-gray-50 disabled:text-gray-400"
        : "border border-slate-300 bg-white text-slate-700 hover:bg-slate-50 disabled:opacity-60";

  return (
    <button
      className={`rounded-lg px-4 py-2 text-sm font-semibold transition disabled:cursor-not-allowed ${variantClass} ${className}`}
      {...props}
    />
  );
}

export function ActionLink({
  className = "",
  ...props
}: LinkProps & { className?: string }) {
  return (
    <Link
      className={`inline-flex items-center justify-center rounded-lg bg-indigo-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-indigo-700 ${className}`}
      {...props}
    />
  );
}
