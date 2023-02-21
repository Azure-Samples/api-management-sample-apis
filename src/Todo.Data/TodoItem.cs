// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

namespace Todo.Data;

public enum TodoItemState
{
    Todo,
    InProgress,
    Done
}

/// <summary>
/// The model definition for the TodoItem.
/// </summary>
public class TodoItem : TodoBaseModel
{
    public TodoItem(Guid listId, string name) : base(name)
    {
        ListId = listId;
    }

    /// <summary>
    /// The list.
    /// </summary>
    public TodoList? List { get; set; }

    /// <summary>
    /// The ID of the list.
    /// </summary>
    public Guid ListId { get; set; }

    public TodoItemState State { get; set; } = TodoItemState.Todo;
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? CompletedDate { get; set; }
    
}
