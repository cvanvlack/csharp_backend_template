// <copyright file="Todo.cs" company="CsharpBackendService">
// Copyright (c) CsharpBackendService. All rights reserved.
// </copyright>

namespace CsharpBackendService.Models;

/// <summary>
/// Represents a todo item in the system.
/// </summary>
/// <param name="id">The unique identifier for the todo item.</param>
/// <param name="title">The title of the todo item.</param>
/// <param name="description">The detailed description of the todo item.</param>
/// <param name="done">Whether the todo item is completed.</param>
public record Todo(Guid id, string title, string description, bool done);