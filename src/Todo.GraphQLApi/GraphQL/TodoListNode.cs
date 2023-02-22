// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Todo.Data;
using Todo.GraphQLApi.GraphQL.Services;

namespace Todo.GraphQLApi.GraphQL;

[ExtendObjectType<DTO.TodoList>]
public class TodoListNode
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public static IQueryable<DTO.TodoItem> GetItems(TodoDataService service, [Parent] DTO.TodoList list, TodoItemState? state)
        => service.GetTodoItems(list.Id, state);
}
