// <copyright file="Todo.cs" company="CsharpBackendService">
// Copyright (c) CsharpBackendService. All rights reserved.
// </copyright>

namespace CsharpBackendService.Models;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Base model for Todo items.
/// </summary>
public record TodoBase
{
    /// <summary>
    /// Gets or sets the title of the todo item.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the todo item.
    /// </summary>
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the todo item is completed.
    /// </summary>
    public bool Done { get; set; } = false;
}

/// <summary>
/// Model for creating a new Todo.
/// </summary>
public record TodoCreate : TodoBase;

/// <summary>
/// Model for Todo responses, includes all fields from TodoBase plus the ID.
/// </summary>
public record TodoResponse : TodoBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the todo item.
    /// </summary>
    [Required]
    public Guid Id { get; set; }
}

/// <summary>
/// Represents a todo item in the system.
/// </summary>
/// <param name="id">The unique identifier for the todo item.</param>
/// <param name="title">The title of the todo item.</param>
/// <param name="description">The detailed description of the todo item.</param>
/// <param name="done">Whether the todo item is completed.</param>
public record Todo(Guid id, string title, string description, bool done)
{
    /// <summary>
    /// Converts a Todo record to a TodoResponse.
    /// </summary>
    /// <returns>A new TodoResponse object.</returns>
    public TodoResponse ToResponse() => new TodoResponse
    {
        Id = id,
        Title = title,
        Description = description,
        Done = done,
    };

    /// <summary>
    /// Creates a new Todo from a TodoCreate request.
    /// </summary>
    /// <param name="create">The TodoCreate request.</param>
    /// <param name="id">Optional ID; if not provided, a new one will be generated.</param>
    /// <returns>A new Todo object.</returns>
    public static Todo FromCreate(TodoCreate create, Guid? id = null)
    {
        ArgumentNullException.ThrowIfNull(create);

        return new Todo(
            id ?? Guid.Empty,
            create.Title,
            create.Description,
            create.Done);
    }
}