"use client";

import { useState } from "react";
import { sendAIMessage, Todo } from "@/services/api";

interface ChatMessage {
  id: string;
  sender: "user" | "ai";
  message: string;
  todos?: Todo[];
}

export default function AIChat() {
  const [message, setMessage] = useState("");
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [loading, setLoading] = useState(false);

 const handleSend = async () => {
  if (!message.trim() || loading) {
    return;
  }

  const userMessage = message.trim();

  // Show user message immediately
  setMessages((previous) => [
    ...previous,
    {
      id: crypto.randomUUID(),
      sender: "user",
      message: userMessage,
    },
  ]);

  setMessage("");
  setLoading(true);

  try {
    const response = await sendAIMessage(userMessage);

    const aiMessage =
      response?.message || "Request completed successfully.";

    const todos =
      response?.data && Array.isArray(response.data)
        ? response.data
        : undefined;

    // Add AI message only ONCE
    setMessages((previous) => [
      ...previous,
      {
        id: crypto.randomUUID(),
        sender: "ai",
        message: aiMessage,
        todos: todos,
      },
    ]);
  } catch (error) {
    setMessages((previous) => [
      ...previous,
      {
        id: crypto.randomUUID(),
        sender: "ai",
        message:
          error instanceof Error
            ? error.message
            : "Something went wrong.",
      },
    ]);
  } finally {
    setLoading(false);
  }
};

  const handleKeyDown = (
    e: React.KeyboardEvent<HTMLInputElement>
  ) => {
    if (e.key === "Enter") {
      handleSend();
    }
  };

  return (
    <div className="chat-container">

     

      <div className="chat-messages">

        {messages.length === 0 && (
          <div className="chat-welcome">
            <h3>👋 Hello!</h3>

            <p>
              I can help you manage your todos.
            </p>

            <div className="suggestions">
              <button
                onClick={() =>
                  setMessage(
                    "Show my pending todos"
                  )
                }
              >
                Show pending todos
              </button>

              <button
                onClick={() =>
                  setMessage(
                    "Show todos due tomorrow"
                  )
                }
              >
                 {"Tomorrow's todos"}
              </button>

              <button
                onClick={() =>
                  setMessage(
                    "Create a todo to learn TypeScript tomorrow"
                  )
                }
              >
                Create a Todo
              </button>
            </div>
          </div>
        )}

       {messages.map((item,index) => (
  <div
      key={`${item.id}-${index}`}
    className={`message-row ${item.sender}`}
  >
    <div className="message">

      <div className="ai-message-text">
        {item.message}
      </div>

      {item.todos && item.todos.length > 0 && (
        <div className="chat-todo-list">

          {item.todos.map((todo) => (
            <div
              className="chat-todo-card"
              key={todo.todoId}
            >

              <div className="chat-todo-card-header">
                <h4>{todo.title}</h4>

                <span
                  className={
                    todo.isCompleted
                      ? "chat-status completed"
                      : "chat-status pending"
                  }
                >
                  {todo.isCompleted
                    ? "Completed"
                    : "Pending"}
                </span>
              </div>

              {todo.description && (
                <p className="chat-todo-description">
                  {todo.description}
                </p>
              )}

              <div className="chat-todo-details">

                {todo.dueDate && (
                  <span>
                    📅{" "}
                    {new Date(
                      todo.dueDate
                    ).toLocaleDateString()}
                  </span>
                )}

                <span>
                  Created:{" "}
                  {new Date(
                    todo.createdDate
                  ).toLocaleDateString()}
                </span>

              </div>

            </div>
          ))}

        </div>
      )}

    </div>
  </div>
))}

        {loading && (
          <div className="message-row ai">
            <div className="message typing">
              Thinking...
            </div>
          </div>
        )}

      </div>

      <div className="chat-input-area">

        <input
          type="text"
          value={message}
          onChange={(e) =>
            setMessage(e.target.value)
          }
          onKeyDown={handleKeyDown}
          placeholder="Ask about your todos..."
          disabled={loading}
        />

        <button
          onClick={handleSend}
          disabled={
            loading || !message.trim()
          }
        >
          {loading ? "..." : "Send"}
        </button>

      </div>

    </div>
  );
}