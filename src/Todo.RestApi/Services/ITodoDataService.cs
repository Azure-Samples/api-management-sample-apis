// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Todo.Data;
using Todo.RestApi.Extensions;
using Todo.RestApi.Models;

namespace Todo.RestApi.Services;

/// <summary>
/// Definition of the <see cref="TodoDataService"/> implementation.
/// </summary>
public interface ITodoDataService
{
    /// <summary>
    /// Creates a new <see cref="TodoList"/> entity within the database.
    /// </summary>
    /// <param name="input">The definition of the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityExistsException">if an ID is provided and the entity exists.</exception>
    /// <exception cref="MalformedInputException">if the input entity is malformed</exception>
    Task<DTO.TodoList> CreateTodoListAsync(CreateUpdateTodoList input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="TodoList"/> and all associated <see cref="TodoItem"/> entities within the database.
    /// </summary>
    /// <param name="id">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the entity is deleted.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    Task DeleteTodoListAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a <see cref="TodoList"/> based on the ID of the entity.
    /// </summary>
    /// <param name="id">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    Task<DTO.TodoList> GetTodoListByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an <see cref="IQueryable{T}"/> of the <see cref="TodoList"/> entities within the database.
    /// </summary>
    /// <returns>The projected IQueryable for the <see cref="TodoList"/> entities in DTO representation.</returns>
    IQueryable<DTO.TodoList> GetTodoLists();

    /// <summary>
    /// Updates an existing <see cref="TodoList"/>.
    /// </summary>
    /// <param name="id">The globally unique ID of the list.</param>
    /// <param name="input">The definition of the updated entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns> task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the input GUID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input record is malformed.</exception>
    Task<DTO.TodoList> UpdateTodoListAsync(Guid id, CreateUpdateTodoList input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new <see cref="TodoItem"/> entity within the database.
    /// </summary>
    /// <param name="listId">The globally unique ID of the associated list.</param>
    /// <param name="input">The definition of the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the list does not exist.</exception>
    /// <exception cref="EntityExistsException">if an ID is provided and the entity exists.</exception>
    /// <exception cref="MalformedInputException">if the input entity is malformed</exception>
    Task<DTO.TodoItem> CreateTodoItemAsync(Guid listId, CreateUpdateTodoItem input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="TodoItem"/> from the database.
    /// </summary>
    /// <param name="listId">The globally unqiue ID of the list.</param>
    /// <param name="itemId">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the entity is removed.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    Task DeleteTodoItemAsync(Guid listId, Guid itemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a <see cref="TodoItem"/> based on the ID of the entity.
    /// </summary>
    /// <param name="listId">The globally unique ID of the list entity.</param>
    /// <param name="itemId">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    Task<DTO.TodoItem> GetTodoItemByIdAsync(Guid listId, Guid itemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an <see cref="IQueryable{T}"/> of the <see cref="TodoItem"/> entities within a list in the database.
    /// </summary>
    /// <param name="listId">The globally unique ID of the list.</param>
    /// <returns>The projected IQueryable for the <see cref="TodoItem"/> entities in DTO representation.</returns>
    IQueryable<DTO.TodoItem> GetTodoItemsInList(Guid listId);

    /// <summary>
    /// Updates an existing <see cref="TodoItem"/>.
    /// </summary>
    /// <param name="listId">The globally unique ID of the list.</param>
    /// <param name="itemId">The globally unique ID of the item.</param>
    /// <param name="input">The definition of the updated entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns> task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the input GUID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input record is malformed.</exception>
    Task<DTO.TodoItem> UpdateTodoItemAsync(Guid listId, Guid itemId, CreateUpdateTodoItem input, CancellationToken cancellationToken = default);
}

/// <summary>
/// Extension methods for the <see cref="ITodoService"/> that handle string IDs.
/// </summary>
public static class ITodoServiceExtensions
{
    /// <summary>
    /// Deletes a <see cref="TodoList"/> and all associated <see cref="TodoItem"/> entities within the database.
    /// </summary>
    /// <param name="id">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to <c>true</c> if the entity was deleted on completion, and <c>false</c> otherwise.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    public static Task DeleteTodoListAsync(this ITodoDataService service, string id, CancellationToken cancellationToken = default)
        => service.DeleteTodoListAsync(Utils.ParseGuid(id), cancellationToken);

    /// <summary>
    /// Retrieves a <see cref="TodoList"/> based on the ID of the entity.
    /// </summary>
    /// <param name="service">The data service to use.</param>
    /// <param name="id">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    public static Task<DTO.TodoList> GetTodoListByIdAsync(this ITodoDataService service, string id, CancellationToken cancellationToken = default)
        => service.GetTodoListByIdAsync(Utils.ParseGuid(id), cancellationToken);

    /// <summary>
    /// Updates an existing <see cref="TodoList"/>.
    /// </summary>
    /// <param name="service">The data service to use.</param>
    /// <param name="id">The globally unique ID of the list.</param>
    /// <param name="input">The definition of the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns> task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the input GUID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input record is malformed.</exception>
    public static Task<DTO.TodoList> UpdateTodoListAsync(this ITodoDataService service, string id, CreateUpdateTodoList input, CancellationToken cancellationToken = default)
        => service.UpdateTodoListAsync(Utils.ParseGuid(id), input, cancellationToken);

    /// <summary>
    /// Creates a new <see cref="TodoItem"/> entity within the database.
    /// </summary>
    /// <param name="service">The data service to use.</param>
    /// <param name="listId">The globally unique ID of the associated list.</param>
    /// <param name="input">The definition of the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the list does not exist.</exception>
    /// <exception cref="EntityExistsException">if an ID is provided and the entity exists.</exception>
    /// <exception cref="MalformedInputException">if the input entity is malformed</exception>
    public static Task<DTO.TodoItem> CreateTodoItemAsync(this ITodoDataService service, string listId, CreateUpdateTodoItem input, CancellationToken cancellationToken = default)
        => service.CreateTodoItemAsync(Utils.ParseGuid(listId), input, cancellationToken);

    /// <summary>
    /// Deletes a <see cref="TodoItem"/> from the database.
    /// </summary>
    /// <param name="service">The data service to use.</param>
    /// <param name="listId">The globally unqiue ID of the list.</param>
    /// <param name="itemId">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the entity is removed.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    public static Task DeleteTodoItemAsync(this ITodoDataService service, string listId, string itemId, CancellationToken cancellationToken = default)
        => service.DeleteTodoItemAsync(Utils.ParseGuid(listId), Utils.ParseGuid(itemId), cancellationToken);

    /// <summary>
    /// Retrieves a <see cref="TodoItem"/> based on the ID of the entity.
    /// </summary>
    /// <param name="service">The data service to use.</param>
    /// <param name="listId">The globally unique ID of the list entity.</param>
    /// <param name="itemId">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    public static Task<DTO.TodoItem> GetTodoItemByIdAsync(this ITodoDataService service, string listId, string itemId, CancellationToken cancellationToken = default)
        => service.GetTodoItemByIdAsync(Utils.ParseGuid(listId), Utils.ParseGuid(itemId), cancellationToken);

    /// <summary>
    /// Retrieves an <see cref="IQueryable{T}"/> of the <see cref="TodoItem"/> entities within a list in the database.
    /// </summary>
    /// <param name="service">The data service to use.</param>
    /// <param name="listId">The globally unique ID of the list.</param>
    /// <returns>The projected IQueryable for the <see cref="TodoItem"/> entities in DTO representation.</returns>
    public static IQueryable<DTO.TodoItem> GetTodoItemsInList(this ITodoDataService service, string listId)
        => service.GetTodoItemsInList(Utils.ParseGuid(listId));

    /// <summary>
    /// Updates an existing <see cref="TodoItem"/>.
    /// </summary>
    /// <param name="service">The data service to use.</param>
    /// <param name="listId">The globally unique ID of the list.</param>
    /// <param name="itemId">The globally unique ID of the item.</param>
    /// <param name="input">The definition of the updated entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns> task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the input GUID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input record is malformed.</exception>
    public static Task<DTO.TodoItem> UpdateTodoItemAsync(this ITodoDataService service, string listId, string itemId, CreateUpdateTodoItem input, CancellationToken cancellationToken = default)
        => service.UpdateTodoItemAsync(Utils.ParseGuid(listId), Utils.ParseGuid(itemId), input, cancellationToken);
}
