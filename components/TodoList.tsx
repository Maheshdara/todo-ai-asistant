"use client";

import { useEffect, useState } from "react";
import {
  Todo,
  getTodos,
  deleteTodo,
} from "@/services/api";

import TodoForm from "./TodoForm";
import "../todo.css";
import AIChat from "./AIChat";

export default function TodoList() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [selectedTodo, setSelectedTodo] =
    useState<Todo | null>(null);

  const [showForm, setShowForm] =
    useState(false);
    const [showChat, setShowChat] = useState(false);

  const loadTodos = async () => {
    try {
      const data = await getTodos();
      setTodos(data);
    } catch (error) {
      console.error(error);
    }
  };



useEffect(() => {
  loadTodos();
}, []);

  const handleDelete = async (id: number) => {
    try {
      await deleteTodo(id);

      setTodos((previous) =>
        previous.filter(
          (todo) => todo.todoId !== id
        )
      );
    } catch (error) {
      console.error(error);
    }
  };

  const handleAdd = () => {
    setSelectedTodo(null);
    setShowForm(true);
  };

  const handleEdit = (todo: Todo) => {
    setSelectedTodo(todo);
    setShowForm(true);
  };

  const handleSuccess = async () => {
    setShowForm(false);
    setSelectedTodo(null);

    await loadTodos();
  };

  const handleCancel = () => {
    setShowForm(false);
    setSelectedTodo(null);
  };

  return (
    <div className="todo-container">

      <div className="todo-header">
        <h1>Todo List</h1>

        <button
          className="add-btn"
          onClick={handleAdd}
        >
          + Add Todo
        </button>
      </div>

      <div className="todo-list">

        {todos.map((todo) => (
          <div
            className="todo-card"
            key={todo.todoId}
          >

            <div className="todo-content">

              <h3>{todo.title}</h3>

              {todo.description && (
                <p>{todo.description}</p>
              )}

              <div className="todo-info">

                <span
                  className={
                    todo.isCompleted
                      ? "status completed"
                      : "status pending"
                  }
                >
                  {todo.isCompleted
                    ? "Completed"
                    : "Pending"}
                </span>

                {todo.dueDate && (
                  <span className="due-date">
                    Due:{" "}
                    {new Date(
                      todo.dueDate
                    ).toLocaleDateString()}
                  </span>
                )}

              </div>

            </div>

            <div className="todo-actions">

              <button
                className="edit-btn"
                onClick={() =>
                  handleEdit(todo)
                }
              >
                Edit
              </button>

              <button
                className="delete-btn"
                onClick={() =>
                  handleDelete(todo.todoId)
                }
              >
                Delete
              </button>

            </div>

          </div>
        ))}

      </div>

            <button
  className="ai-chat-button"
  onClick={() => setShowChat(true)}
>
  🤖 AI Chat
</button>




    {showChat && (
  <div className="ai-chat-popup">

    <div className="ai-chat-popup-header">
      <span>🤖 Todo AI Assistant</span>

      <button
        type="button"
        className="ai-close-button"
        onClick={() => 
         { 
          setShowChat(false)
          loadTodos()
         }
        }
      >
        ×
      </button>
    </div>

    <AIChat />

  </div>
)}




    {/* Add/Edit Form */}
    {showForm && (
      <TodoForm
        todo={selectedTodo}
        onSuccess={handleSuccess}
        onCancel={handleCancel}
      />
    )}





    </div>
  );
}