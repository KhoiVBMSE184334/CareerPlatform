import { Link } from "react-router-dom";

const problems = [
  {
    label: "Options",
    title: "Too many career options",
    description:
      "Students often see backend, frontend, AI, data, mobile, and DevOps at once without a clear place to begin.",
  },
  {
    label: "Plans",
    title: "Outdated learning plans",
    description:
      "Generic roadmaps can miss the skills and project practice expected by current internship teams.",
  },
  {
    label: "Skills",
    title: "Skill gaps before internship",
    description:
      "It is hard to know which missing skills matter most until interview season is already close.",
  },
  {
    label: "Work",
    title: "Weak portfolio",
    description:
      "Students need visible, relevant projects that prove they can build real software.",
  },
];

const features = [
  {
    label: "AI",
    title: "AI Mentor",
    description:
      "Get practical, career-focused guidance based on your roadmap progress and missing skills.",
  },
  {
    label: "Map",
    title: "Dynamic Roadmap",
    description:
      "Follow clear learning steps with skills, estimated effort, and resources organized by career path.",
  },
  {
    label: "Gap",
    title: "Skill Gap Analysis",
    description:
      "Compare your current skills against a chosen role and focus on what matters next.",
  },
  {
    label: "Git",
    title: "GitHub Portfolio Tracking",
    description:
      "Showcase projects, technology stacks, and repository links in one internship-ready profile.",
  },
];

const careerPaths = [
  "Backend Developer",
  "Frontend Developer",
  "Full Stack Developer",
  "Mobile Developer",
  "DevOps Engineer",
  "Data Engineer",
  "AI Engineer",
];

function HeroIllustration() {
  return (
    <div className="relative mx-auto w-full max-w-xl">
      <div className="absolute -left-6 top-10 h-36 w-36 rounded-full bg-emerald-300/30 blur-3xl" />
      <div className="absolute -right-6 bottom-8 h-44 w-44 rounded-full bg-violet-300/30 blur-3xl" />
      <div className="relative overflow-hidden rounded-[2rem] border border-white/70 bg-white/80 p-5 shadow-2xl shadow-indigo-100 backdrop-blur">
        <svg
          aria-label="Software engineering roadmap illustration"
          className="h-auto w-full"
          fill="none"
          role="img"
          viewBox="0 0 560 420"
          xmlns="http://www.w3.org/2000/svg"
        >
          <rect fill="#EEF2FF" height="420" rx="28" width="560" />
          <path
            d="M72 318C118 250 151 235 215 250C268 262 298 216 337 167C380 113 438 92 498 119"
            stroke="#4F46E5"
            strokeDasharray="10 10"
            strokeLinecap="round"
            strokeWidth="8"
          />
          <rect fill="white" height="110" rx="18" stroke="#CBD5E1" width="178" x="44" y="52" />
          <rect fill="#E0E7FF" height="14" rx="7" width="92" x="72" y="82" />
          <rect fill="#F8FAFC" height="12" rx="6" width="116" x="72" y="112" />
          <rect fill="#F8FAFC" height="12" rx="6" width="84" x="72" y="136" />
          <circle cx="190" cy="108" fill="#10B981" r="18" />
          <path d="M181 108L188 115L201 99" stroke="white" strokeLinecap="round" strokeWidth="5" />

          <rect fill="white" height="130" rx="22" stroke="#CBD5E1" width="210" x="300" y="44" />
          <rect fill="#DBEAFE" height="44" rx="14" width="44" x="326" y="72" />
          <path d="M338 94H358M348 84V104" stroke="#2563EB" strokeLinecap="round" strokeWidth="5" />
          <rect fill="#E0E7FF" height="13" rx="6.5" width="102" x="390" y="78" />
          <rect fill="#F8FAFC" height="11" rx="5.5" width="82" x="390" y="104" />
          <rect fill="#F8FAFC" height="11" rx="5.5" width="104" x="390" y="127" />

          <rect fill="white" height="124" rx="22" stroke="#CBD5E1" width="206" x="74" y="236" />
          <rect fill="#DCFCE7" height="42" rx="13" width="42" x="102" y="266" />
          <path d="M113 287L121 295L135 277" stroke="#16A34A" strokeLinecap="round" strokeWidth="5" />
          <rect fill="#E0E7FF" height="13" rx="6.5" width="96" x="162" y="268" />
          <rect fill="#F8FAFC" height="11" rx="5.5" width="74" x="162" y="294" />
          <rect fill="#F8FAFC" height="11" rx="5.5" width="92" x="162" y="317" />

          <rect fill="white" height="132" rx="22" stroke="#CBD5E1" width="198" x="324" y="236" />
          <rect fill="#FEF3C7" height="42" rx="13" width="42" x="352" y="266" />
          <path d="M363 288H383M373 278V298" stroke="#D97706" strokeLinecap="round" strokeWidth="5" />
          <rect fill="#E0E7FF" height="13" rx="6.5" width="94" x="412" y="267" />
          <rect fill="#F8FAFC" height="11" rx="5.5" width="74" x="412" y="294" />
          <rect fill="#F8FAFC" height="11" rx="5.5" width="90" x="412" y="318" />

          <circle cx="72" cy="318" fill="#4F46E5" r="16" />
          <circle cx="215" cy="250" fill="#10B981" r="16" />
          <circle cx="337" cy="167" fill="#F59E0B" r="16" />
          <circle cx="498" cy="119" fill="#7C3AED" r="16" />
          <path d="M64 318H80M215 242V258M329 167H345M490 119H506" stroke="white" strokeLinecap="round" strokeWidth="5" />
        </svg>
      </div>
    </div>
  );
}

function Home() {
  return (
    <div className="space-y-20 pb-10">
      <section className="grid items-center gap-12 overflow-hidden rounded-[2rem] border border-white/70 bg-gradient-to-br from-white via-indigo-50 to-blue-50 px-6 py-12 shadow-xl shadow-slate-200/70 sm:px-10 lg:grid-cols-[1fr_0.95fr] lg:px-14 lg:py-16">
        <div className="space-y-8">
          <span className="inline-flex rounded-full border border-indigo-200 bg-white/80 px-4 py-2 text-sm font-semibold text-indigo-700 shadow-sm">
            AI-Powered Career Guidance
          </span>

          <div className="space-y-5">
            <h1 className="max-w-4xl text-4xl font-bold leading-tight text-slate-950 sm:text-5xl lg:text-6xl">
              Build Your Software Engineering Career with Confidence
            </h1>
            <p className="max-w-2xl text-lg leading-8 text-slate-600">
              CareerPlatform helps students choose a career path, identify
              skill gaps, build a learning roadmap, and prepare for internships.
            </p>
          </div>

          <div className="flex flex-col gap-3 sm:flex-row">
            <Link
              className="inline-flex items-center justify-center rounded-xl bg-indigo-600 px-6 py-3 text-sm font-semibold text-white shadow-lg shadow-indigo-200 transition hover:-translate-y-0.5 hover:bg-indigo-700"
              to="/register"
            >
              Get Started
            </Link>
            <Link
              className="inline-flex items-center justify-center rounded-xl border border-slate-300 bg-white px-6 py-3 text-sm font-semibold text-slate-700 shadow-sm transition hover:-translate-y-0.5 hover:border-indigo-300 hover:text-indigo-700"
              to="/login"
            >
              Login
            </Link>
          </div>
        </div>

        <HeroIllustration />
      </section>

      <section className="space-y-8">
        <div className="max-w-2xl">
          <p className="text-sm font-semibold uppercase tracking-wide text-indigo-600">
            The student challenge
          </p>
          <h2 className="mt-3 text-3xl font-bold text-slate-950 sm:text-4xl">
            Why students struggle
          </h2>
        </div>
        <div className="grid gap-5 sm:grid-cols-2 lg:grid-cols-4">
          {problems.map((problem) => (
            <article
              className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm transition hover:-translate-y-1 hover:shadow-xl"
              key={problem.title}
            >
              <span className="grid h-11 w-11 place-items-center rounded-xl bg-amber-100 text-sm font-bold text-amber-700">
                {problem.label}
              </span>
              <h3 className="mt-5 text-lg font-bold text-slate-950">
                {problem.title}
              </h3>
              <p className="mt-3 text-sm leading-6 text-slate-600">
                {problem.description}
              </p>
            </article>
          ))}
        </div>
      </section>

      <section className="space-y-8">
        <div className="max-w-2xl">
          <p className="text-sm font-semibold uppercase tracking-wide text-indigo-600">
            Platform features
          </p>
          <h2 className="mt-3 text-3xl font-bold text-slate-950 sm:text-4xl">
            Everything students need to move from plan to portfolio
          </h2>
        </div>
        <div className="grid gap-5 md:grid-cols-2">
          {features.map((feature) => (
            <article
              className="group rounded-2xl border border-slate-200 bg-white p-7 shadow-sm transition hover:-translate-y-1 hover:border-indigo-200 hover:shadow-xl"
              key={feature.title}
            >
              <div className="flex items-start gap-5">
                <span className="grid h-12 w-12 shrink-0 place-items-center rounded-2xl bg-indigo-600 text-sm font-bold text-white shadow-md shadow-indigo-200 transition group-hover:bg-indigo-700">
                  {feature.label}
                </span>
                <div>
                  <h3 className="text-xl font-bold text-slate-950">
                    {feature.title}
                  </h3>
                  <p className="mt-3 leading-7 text-slate-600">
                    {feature.description}
                  </p>
                </div>
              </div>
            </article>
          ))}
        </div>
      </section>

      <section className="space-y-8">
        <div className="flex flex-col justify-between gap-4 sm:flex-row sm:items-end">
          <div className="max-w-2xl">
            <p className="text-sm font-semibold uppercase tracking-wide text-indigo-600">
              Career paths
            </p>
            <h2 className="mt-3 text-3xl font-bold text-slate-950 sm:text-4xl">
              Choose a software engineering direction
            </h2>
          </div>
          <Link
            className="text-sm font-semibold text-indigo-700 transition hover:text-indigo-900"
            to="/register"
          >
            Explore after sign up
          </Link>
        </div>
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
          {careerPaths.map((path, index) => (
            <article
              className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm transition hover:-translate-y-1 hover:border-indigo-200 hover:shadow-lg"
              key={path}
            >
              <span className="grid h-10 w-10 place-items-center rounded-xl bg-gradient-to-br from-indigo-600 to-blue-600 text-sm font-bold text-white">
                {String(index + 1).padStart(2, "0")}
              </span>
              <h3 className="mt-5 text-lg font-bold text-slate-950">{path}</h3>
              <p className="mt-2 text-sm leading-6 text-slate-600">
                Follow a focused roadmap and build projects aligned with this
                role.
              </p>
            </article>
          ))}
        </div>
      </section>

      <section className="overflow-hidden rounded-[2rem] bg-gradient-to-br from-indigo-600 via-blue-600 to-violet-600 px-6 py-12 text-center shadow-2xl shadow-indigo-200 sm:px-10 lg:py-16">
        <div className="mx-auto max-w-3xl space-y-6">
          <h2 className="text-3xl font-bold text-white sm:text-5xl">
            Start building your career today
          </h2>
          <p className="text-lg leading-8 text-indigo-50">
            Turn uncertainty into a clear learning path, practical projects,
            and internship-ready confidence.
          </p>
          <Link
            className="inline-flex items-center justify-center rounded-xl bg-white px-6 py-3 text-sm font-semibold text-indigo-700 shadow-lg transition hover:-translate-y-0.5 hover:bg-indigo-50"
            to="/register"
          >
            Create Account
          </Link>
        </div>
      </section>
    </div>
  );
}

export default Home;
