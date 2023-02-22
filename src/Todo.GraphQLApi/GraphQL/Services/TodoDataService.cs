// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Todo.Data;

namespace Todo.GraphQLApi.GraphQL.Services;

/// <summary>
/// The service reference for the Todo application data.
/// </summary>
public class TodoDataService : IAsyncDisposable
{
    private readonly TodoDbContext context;
    private readonly IMapper mapper;

    public TodoDataService(IDbContextFactory<TodoDbContext> contextFactory, IMapper automapper)
    {
        context = contextFactory.CreateDbContext();
        mapper = automapper;
    }

    #region TodoList CRUD
    /// <summary>
    /// Creates a new <see cref="TodoList"/>.
    /// </summary>
    /// <param name="input">The data to use in creating the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> that resolves to the DTO of the saved entity when complete.</returns>
    /// <exception cref="TodoServiceException">if an error occurs.</exception>
    public async Task<DTO.TodoList?> CreateTodoListAsync(SaveTodoListInput input, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new TodoServiceException($"A list name must be specified when creating a TodoList.");
        }

        if (input.Id.HasValue)
        {
            bool exists = await context.TodoLists.AnyAsync(x => x.Id == input.Id, cancellationToken).ConfigureAwait(false);
            if (exists)
            {
                throw new TodoServiceException($"A TodoList with ID {input.Id} already exists.");
            }
        }

        TodoList newlist = new(input.Name.Trim()) { Id = input.Id ?? Guid.NewGuid() };
        var savedEntity = context.TodoLists.Add(input.CopyTo(newlist));
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return mapper.Map<DTO.TodoList>(savedEntity.Entity);
    }

    /// <summary>
    /// Deletes an existing <see cref="TodoList"/>.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns><c>true</c> if the entity was deleted, <c>false</c> otherwise.</returns>
    public async Task<bool> DeleteTodoListAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.TodoLists.SingleOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        if (entity != null)
        {
            context.TodoLists.Remove(entity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Retrieves a <see cref="TodoList"/> and transforms it into the DTO.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> that resolves to the DTO of the entity, or <c>null</c> if the entity does not exist.</returns>
    public async Task<DTO.TodoList?> GetTodoListByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.TodoLists.SingleOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        return entity != null ? mapper.Map<DTO.TodoList>(entity) : null;
    }

    /// <summary>
    /// Queries for all the <see cref="TodoList"/> entities, projecting them into a DTO.
    /// </summary>
    /// <returns>An <see cref="IQueryable{T}"/> of the DTO.</returns>
    public IQueryable<DTO.TodoList> GetTodoLists()
        => context.TodoLists.ProjectTo<DTO.TodoList>(mapper.ConfigurationProvider);

    /// <summary>
    /// Saves a <see cref="TodoList"/>, creating it if it doesn't exist.
    /// </summary>
    /// <param name="input">The data to use in creating the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> that resolves to the DTO of the saved entity when complete.</returns>
    /// <exception cref="TodoServiceException">if an error occurs.</exception>
    public Task<DTO.TodoList?> SaveTodoListAsync(SaveTodoListInput input, CancellationToken cancellationToken = default)
        => input.Id.HasValue && input.Id.Value != Guid.Empty ? UpdateTodoListAsync(input, cancellationToken) : CreateTodoListAsync(input, cancellationToken);

    /// <summary>
    /// Updates an existing <see cref="TodoList"/>.
    /// </summary>
    /// <param name="input">The data to use in updating the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> that resolves to the DTO of the saved entity when complete.</returns>
    /// <exception cref="TodoServiceException">if an error occurs.</exception>
    public async Task<DTO.TodoList?> UpdateTodoListAsync(SaveTodoListInput input, CancellationToken cancellationToken = default)
    {
        if (!input.Id.HasValue || input.Id.Value == Guid.Empty)
        {
            throw new TodoServiceException($"An ID must be specified when updating a TodoList.");
        }
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new TodoServiceException($"A list name must be specified when updating a TodoList.");
        }

        var existinglist = await context.TodoLists.SingleOrDefaultAsync(x => x.Id == input.Id, cancellationToken).ConfigureAwait(false);
        if (existinglist == null)
        {
            throw new TodoServiceException($"A TodoList with ID {input.Id} does not exist.");
        }

        var savedEntity = context.TodoLists.Update(input.CopyTo(existinglist));
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return mapper.Map<DTO.TodoList>(savedEntity.Entity);
    }
    #endregion

    #region TodoItem CRUD
    /// <summary>
    /// Creates a new <see cref="TodoItem"/>.
    /// </summary>
    /// <param name="input">The data to use in creating the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> that resolves to the DTO of the saved entity when complete.</returns>
    /// <exception cref="TodoServiceException">if an error occurs.</exception>
    public async Task<DTO.TodoItem?> CreateTodoItemAsync(SaveTodoItemInput input, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new TodoServiceException($"An item name must be specified when creating a TodoItem.");
        }

        if (input.Id.HasValue)
        {
            bool exists = await context.TodoItems.AnyAsync(x => x.Id == input.Id, cancellationToken).ConfigureAwait(false);
            if (exists)
            {
                throw new TodoServiceException($"A TodoItem with ID {input.Id} already exists.");
            }
        }

        var listExists = await context.TodoLists.AnyAsync(x => x.Id == input.ListId, cancellationToken).ConfigureAwait(false);
        if (!listExists)
        {
            throw new TodoServiceException($"A TodoList with ID {input.ListId} does not exist.");
        }

        TodoItem newitem = new(input.ListId, input.Name.Trim()) { Id = input.Id ?? Guid.NewGuid() };
        var savedEntity = context.TodoItems.Add(input.CopyTo(newitem));
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return mapper.Map<DTO.TodoItem>(savedEntity.Entity);
    }

    /// <summary>
    /// Deletes an existing <see cref="TodoItem"/>.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns><c>true</c> if the entity was deleted, <c>false</c> otherwise.</returns>
    public async Task<bool> DeleteTodoItemAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.TodoItems.SingleOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        if (entity != null)
        {
            context.TodoItems.Remove(entity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Retrieves a <see cref="TodoItem"/> and transforms it into the DTO.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> that resolves to the DTO of the entity, or <c>null</c> if the entity does not exist.</returns>
    public async Task<DTO.TodoItem?> GetTodoItemByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.TodoItems.SingleOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        return entity != null ? mapper.Map<DTO.TodoItem>(entity) : null;
    }

    /// <summary>
    /// Queries for all the <see cref="TodoItem"/> entities, projecting them into a DTO.
    /// </summary>
    /// <returns>An <see cref="IQueryable{T}"/> of the DTO.</returns>
    public IQueryable<DTO.TodoItem> GetTodoItems(Guid listId, TodoItemState? state) => state.HasValue
        ? context.TodoItems.Where(x => x.ListId == listId && x.State == state.Value).ProjectTo<DTO.TodoItem>(mapper.ConfigurationProvider)
        : context.TodoItems.Where(x => x.ListId == listId).ProjectTo<DTO.TodoItem>(mapper.ConfigurationProvider);

    /// <summary>
    /// Saves a <see cref="TodoItem"/>, creating it if it doesn't exist.
    /// </summary>
    /// <param name="input">The data to use in creating the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> that resolves to the DTO of the saved entity when complete.</returns>
    /// <exception cref="TodoServiceException">if an error occurs.</exception>
    public Task<DTO.TodoItem?> SaveTodoItemAsync(SaveTodoItemInput input, CancellationToken cancellationToken = default)
        => input.Id.HasValue && input.Id.Value != Guid.Empty ? UpdateTodoItemAsync(input, cancellationToken) : CreateTodoItemAsync(input, cancellationToken);

    /// <summary>
    /// Updates an existing <see cref="TodoItem"/>.
    /// </summary>
    /// <param name="input">The data to use in updating the new entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> that resolves to the DTO of the saved entity when complete.</returns>
    /// <exception cref="TodoServiceException">if an error occurs.</exception>
    public async Task<DTO.TodoItem?> UpdateTodoItemAsync(SaveTodoItemInput input, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new TodoServiceException($"An item name must be specified when updating a TodoItem.");
        }

        var listExists = await context.TodoLists.AnyAsync(x => x.Id == input.ListId, cancellationToken).ConfigureAwait(false);
        if (!listExists)
        {
            throw new TodoServiceException($"A TodoList with ID {input.ListId} does not exist.");
        }

        if (!input.Id.HasValue || input.Id.Value == Guid.Empty)
        {
            throw new TodoServiceException($"An item ID must be specified when updating a TodoItem.");
        }
        var existingitem = await context.TodoItems.SingleOrDefaultAsync(x => x.Id == input.Id, cancellationToken).ConfigureAwait(false);
        if (existingitem == null)
        {
            throw new TodoServiceException($"A TodoItem with ID {input.Id} does not exist.");
        }

        var savedEntity = context.TodoItems.Update(input.CopyTo(existingitem));
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return mapper.Map<DTO.TodoItem>(savedEntity.Entity);
    }

    #endregion

    #region IAsyncDisposable
    /// <summary>
    /// Disposes of the service.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that resolves when the service is disposed.</returns>
    public ValueTask DisposeAsync()
        => context.DisposeAsync();
    #endregion
}
