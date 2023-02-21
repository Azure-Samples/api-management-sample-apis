// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Todo.RestApi.Extensions;
using Todo.RestApi.Models;
using Todo.RestApi.Services;
using Todo.RestApi.Services.DTO;

namespace Todo.RestApi.Controllers;

[ApiController]
[Route("/todo/lists")]
[ServiceExceptionFilter]
public class ListsController : ControllerBase
{
    private readonly ITodoDataService service;
    private readonly IHttpContextAccessor contextAccessor;

    public ListsController(ITodoDataService service, IHttpContextAccessor contextAccessor)
    {
        this.service = service;
        this.contextAccessor = contextAccessor;
    }

    /// <summary>
    /// Returns the base URI of the current request.
    /// </summary>
    /// <returns></returns>
    private string? GetBaseUri() => Utils.GetBaseUri(contextAccessor.HttpContext?.Request ?? Request);

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(Page<TodoList>))]
    public Page<TodoList> GetListsAsync([FromQuery(Name = "$skip")] int? skip = null, [FromQuery(Name = "$top")] int? batchSize = null)
    {
        Utils.ValidatePaging(skip, batchSize);
        int skipValue = skip ?? 0;
        var totalCount = service.GetTodoLists().Count();
        var items = service.GetTodoLists()
            .Skip(skipValue).Take(Utils.GetBatchSize(batchSize))
            .ToList();
        return new Page<TodoList>(items, skipValue + items.Count < totalCount, new Uri($"{GetBaseUri()}?$skip={skipValue + items.Count}&$top={Utils.GetBatchSize(batchSize)}"));
    } 

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(201, Type = typeof(TodoList))]
    public async Task<ActionResult> CreateListAsync([FromBody] CreateUpdateTodoList list, CancellationToken cancellationToken = default)
    {
        var entity = await service.CreateTodoListAsync(list, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetListAsync), new { list_id = entity.Id }, entity);
    }

    [HttpGet("{list_id}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(TodoList))]
    [ProducesResponseType(404)]
    [ActionName(nameof(GetListAsync))]
    public Task<TodoList> GetListAsync([FromRoute] string list_id, CancellationToken cancellationToken = default)
        => service.GetTodoListByIdAsync(list_id, cancellationToken);

    [HttpPut("{list_id}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(TodoList))]
    [ProducesResponseType(404)]
    public Task<TodoList> ReplaceListAsync([FromRoute] string list_id, [FromBody] CreateUpdateTodoList list, CancellationToken cancellationToken = default)
        => service.UpdateTodoListAsync(list_id, list, cancellationToken);

    [HttpDelete("{list_id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> DeleteListAsync([FromRoute] string list_id, CancellationToken cancellationToken = default)
    {
        await service.DeleteTodoListAsync(list_id, cancellationToken);
        return NoContent();
    }

    [HttpGet("{list_id}/items")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(Page<TodoItem>))]
    [ProducesResponseType(404)]
    public Page<TodoItem> GetListItemsAsync([FromRoute] string list_id, [FromQuery(Name = "$skip")] int? skip = null, [FromQuery(Name = "$top")] int? batchSize = null)
    {
        Utils.ValidatePaging(skip, batchSize);
        int skipValue = skip ?? 0;

        var totalCount = service.GetTodoItemsInList(list_id).Count();
        var items = service.GetTodoItemsInList(list_id)
            .OrderBy(x => x.CreatedDate)
            .Skip(skipValue).Take(Utils.GetBatchSize(batchSize))
            .ToList();
        return new Page<TodoItem>(items, skipValue + items.Count < totalCount, new Uri($"{GetBaseUri()}?$skip={skipValue + items.Count}&$top={Utils.GetBatchSize(batchSize)}"));
    }

    [HttpPost("{list_id}/items")]
    [Produces("application/json")]
    [ProducesResponseType(201, Type = typeof(TodoItem))]
    [ProducesResponseType(404)]
    public async Task<ActionResult<TodoItem>> CreateListItemAsync([FromRoute] string list_id, [FromBody] CreateUpdateTodoItem item, CancellationToken cancellationToken = default)
    {
        var entity = await service.CreateTodoItemAsync(list_id, item, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetListItemAsync), new { list_id = entity.ListId, item_id = entity.Id }, entity);
    }

    [HttpGet("{list_id}/items/{item_id}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(TodoItem))]
    [ProducesResponseType(404)]
    [ActionName(nameof(GetListItemAsync))]
    public Task<TodoItem> GetListItemAsync([FromRoute] string list_id, [FromRoute] string item_id, CancellationToken cancellationToken = default)
        => service.GetTodoItemByIdAsync(list_id, item_id, cancellationToken);

    [HttpPut("{list_id}/items/{item_id}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(TodoItem))]
    [ProducesResponseType(404)]
    public Task<TodoItem> UpdateListItemAsync([FromRoute] string list_id, [FromRoute] string item_id, [FromBody] CreateUpdateTodoItem item, CancellationToken cancellationToken = default)
        => service.UpdateTodoItemAsync(list_id, item_id, item, cancellationToken);

    [HttpDelete("{list_id}/items/{item_id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> DeleteListItemAsync([FromRoute] string list_id, [FromRoute] string item_id, CancellationToken cancellationToken = default)
    {
        await service.DeleteTodoItemAsync(list_id, item_id, cancellationToken).ConfigureAwait(false);
        return NoContent();
    }

    [HttpGet("{list_id}/state/{state}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(Page<TodoItem>))]
    [ProducesResponseType(404)]
    public Page<TodoItem> GetListItemsByStateAsync([FromRoute] string list_id, [FromRoute] string state, 
        [FromQuery(Name = "$skip")] int? skip = null, [FromQuery(Name = "$top")] int? batchSize = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(state) || !Utils.ParseState(state, out Data.TodoItemState parsedState))
        {
            throw new MalformedInputException($"The item state \'{state}\' is not valid");
        }

        Utils.ValidatePaging(skip, batchSize);
        int skipValue = skip ?? 0;

        var totalCount = service.GetTodoItemsInList(list_id)
            .Where(x => x.State == parsedState)
            .Count();
        var items = service.GetTodoItemsInList(list_id)
            .Where(x => x.State == parsedState)
            .OrderBy(x => x.CreatedDate)
            .Skip(skipValue).Take(Utils.GetBatchSize(batchSize))
            .ToList();
        return new Page<TodoItem>(items, skipValue + items.Count < totalCount, new Uri($"{GetBaseUri()}?$skip={skipValue + items.Count}&$top={Utils.GetBatchSize(batchSize)}"));
    }
}
