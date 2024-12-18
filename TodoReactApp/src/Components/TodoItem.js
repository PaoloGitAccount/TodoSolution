import React from 'react';

function TodoItem({ item, onToggle }) {
    return (
        <div>
            <input
                type="checkbox"
                checked={item.isComplete}
                onChange={() => onToggle(item.id)}
            />
            {item.name}
        </div>
    );
}

export default TodoItem;
