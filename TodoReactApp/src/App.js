import React, { useState } from 'react';
import TodoItem from './components/TodoItem';
import './App.css';

function App() {
    const [todos, setTodos] = useState([
        { id: 1, name: 'Learn React', isComplete: false },
        { id: 2, name: 'Build a Todo App', isComplete: false },
    ]);

    const toggleTodo = (id) => {
        setTodos(
            todos.map(todo =>
                todo.id === id ? { ...todo, isComplete: !todo.isComplete } : todo
            )
        );
    };

    return (
        <div className="App">
            <h1>Todo List</h1>
            {todos.map(todo => (
                <TodoItem key={todo.id} item={todo} onToggle={toggleTodo} />
            ))}
        </div>
    );
}

export default App;
