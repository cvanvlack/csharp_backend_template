// <copyright file="TodoStore.cs" company="CsharpBackendService">
// Copyright (c) CsharpBackendService. All rights reserved.
// </copyright>

namespace CsharpBackendService.Services;

using System.Collections.Concurrent;
using CsharpBackendService.Models;

/// <summary>
/// An in-memory implementation of <see cref="ITodoStore"/> using a thread-safe dictionary.
/// </summary>
public class TodoStore : ITodoStore
{
    private readonly ConcurrentDictionary<Guid, Todo> _todos = new ();

    /// <inheritdoc/>
    public IEnumerable<Todo> GetAll() => _todos.Values;

    /// <inheritdoc/>
    public Todo? Get(Guid id)
    {
        _todos.TryGetValue(id, out var todo);
        return todo;
    }

    /// <inheritdoc/>
    public Todo Create(Todo todo)
    {
        ArgumentNullException.ThrowIfNull(todo);

        // Generate a new ID if it's empty
        var id = todo.id == Guid.Empty ? Guid.NewGuid() : todo.id;

        // Create a new todo with the assigned ID
        var newTodo = todo with { id = id };

        // Add to the dictionary
        if (!_todos.TryAdd(id, newTodo))
        {
            throw new InvalidOperationException($"Failed to create Todo with ID {id}");
        }

        return newTodo;
    }

    /// <inheritdoc/>
    public bool Replace(Guid id, Todo replacement)
    {
        ArgumentNullException.ThrowIfNull(replacement);

        // Create a new todo with the correct ID to ensure ID is preserved
        var updatedTodo = replacement with { id = id };

        // Get the current value to use in TryUpdate
        if (!_todos.TryGetValue(id, out var currentTodo))
        {
            return false;
        }

        // Try to update with the new value
        return _todos.TryUpdate(id, updatedTodo, currentTodo);
    }

    /// <inheritdoc/>
    public bool Delete(Guid id)
    {
        return _todos.TryRemove(id, out _);
    }
}