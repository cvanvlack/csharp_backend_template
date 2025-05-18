// <copyright file="ITodoStore.cs" company="CsharpBackendService">
// Copyright (c) CsharpBackendService. All rights reserved.
// </copyright>

namespace CsharpBackendService.Services;

using CsharpBackendService.Models;

/// <summary>
/// Defines the contract for a storage service for Todo items.
/// </summary>
public interface ITodoStore
{
    /// <summary>
    /// Gets all todo items.
    /// </summary>
    /// <returns>A collection of all todo items.</returns>
    IEnumerable<Todo> GetAll();

    /// <summary>
    /// Gets a specific todo item by ID.
    /// </summary>
    /// <param name="id">The ID of the todo item to retrieve.</param>
    /// <returns>The todo item if found; otherwise, null.</returns>
    Todo? Get(Guid id);

    /// <summary>
    /// Creates a new todo item.
    /// </summary>
    /// <param name="todo">The todo item to create.</param>
    /// <returns>The created todo item.</returns>
    Todo Create(Todo todo);

    /// <summary>
    /// Replaces an existing todo item.
    /// </summary>
    /// <param name="id">The ID of the todo item to replace.</param>
    /// <param name="replacement">The new todo item data.</param>
    /// <returns>True if the item was replaced; otherwise, false.</returns>
    bool Replace(Guid id, Todo replacement);

    /// <summary>
    /// Deletes a todo item.
    /// </summary>
    /// <param name="id">The ID of the todo item to delete.</param>
    /// <returns>True if the item was deleted; otherwise, false.</returns>
    bool Delete(Guid id);
}