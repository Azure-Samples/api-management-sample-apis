// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.RestApi.Extensions;
using Todo.RestApi.Models;

namespace Todo.RestApi.Services;

public class TodoDataService : ITodoDataService
{
    private readonly TodoDbContext context;
    private readonly IMapper mapper;

    public TodoDataService(TodoDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    /// <summary>
    /// Creates a new <see cref="TodoList"/> entity within the database.
    /// </summary>
    /// <param name="input">The definition of the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityExistsException">if an ID is provided and the entity exists.</exception>
    /// <exception cref="MalformedInputException">if the input entity is malformed</exception>
    public async Task<DTO.TodoList> CreateTodoListAsync(CreateUpdateTodoList input, CancellationToken cancellationToken = default)
    {
        var list = new TodoList(input.Name) { Description = input.Description, UpdatedDate = DateTimeOffset.UtcNow };
        var entity = context.TodoLists.Add(list);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return mapper.Map<DTO.TodoList>(entity.Entity);
    }

    /// <summary>
    /// Deletes a <see cref="TodoList"/> and all associated <see cref="TodoItem"/> entities within the database.
    /// </summary>
    /// <param name="id">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the entity is deleted on completion.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    public async Task DeleteTodoListAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var list = await context.TodoLists.SingleOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        if (list == null)
        {
            throw new EntityMissingException($"A list with ID \'{id}\' does not exist.");
        }
        context.TodoLists.Remove(list);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a <see cref="TodoList"/> based on the ID of the entity.
    /// </summary>
    /// <param name="id">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    public async Task<DTO.TodoList> GetTodoListByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var list = await context.TodoLists.SingleOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        if (list == null)
        {
            throw new EntityMissingException($"A list with ID \'{id}\' does not exist.");
        }
        return mapper.Map<DTO.TodoList>(list);
    }

    /// <summary>
    /// Retrieves an <see cref="IQueryable{T}"/> of the <see cref="TodoList"/> entities within the database.
    /// </summary>
    /// <returns>The projected IQueryable for the <see cref="TodoList"/> entities in DTO representation.</returns>
    public IQueryable<DTO.TodoList> GetTodoLists()
        => context.TodoLists.ProjectTo<DTO.TodoList>(mapper.ConfigurationProvider);

    /// <summary>
    /// Updates an existing <see cref="TodoList"/>.
    /// </summary>
    /// <param name="id">The globally unique ID of the list.</param>
    /// <param name="input">The definition of the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns> task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the input GUID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input record is malformed.</exception>
    public async Task<DTO.TodoList> UpdateTodoListAsync(Guid id, CreateUpdateTodoList input, CancellationToken cancellationToken = default)
    {
        var existinglist = await context.TodoLists.SingleOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        if (existinglist == null)
        {
            throw new EntityMissingException($"A list with ID \'{id}\' does not exist.");
        }

        existinglist.Name = input.Name;
        existinglist.Description = input.Description;
        existinglist.UpdatedDate = DateTimeOffset.UtcNow;

        var entity = context.TodoLists.Update(existinglist);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return mapper.Map<DTO.TodoList>(entity.Entity);
    }

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
    public async Task<DTO.TodoItem> CreateTodoItemAsync(Guid listId, CreateUpdateTodoItem input, CancellationToken cancellationToken = default)
    {
        var list = await context.TodoLists.SingleOrDefaultAsync(x => x.Id == listId, cancellationToken).ConfigureAwait(false);
        if (list == null)
        {
            throw new EntityMissingException($"A list with ID \'{listId}\' does not exist.");
        }

        var newitem = new TodoItem(list.Id!.Value, input.Name)
        {
            Description = input.Description,
            DueDate = input.DueDate,
            CompletedDate = input.CompletedDate,
            UpdatedDate = DateTimeOffset.UtcNow
        };
        if (Utils.ParseState(input.State, out TodoItemState state))
        {
            newitem.State = state;
        } 
        else
        {
            throw new MalformedInputException($"The state value \'{input.State}\' is invalid.");
        }
        list.UpdatedDate = DateTimeOffset.UtcNow;
        context.TodoLists.Update(list);
        var entity = context.TodoItems.Add(newitem);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return mapper.Map<DTO.TodoItem>(entity.Entity);
    }

    /// <summary>
    /// Deletes a <see cref="TodoItem"/> from the database.
    /// </summary>
    /// <param name="listId">The globally unqiue ID of the list.</param>
    /// <param name="itemId">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the entity is removed.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    public async Task DeleteTodoItemAsync(Guid listId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var list = await context.TodoLists.SingleOrDefaultAsync(x => x.Id == listId, cancellationToken).ConfigureAwait(false);
        if (list == null)
        {
            throw new EntityMissingException($"A list with ID \'{listId}\' does not exist.");
        }
        var entity = await context.TodoItems.SingleOrDefaultAsync(x => x.ListId == listId && x.Id == itemId, cancellationToken).ConfigureAwait(false);
        if (entity == null)
        {
            throw new EntityMissingException($"An item in list \'{listId}\' with ID \'{itemId}\' does not exist.");
        }
        list.UpdatedDate = DateTimeOffset.UtcNow;
        context.TodoLists.Update(list);
        context.TodoItems.Remove(entity);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a <see cref="TodoItem"/> based on the ID of the entity.
    /// </summary>
    /// <param name="listId">The globally unique ID of the list entity.</param>
    /// <param name="itemId">The globally unique ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the DTO representation of the entity on completion.</returns>
    /// <exception cref="EntityMissingException">if the ID does not exist.</exception>
    /// <exception cref="MalformedInputException">if the input GUID is malformed.</exception>
    public async Task<DTO.TodoItem> GetTodoItemByIdAsync(Guid listId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var entity = await context.TodoItems.SingleOrDefaultAsync(x => x.ListId == listId && x.Id == itemId, cancellationToken).ConfigureAwait(false);
        if (entity == null)
        {
            throw new EntityMissingException($"An item in list \'{listId}\' with ID \'{itemId}\' does not exist.");
        }
        return mapper.Map<DTO.TodoItem>(entity);
    }

    /// <summary>
    /// Retrieves an <see cref="IQueryable{T}"/> of the <see cref="TodoItem"/> entities within a list in the database.
    /// </summary>
    /// <returns>The projected IQueryable for the <see cref="TodoItem"/> entities in DTO representation.</returns>
    public IQueryable<DTO.TodoItem> GetTodoItemsInList(Guid listId)
        => context.TodoItems.Where(x => x.ListId == listId).ProjectTo<DTO.TodoItem>(mapper.ConfigurationProvider);

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
    public async Task<DTO.TodoItem> UpdateTodoItemAsync(Guid listId, Guid itemId, CreateUpdateTodoItem input, CancellationToken cancellationToken = default)
    {
        var list = await context.TodoLists.SingleOrDefaultAsync(x => x.Id == listId, cancellationToken).ConfigureAwait(false);
        if (list == null)
        {
            throw new EntityMissingException($"A list with ID \'{listId}\' does not exist.");
        }
        var existingitem = await context.TodoItems.SingleOrDefaultAsync(x => x.ListId == listId && x.Id == itemId, cancellationToken).ConfigureAwait(false);
        if (existingitem == null)
        {
            throw new EntityMissingException($"An item in list \'{listId}\' with ID \'{itemId}\' does not exist.");
        }

        existingitem.Name = input.Name;
        existingitem.Description = input.Description;
        existingitem.CompletedDate = input.CompletedDate;
        existingitem.DueDate = input.DueDate;
        existingitem.UpdatedDate = DateTimeOffset.UtcNow;
        if (Utils.ParseState(input.State, out TodoItemState state))
        {
            existingitem.State = state;
        }
        else
        {
            throw new MalformedInputException($"The state value \'{input.State}\' is invalid.");
        }

        list.UpdatedDate = DateTimeOffset.UtcNow;
        context.TodoLists.Update(list);
        var entity = context.TodoItems.Update(existingitem);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return mapper.Map<DTO.TodoItem>(entity.Entity);
    }
}
