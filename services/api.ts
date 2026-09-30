import axios, { AxiosError } from "axios";

export interface Todo {
  todoId: number;
  title: string;
  description?: string;
  isCompleted: boolean;
  dueDate?: string;
  createdDate: string;
}

const API_URL = "https://localhost:7290/api";

const api = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Common error handler
function handleApiError(error: unknown): never {
  if (axios.isAxiosError(error)) {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const axiosError = error as AxiosError<any>;

    console.log(error);
    
    // Backend returned an error response
    if (axiosError.response) {
      const status = axiosError.response.status;

      const message =
        axiosError.response.data?.message ||
        axiosError.response.data?.title ||
        "Something went wrong";

      throw new Error(`${status}: ${message}`);
    }

    // Request was sent but no response received
    if (axiosError.request) {
      throw new Error(
        "Unable to connect to the server. Please check whether the API is running."
      );
    }

    // Error while setting up request
    throw new Error(axiosError.message);
  }

  throw new Error("An unexpected error occurred.");
}

// Get all todos
export async function getTodos(): Promise<Todo[]> {
  try {
    const response = await api.get<Todo[]>("/Todo");

    return response.data;
  } catch (error) {
    handleApiError(error);
  }
}

// Delete todo
export async function deleteTodo(id: number) {
  try {
    const response = await api.delete(`/Todo/${id}`);

    return response.data;
  } catch (error) {
    handleApiError(error);
  }
}

// Create todo
export async function createTodo(todo: {
  title: string;
  description?: string;
  dueDate?: string;
}) {
  try {
    const response = await api.post("/Todo", todo);

    return response.data;
  } catch (error) {
    handleApiError(error);
  }
}

// Update todo
export async function updateTodo(
  id: number,
  todo: {
    title: string;
    description?: string;
    isCompleted: boolean;
    dueDate?: string;
  }
) {
  try {
    const response = await api.put(`/Todo/${id}`, todo);

    return response.data;
  } catch (error) {
    handleApiError(error);
  }
}

// AI Chat
export async function sendAIMessage(message: string) {
  try {
    const response = await api.post("/AI/chat", {
      message: message,
    });

    return response.data;
  } catch (error) {
    handleApiError(error);
  }
} 