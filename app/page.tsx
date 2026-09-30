import TodoList from "@/components/TodoList";

export default function Home() {
  return (
    <main style={{ padding: "30px" }}>
      <h1>Todo AI Assistant</h1>

      <TodoList />
    </main>
  );
}