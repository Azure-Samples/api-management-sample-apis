// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Todo.Data;

namespace Todo.RestApi.Models;

/// <summary>
/// The input record for creating or updating a <see cref="TodoItem"/>.
/// </summary>
/// <param name="Name">The replacement name for the entity.</param>
/// <param name="State">The updated state for the entity.</param>
/// <param name="DueDate">The updated due date for the entity.</param>
/// <param name="CompletedDate">The updated completion date for the entity.</param>
/// <param name="Description">The updated description for the entity.</param>
public record CreateUpdateTodoItem(
    string Name,
    string? State,
    DateTimeOffset? DueDate,
    DateTimeOffset? CompletedDate,
    string? Description
);
