// <copyright file="TodoStoreTests.cs" company="CsharpBackendService">
// Copyright (c) CsharpBackendService. All rights reserved.
// </copyright>

namespace CsharpBackendService.Tests;

using CsharpBackendService.Models;
using CsharpBackendService.Services;

/// <summary>
/// Tests for the TodoStore implementation.
/// </summary>
public class TodoStoreTests
{
    private ITodoStore _store = null!;

    /// <summary>
    /// Set up a new TodoStore instance before each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _store = new TodoStore();
    }

    /// <summary>
    /// Test that creating a Todo with an empty ID generates a new GUID.
    /// </summary>
    [Test]
    public void Create_WithEmptyId_GeneratesNewGuid()
    {
        // Arrange
        var todo = new Todo(Guid.Empty, "Test Todo", "Description", false);

        // Act
        var result = _store.Create(todo);

        // Assert
        Assert.That(result.id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.title, Is.EqualTo("Test Todo"));
        Assert.That(result.description, Is.EqualTo("Description"));
        Assert.That(result.done, Is.False);
    }

    /// <summary>
    /// Test that Replace returns false when a non-existent ID is specified.
    /// </summary>
    [Test]
    public void Replace_WithNonExistentId_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var replacement = new Todo(id, "Updated", "Updated description", true);

        // Act
        var result = _store.Replace(id, replacement);

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Test that Delete removes an existing item and returns true.
    /// </summary>
    [Test]
    public void Delete_RemovesExistingItemAndReturnsTrue()
    {
        // Arrange
        var todo = new Todo(Guid.Empty, "Delete Test", "To be deleted", false);
        var created = _store.Create(todo);

        // Act
        var result = _store.Delete(created.id);
        var retrievedAfterDelete = _store.Get(created.id);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(retrievedAfterDelete, Is.Null);
    }

    /// <summary>
    /// Test that GetAll returns all items in the store.
    /// </summary>
    [Test]
    public void GetAll_ReturnsAllItems()
    {
        // Arrange
        var todo1 = new Todo(Guid.Empty, "Task 1", "Description 1", false);
        var todo2 = new Todo(Guid.Empty, "Task 2", "Description 2", true);
        _store.Create(todo1);
        _store.Create(todo2);

        // Act
        var allTodos = _store.GetAll().ToList();

        // Assert
        Assert.That(allTodos, Has.Count.EqualTo(2));
        Assert.That(allTodos.Select(t => t.title), Does.Contain("Task 1"));
        Assert.That(allTodos.Select(t => t.title), Does.Contain("Task 2"));
    }

    /// <summary>
    /// Test that Get returns the correct item or null when not found.
    /// </summary>
    [Test]
    public void Get_ReturnsCorrectItemOrNull()
    {
        // Arrange
        var todo = new Todo(Guid.Empty, "Get Test", "For retrieval", false);
        var created = _store.Create(todo);
        var nonExistentId = Guid.NewGuid();

        // Act
        var retrievedItem = _store.Get(created.id);
        var nullItem = _store.Get(nonExistentId);

        // Assert
        Assert.That(retrievedItem, Is.Not.Null);
        Assert.That(retrievedItem!.title, Is.EqualTo("Get Test"));
        Assert.That(nullItem, Is.Null);
    }
}