// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Todo.Data;

namespace Todo.GraphQLApi.GraphQL;

/// <summary>
/// The input type for saving a TodoList.
/// </summary>
public class SaveTodoListInput
{
    [ID] public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public TodoList CopyTo(TodoList target)
    {
        bool isUpdated = false;

        if (target.Name != Name)               { isUpdated = true; target.Name = Name; }
        if (target.Description != Description) { isUpdated = true; target.Description = Description; }

        if (isUpdated)
            target.UpdatedDate = DateTimeOffset.UtcNow;
        return target;
    }
}

/// <summary>
/// The input type for saving a TodoItem.
/// </summary>
public class SaveTodoItemInput
{
    [ID] public Guid? Id { get; set; }
    [ID] public Guid ListId { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public TodoItemState State { get; set; } = TodoItemState.Todo;
    public string? Description { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? CompletedDate { get; set; }

    public TodoItem CopyTo(TodoItem target)
    {
        bool isUpdated = false;

        if (target.ListId != ListId)               { isUpdated = true; target.ListId = ListId; }
        if (target.Name != Name)                   { isUpdated = true; target.Name = Name; }
        if (target.State != State)                 { isUpdated = true; target.State = State; }
        if (target.Description != Description)     { isUpdated = true; target.Description = Description; }
        if (target.DueDate != DueDate)             { isUpdated = true; target.DueDate = DueDate; }
        if (target.CompletedDate != CompletedDate) { isUpdated = true; target.CompletedDate = CompletedDate; }

        if (isUpdated)
            target.UpdatedDate = DateTimeOffset.UtcNow;
        return target;
    }
}

