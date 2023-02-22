// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Todo.GraphQLApi.GraphQL.Services;

namespace Todo.GraphQLApi.GraphQL;

[QueryType]
public static class Query
{
    [NodeResolver]
    public static Task<DTO.TodoItem?> GetTodoItemByIdAsync(TodoDataService service, Guid id, CancellationToken cancellationToken = default)
        => service.GetTodoItemByIdAsync(id, cancellationToken);

    [NodeResolver]
    public static Task<DTO.TodoList?> GetTodoListByIdAsync(TodoDataService service, Guid id, CancellationToken cancellationToken = default)
        => service.GetTodoListByIdAsync(id, cancellationToken);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public static IQueryable<DTO.TodoList> GetTodoLists(TodoDataService service)
        => service.GetTodoLists();
}
