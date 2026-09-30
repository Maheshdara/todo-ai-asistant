"use client";

import { useEffect, useState } from "react";
import { createTodo, updateTodo, Todo } from "@/services/api";

interface TodoFormProps {
  todo?: Todo | null;
  onSuccess: () => void;
  onCancel: () => void;
}

export default function TodoForm({
  todo,
  onSuccess,
  onCancel,
}: TodoFormProps) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [dueDate, setDueDate] = useState("");
  const [isCompleted, setIsCompleted] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const isEdit = !!todo;

  useEffect(() => {
    if (todo) {
      setTitle(todo.title);
      setDescription(todo.description ?? "");
      setDueDate(
        todo.dueDate
          ? todo.dueDate.substring(0, 10)
          : ""
      );
      setIsCompleted(todo.isCompleted);
    } else {
      setTitle("");
      setDescription("");
      setDueDate("");
      setIsCompleted(false);
    }
  }, [todo]);

  const handleSubmit = async (
    e: React.FormEvent
  ) => {
    e.preventDefault();

    if (!title.trim()) {
      setError("Title is required");
      return;
    }

    try {
      setLoading(true);
      setError("");

      if (isEdit && todo) {
        await updateTodo(todo.todoId, {
          title,
          description,
          isCompleted,
          dueDate: dueDate || undefined,
        });
      } else {
        await createTodo({
          title,
          description,
          dueDate: dueDate || undefined,
        });
      }

      onSuccess();
    } catch (error) {
      if (error instanceof Error) {
        setError(error.message);
      } else {
        setError("Something went wrong");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="form-overlay">
      <div className="todo-form">

        <div className="form-header">
          <h2>
            {isEdit ? "Edit Todo" : "Add Todo"}
          </h2>

          <button
            type="button"
            className="close-btn"
            onClick={onCancel}
          >
            ×
          </button>
        </div>

        {error && (
          <div className="form-error">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit}>

          <div className="form-group">
            <label>Title</label>

            <input
              type="text"
              value={title}
              onChange={(e) =>
                setTitle(e.target.value)
              }
              placeholder="Enter todo title"
            />
          </div>

          <div className="form-group">
            <label>Description</label>

            <textarea
              value={description}
              onChange={(e) =>
                setDescription(e.target.value)
              }
              placeholder="Enter description"
              rows={4}
            />
          </div>

          <div className="form-group">
            <label>Due Date</label>

            <input
              type="date"
              value={dueDate}
              onChange={(e) =>
                setDueDate(e.target.value)
              }
            />
          </div>

          {isEdit && (
            <div className="checkbox-group">
              <input
                type="checkbox"
                checked={isCompleted}
                onChange={(e) =>
                  setIsCompleted(e.target.checked)
                }
              />

              <label>Completed</label>
            </div>
          )}

          <div className="form-actions">

            <button
              type="button"
              className="cancel-btn"
              onClick={onCancel}
            >
              Cancel
            </button>

            <button
              type="submit"
              className="save-btn"
              disabled={loading}
            >
              {loading
                ? "Saving..."
                : isEdit
                ? "Update Todo"
                : "Add Todo"}
            </button>

          </div>

        </form>
      </div>
    </div>
  );
}