import api from "../api/api";

export type ChatMessage = {
  messageId: string;
  role: string;
  content: string;
  createdAt: string;
};

export type ChatSession = {
  sessionId: string;
  createdAt: string;
  messages: ChatMessage[];
};

export type ChatResponse = {
  sessionId: string;
  response: string;
  createdAt: string;
};

export async function sendMentorMessage(message: string) {
  const { data } = await api.post<ChatResponse>("/api/chat", { message });
  return data;
}

export async function getChatSessions() {
  const { data } = await api.get<ChatSession[]>("/api/chat/sessions");
  return data;
}
