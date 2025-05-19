// <copyright file="Todo.cs" company="CsharpBackendService">
// Copyright (c) CsharpBackendService. All rights reserved.
// </copyright>

namespace CsharpBackendService.Models;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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
public record Todo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Todo"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the todo item.</param>
    /// <param name="title">The title of the todo item.</param>
    /// <param name="description">The detailed description of the todo item.</param>
    /// <param name="done">Whether the todo item is completed.</param>
    public Todo(Guid id, string title, string description, bool done)
    {
        Id = id;
        Title = title;
        Description = description;
        Done = done;
    }

    /// <summary>
    /// Gets the unique identifier for the todo item.
    /// For backwards compatibility with existing code.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with an uppercase letter", Justification = "Maintaining API compatibility")]
    [SuppressMessage("Design", "IDE1006:Naming Rule Violation", Justification = "Maintaining API compatibility")]
    [JsonIgnore]
    public Guid id => Id;

    /// <summary>
    /// Gets the title of the todo item.
    /// For backwards compatibility with existing code.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with an uppercase letter", Justification = "Maintaining API compatibility")]
    [SuppressMessage("Design", "IDE1006:Naming Rule Violation", Justification = "Maintaining API compatibility")]
    [JsonIgnore]
    public string title => Title;

    /// <summary>
    /// Gets the detailed description of the todo item.
    /// For backwards compatibility with existing code.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with an uppercase letter", Justification = "Maintaining API compatibility")]
    [SuppressMessage("Design", "IDE1006:Naming Rule Violation", Justification = "Maintaining API compatibility")]
    [JsonIgnore]
    public string description => Description;

    /// <summary>
    /// Gets a value indicating whether the todo item is completed.
    /// For backwards compatibility with existing code.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with an uppercase letter", Justification = "Maintaining API compatibility")]
    [SuppressMessage("Design", "IDE1006:Naming Rule Violation", Justification = "Maintaining API compatibility")]
    [JsonIgnore]
    public bool done => Done;

    /// <summary>
    /// Gets the unique identifier for the todo item with proper naming convention.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the title of the todo item with proper naming convention.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; init; }

    /// <summary>
    /// Gets the detailed description of the todo item with proper naming convention.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; init; }

    /// <summary>
    /// Gets a value indicating whether the todo item is completed with proper naming convention.
    /// </summary>
    [JsonPropertyName("done")]
    public bool Done { get; init; }

    /// <summary>
    /// Converts a Todo record to a TodoResponse.
    /// </summary>
    /// <returns>A new TodoResponse object.</returns>
    public TodoResponse ToResponse() => new TodoResponse
    {
        Id = Id,
        Title = Title,
        Description = Description,
        Done = Done,
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
