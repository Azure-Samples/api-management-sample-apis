// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Todo.Data;

namespace Todo.GraphQLApi.GraphQL.DTO;

public class TodoItem
{
    /// <summary>
    /// The ID of the entity.
    /// </summary>
    [ID]
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>
    /// The name of the entity
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the entity
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The date that the entity was created.
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// The date that the entity was last updated.
    /// </summary>
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// The ID of the list.
    /// </summary>
    [ID]
    public Guid ListId { get; set; }

    /// <summary>
    /// The current state of the item.
    /// </summary>
    public TodoItemState State { get; set; } = TodoItemState.Todo;

    /// <summary>
    /// The due date for the item.
    /// </summary>
    public DateTimeOffset? DueDate { get; set; }

    /// <summary>
    /// The completed date for the item.
    /// </summary>
    public DateTimeOffset? CompletedDate { get; set; }
}
