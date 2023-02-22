// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Todo.GraphQLApi.GraphQL.Services;

namespace Todo.GraphQLApi.GraphQL;

[MutationType]
public static class Mutation
{
    [UseMutationConvention(PayloadFieldName = "success")]
    public static Task<bool> DeleteTodoItemAsync(TodoDataService service, [ID] Guid id, CancellationToken cancellationToken = default)
        => service.DeleteTodoItemAsync(id, cancellationToken);

    [UseMutationConvention(PayloadFieldName = "success")]
    public static Task<bool> DeleteTodoListAsync(TodoDataService service, [ID] Guid id, CancellationToken cancellationToken = default)
        => service.DeleteTodoListAsync(id, cancellationToken);

    [Error(typeof(TodoServiceException))]
    public static Task<DTO.TodoItem?> SaveTodoItemAsync(TodoDataService service, SaveTodoItemInput input, CancellationToken cancellationToken = default)
        => service.SaveTodoItemAsync(input, cancellationToken);

    [Error(typeof(TodoServiceException))]
    public static Task<DTO.TodoList?> SaveTodoListAsync(TodoDataService service, SaveTodoListInput input, CancellationToken cancellationToken = default)
        => service.SaveTodoListAsync(input, cancellationToken);
}
