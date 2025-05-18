// <copyright file="TodosController.cs" company="CsharpBackendService">
// Copyright (c) CsharpBackendService. All rights reserved.
// </copyright>

namespace CsharpBackendService.Controllers;

using CsharpBackendService.Models;
using CsharpBackendService.Services;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// API controller for Todo items.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TodosController : ControllerBase
{
    private readonly ITodoStore _store;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodosController"/> class.
    /// </summary>
    /// <param name="store">The todo store service.</param>
    public TodosController(ITodoStore store)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    /// <summary>
    /// Gets all todo items.
    /// </summary>
    /// <returns>A collection of all todo items.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TodoResponse>))]
    public IActionResult GetAll()
    {
        var todos = _store.GetAll().Select(t => t.ToResponse());
        return Ok(todos);
    }

    /// <summary>
    /// Gets a specific todo item by ID.
    /// </summary>
    /// <param name="id">The ID of the todo item to retrieve.</param>
    /// <returns>The todo item if found; otherwise, 404 Not Found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult GetById(Guid id)
    {
        var todo = _store.Get(id);
        if (todo == null)
        {
            return NotFound();
        }

        return Ok(todo.ToResponse());
    }

    /// <summary>
    /// Creates a new todo item.
    /// </summary>
    /// <param name="todoCreate">The todo item to create.</param>
    /// <returns>The created todo item and a Location header to the new resource.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TodoResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Create([FromBody] TodoCreate todoCreate)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todo = Todo.FromCreate(todoCreate);
        var createdTodo = _store.Create(todo);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdTodo.id },
            createdTodo.ToResponse());
    }

    /// <summary>
    /// Updates an existing todo item.
    /// </summary>
    /// <param name="id">The ID of the todo item to update.</param>
    /// <param name="todoCreate">The updated todo item data.</param>
    /// <returns>The updated todo if successful; otherwise, 404 Not Found.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Update(Guid id, [FromBody] TodoCreate todoCreate)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todo = Todo.FromCreate(todoCreate, id);

        if (!_store.Replace(id, todo))
        {
            return NotFound();
        }

        // Per the OpenAPI spec, return the updated todo in the response
        return Ok(todo.ToResponse());
    }

    /// <summary>
    /// Deletes a todo item.
    /// </summary>
    /// <param name="id">The ID of the todo item to delete.</param>
    /// <returns>No content if successful; otherwise, 404 Not Found.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Delete(Guid id)
    {
        if (_store.Delete(id))
        {
            return NoContent();
        }

        return NotFound();
    }
}