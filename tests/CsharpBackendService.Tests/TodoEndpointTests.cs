// <copyright file="TodoEndpointTests.cs" company="CsharpBackendService">
// Copyright (c) CsharpBackendService. All rights reserved.
// </copyright>

namespace CsharpBackendService.Tests;

using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CsharpBackendService;
using CsharpBackendService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

/// <summary>
/// Integration tests for the Todo API endpoints.
/// </summary>
public class TodoEndpointTests
{
    private CustomWebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Set up the test environment before each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    /// <summary>
    /// Clean up resources after each test.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    /// <summary>
    /// Test the GET /api/todos endpoint.
    /// </summary>
    [Test]
    public async Task GetAll_ReturnsSuccessAndEmptyArray_WhenNoTodos()
    {
        // Act
        var response = await _client.GetAsync("/api/todos");
        var todos = await response.Content.ReadFromJsonAsync<IEnumerable<Todo>>();

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(todos, Is.Not.Null);
        Assert.That(todos, Is.Empty);
    }

    /// <summary>
    /// Test the POST /api/todos endpoint.
    /// </summary>
    [Test]
    public async Task Post_ReturnsCreatedAndTodo_WithGeneratedId()
    {
        // Arrange
        var newTodo = new Todo(Guid.Empty, "Test Todo", "Test Description", false);
        var content = new StringContent(
            JsonSerializer.Serialize(newTodo, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/todos", content);
        var createdTodo = await response.Content.ReadFromJsonAsync<Todo>(_jsonOptions);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(createdTodo, Is.Not.Null);
        Assert.That(createdTodo!.id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(createdTodo.title, Is.EqualTo("Test Todo"));
        Assert.That(createdTodo.description, Is.EqualTo("Test Description"));
        Assert.That(createdTodo.done, Is.False);
        Assert.That(response.Headers.Location, Is.Not.Null);
    }

    /// <summary>
    /// Test the GET, PUT, and DELETE endpoints with specific ID.
    /// </summary>
    [Test]
    public async Task CrudOperations_WorkTogether()
    {
        // Arrange - Create a todo
        var newTodo = new Todo(Guid.Empty, "CRUD Test", "Testing full CRUD", false);
        var content = new StringContent(
            JsonSerializer.Serialize(newTodo, _jsonOptions),
            Encoding.UTF8,
            "application/json");
        var createResponse = await _client.PostAsync("/api/todos", content);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<Todo>(_jsonOptions);
        Assert.That(createdTodo, Is.Not.Null);

        // Act & Assert - Get the todo by ID
        var getResponse = await _client.GetAsync($"/api/todos/{createdTodo!.id}");
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var retrievedTodo = await getResponse.Content.ReadFromJsonAsync<Todo>(_jsonOptions);
        Assert.That(retrievedTodo!.id, Is.EqualTo(createdTodo.id));
        Assert.That(retrievedTodo.title, Is.EqualTo("CRUD Test"));

        // Act & Assert - Update the todo
        var updatedTodo = createdTodo with { Title = "Updated Title", Done = true };
        var updateContent = new StringContent(
            JsonSerializer.Serialize(updatedTodo, _jsonOptions),
            Encoding.UTF8,
            "application/json");
        var updateResponse = await _client.PutAsync($"/api/todos/{createdTodo.id}", updateContent);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        // Verify the update
        var getAfterUpdateResponse = await _client.GetAsync($"/api/todos/{createdTodo.id}");
        var todoAfterUpdate = await getAfterUpdateResponse.Content.ReadFromJsonAsync<Todo>(_jsonOptions);
        Assert.That(todoAfterUpdate!.title, Is.EqualTo("Updated Title"));
        Assert.That(todoAfterUpdate.done, Is.True);

        // Act & Assert - Delete the todo
        var deleteResponse = await _client.DeleteAsync($"/api/todos/{createdTodo.id}");
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        // Verify the delete
        var getAfterDeleteResponse = await _client.GetAsync($"/api/todos/{createdTodo.id}");
        Assert.That(getAfterDeleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    /// <summary>
    /// Test the GET endpoint with non-existent ID.
    /// </summary>
    [Test]
    public async Task GetById_ReturnsNotFound_WhenTodoDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/todos/{nonExistentId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    /// <summary>
    /// Test the PUT endpoint with non-existent ID.
    /// </summary>
    [Test]
    public async Task Put_ReturnsNotFound_WhenTodoDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var todo = new Todo(nonExistentId, "Non-existent", "This doesn't exist", false);
        var content = new StringContent(
            JsonSerializer.Serialize(todo, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync($"/api/todos/{nonExistentId}", content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    /// <summary>
    /// Test the DELETE endpoint with non-existent ID.
    /// </summary>
    [Test]
    public async Task Delete_ReturnsNotFound_WhenTodoDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/todos/{nonExistentId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
