import { useEffect, useState, type FormEvent } from "react";
import { isAxiosError } from "axios";

import {
  getChatSessions,
  sendMentorMessage,
  type ChatSession,
} from "../services/aiService";
import {
  ErrorAlert,
  LoadingSpinner,
  PageHeader,
} from "../components/common";

type LocalMessage = {
  role: "user" | "assistant";
  content: string;
};

function getApiErrorMessage(error: unknown) {
  console.error(error);

  if (isAxiosError(error)) {
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

  return "Unable to send message to AI mentor.";
}

function MentorAvatar() {
  return (
    <svg
      aria-label="AI mentor avatar"
      className="h-28 w-28"
      fill="none"
      role="img"
      viewBox="0 0 120 120"
      xmlns="http://www.w3.org/2000/svg"
    >
      <rect fill="#EEF2FF" height="120" rx="32" width="120" />
      <rect fill="white" height="58" rx="18" stroke="#C7D2FE" strokeWidth="3" width="76" x="22" y="38" />
      <circle cx="46" cy="66" fill="#4F46E5" r="6" />
      <circle cx="74" cy="66" fill="#4F46E5" r="6" />
      <path d="M49 82C56 87 64 87 71 82" stroke="#10B981" strokeLinecap="round" strokeWidth="4" />
      <path d="M60 38V24" stroke="#4F46E5" strokeLinecap="round" strokeWidth="5" />
      <circle cx="60" cy="20" fill="#A78BFA" r="7" />
      <path d="M22 62H12M108 62H98" stroke="#4F46E5" strokeLinecap="round" strokeWidth="5" />
    </svg>
  );
}

function AIMentor() {
  const [message, setMessage] = useState("");
  const [conversation, setConversation] = useState<LocalMessage[]>([]);
  const [sessions, setSessions] = useState<ChatSession[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isSending, setIsSending] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    loadSessions()
      .finally(() => setIsLoading(false));
  }, []);

  const loadSessions = async () => {
    await getChatSessions()
      .then(setSessions)
      .catch(() => setError("Unable to load mentor chat history."));
  };

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const trimmedMessage = message.trim();

    if (!trimmedMessage) {
      setError("Enter a message for the mentor.");
      return;
    }

    setError("");
    setMessage("");
    setConversation((current) => [
      ...current,
      { role: "user", content: trimmedMessage },
    ]);
    setIsSending(true);

    try {
      const response = await sendMentorMessage(trimmedMessage);
      setConversation((current) => [
        ...current,
        { role: "assistant", content: response.response },
      ]);
      setError("");
      await loadSessions();
    } catch (sendError) {
      setError(getApiErrorMessage(sendError));
    } finally {
      setIsSending(false);
    }
  };

  return (
    <section className="space-y-6">
      <PageHeader
        eyebrow="Guidance"
        title="AI Mentor"
        description="Ask focused questions about career direction, missing skills, and practical next steps."
      />

      <div className="relative overflow-hidden rounded-2xl border border-indigo-100 bg-white/95 p-5 shadow-sm backdrop-blur">
        <div className="pointer-events-none absolute -right-12 -top-16 h-40 w-40 rounded-full bg-violet-200/70 blur-3xl" />
        <div className="relative flex flex-col gap-5 md:flex-row md:items-center">
          <div className="shrink-0">
            <MentorAvatar />
          </div>
          <div>
            <p className="text-sm font-semibold uppercase tracking-wide text-indigo-600">
              Personalized guidance
            </p>
            <h2 className="mt-2 text-2xl font-bold text-slate-950">
              Ask me about backend, frontend, roadmap planning, portfolio
              building, and career guidance.
            </h2>
            <p className="mt-2 max-w-3xl text-sm leading-6 text-slate-600">
              I will use your selected path, completed roadmap skills, missing
              skills, and portfolio projects to keep advice practical.
            </p>
          </div>
        </div>
      </div>

      {error ? <ErrorAlert message={error} /> : null}

      <div className="grid gap-4 xl:grid-cols-[1fr_320px]">
        <div className="overflow-hidden rounded-2xl border border-slate-200 bg-white/95 shadow-sm backdrop-blur">
          <div className="max-h-[560px] min-h-96 space-y-3 overflow-y-auto bg-slate-50/70 p-5">
            {conversation.length ? (
              conversation.map((item, index) => (
                <div
                  className={`rounded-2xl px-4 py-3 text-sm leading-6 shadow-sm ${
                    item.role === "user"
                      ? "ml-auto max-w-xl rounded-br-md bg-indigo-600 text-white"
                      : "mr-auto max-w-xl rounded-bl-md border border-slate-200 bg-white text-slate-800"
                  }`}
                  key={`${item.role}-${index}`}
                >
                  {item.content}
                </div>
              ))
            ) : (
              <div className="rounded-2xl border border-dashed border-slate-300 bg-white p-5 text-sm text-slate-600">
                Start with a question like: What should I learn next for backend
                development?
              </div>
            )}
            {isSending ? (
              <p className="text-sm font-medium text-slate-500">Mentor is thinking...</p>
            ) : null}
          </div>

          <form
            className="sticky bottom-0 flex flex-col gap-3 border-t border-slate-200 bg-white p-4 sm:flex-row"
            onSubmit={handleSubmit}
          >
            <input
              className="min-w-0 flex-1 rounded-xl border border-slate-300 px-4 py-3 outline-none transition focus:border-indigo-500 focus:ring-4 focus:ring-indigo-100"
              onChange={(event) => setMessage(event.target.value)}
              placeholder="Ask your mentor..."
              value={message}
            />
            <button
              className="rounded-xl bg-indigo-600 px-5 py-3 font-semibold text-white transition hover:bg-indigo-700 disabled:cursor-not-allowed disabled:bg-gray-400"
              disabled={isSending || !message.trim()}
              type="submit"
            >
              {isSending ? "Sending..." : "Send"}
            </button>
          </form>
        </div>

        <aside className="rounded-2xl border border-slate-200 bg-white/95 p-5 shadow-sm backdrop-blur">
          <h2 className="text-lg font-semibold">Recent sessions</h2>
          {isLoading ? (
            <div className="mt-3">
              <LoadingSpinner label="Loading sessions..." />
            </div>
          ) : sessions.length ? (
            <div className="mt-3 space-y-3">
              {sessions.slice(0, 5).map((session) => (
                <div
                  className="rounded-xl border border-slate-200 bg-slate-50 p-3 transition hover:bg-white"
                  key={session.sessionId}
                >
                  <p className="text-sm font-medium">
                    {new Date(session.createdAt).toLocaleString()}
                  </p>
                  <p className="mt-1 text-sm text-slate-600">
                    {session.messages.length} messages
                  </p>
                </div>
              ))}
            </div>
          ) : (
            <p className="mt-3 text-sm text-slate-600">No sessions yet.</p>
          )}
        </aside>
      </div>
    </section>
  );
}

export default AIMentor;
